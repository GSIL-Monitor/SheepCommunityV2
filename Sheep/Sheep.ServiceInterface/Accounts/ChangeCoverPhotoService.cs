using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Aliyun.OSS;
using Aliyun.OSS.Common;
using Aliyun.OSS.Util;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.Extensions;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Common.Auth;
using Sheep.Common.Settings;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Accounts;

namespace Sheep.ServiceInterface.Accounts
{
    /// <summary>
    ///     更改封面图片服务接口。
    /// </summary>
    public class ChangeCoverPhotoService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ChangeCoverPhotoService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置阿里云对象存储客户端。
        /// </summary>
        public IOss OssClient { get; set; }

        /// <summary>
        ///     获取及设置更改封面图片的校验器。
        /// </summary>
        public IValidator<AccountChangeCoverPhoto> AccountChangeCoverPhotoValidator { get; set; }

        #endregion

        #region 更改封面图片

        /// <summary>
        ///     更改封面图片。
        /// </summary>
        public async Task<object> Put(AccountChangeCoverPhoto request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                AccountChangeCoverPhotoValidator.ValidateAndThrow(request, ApplyTo.Put);
            }
            var session = GetSession();
            var authRepo = HostContext.AppHost.GetAuthRepository(Request);
            using (authRepo as IDisposable)
            {
                var existingUserAuth = await ((IUserAuthRepositoryExtended) authRepo).GetUserAuthAsync(session, null);
                if (existingUserAuth == null)
                {
                    throw HttpError.NotFound(string.Format(Resources.UserNotFound, session.UserAuthId));
                }
                string coverphotoUrl = null;
                if (!request.SourceCoverPhotoUrl.IsNullOrEmpty())
                {
                    var imageBuffer = await request.SourceCoverPhotoUrl.GetBytesFromUrlAsync();
                    if (imageBuffer != null && imageBuffer.Length > 0)
                    {
                        using (var imageStream = new MemoryStream(imageBuffer))
                        {
                            var md5Hash = OssUtils.ComputeContentMd5(imageStream, imageStream.Length);
                            var path = $"users/{session.UserAuthId}/coverphotos/{Guid.NewGuid():N}.{request.SourceCoverPhotoUrl.GetImageUrlExtension()}";
                            var objectMetadata = new ObjectMetadata
                                                 {
                                                     ContentMd5 = md5Hash,
                                                     ContentType = request.SourceCoverPhotoUrl.GetImageUrlExtension().GetImageContentType(),
                                                     ContentLength = imageBuffer.Length,
                                                     CacheControl = "max-age=604800"
                                                 };
                            try
                            {
                                await OssClient.PutObjectAsync(AppSettings.GetString(AppSettingsOssNames.OssBucket), path, imageStream, objectMetadata);
                                coverphotoUrl = $"{AppSettings.GetString(AppSettingsOssNames.OssUrl)}/{path}";
                            }
                            catch (OssException ex)
                            {
                                Log.WarnFormat("Failed with error code: {0}; Error info: {1}. RequestID:{2}\tHostID:{3}", ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
                                throw new HttpError(HttpStatusCode.InternalServerError, ex.ErrorCode, ex.Message);
                            }
                            catch (Exception ex)
                            {
                                Log.WarnFormat("Failed with error info: {0}", ex.Message);
                                throw new HttpError(HttpStatusCode.InternalServerError, ex.Message);
                            }
                        }
                    }
                }
                else
                {
                    var imageFile = Request.Files.FirstOrDefault(file => file.ContentLength > 0);
                    if (imageFile != null)
                    {
                        using (var imageStream = imageFile.InputStream)
                        {
                            var md5Hash = OssUtils.ComputeContentMd5(imageStream, imageStream.Length);
                            var path = $"users/{session.UserAuthId}/coverphotos/{Guid.NewGuid():N}.{imageFile.FileName.GetImageFileExtension()}";
                            var objectMetadata = new ObjectMetadata
                                                 {
                                                     ContentMd5 = md5Hash,
                                                     ContentType = imageFile.ContentType,
                                                     ContentLength = imageFile.ContentLength,
                                                     CacheControl = "max-age=604800"
                                                 };
                            try
                            {
                                await OssClient.PutObjectAsync(AppSettings.GetString(AppSettingsOssNames.OssBucket), path, imageStream, objectMetadata);
                                coverphotoUrl = $"{AppSettings.GetString(AppSettingsOssNames.OssUrl)}/{path}";
                            }
                            catch (OssException ex)
                            {
                                Log.WarnFormat("Failed with error code: {0}; Error info: {1}. RequestID:{2}\tHostID:{3}", ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
                                throw new HttpError(HttpStatusCode.InternalServerError, ex.ErrorCode, ex.Message);
                            }
                            catch (Exception ex)
                            {
                                Log.WarnFormat("Failed with error info: {0}", ex.Message);
                                throw new HttpError(HttpStatusCode.InternalServerError, ex.Message);
                            }
                        }
                    }
                }
                var newUserAuth = authRepo is ICustomUserAuth customUserAuth ? customUserAuth.CreateUserAuth() : new UserAuth();
                newUserAuth.PopulateMissingExtended(existingUserAuth);
                newUserAuth.Meta = existingUserAuth.Meta == null ? new Dictionary<string, string>() : new Dictionary<string, string>(existingUserAuth.Meta);
                newUserAuth.Meta["CoverPhotoUrl"] = coverphotoUrl;
                var user = await ((IUserAuthRepositoryExtended) authRepo).UpdateUserAuthAsync(existingUserAuth, newUserAuth);
                Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/users/{0}", user.Id)).ToArray());
                Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/users/{0}", user.Id)).ToArray());
                Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/users/basic/{0}", user.Id)).ToArray());
                Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/users/basic/{0}", user.Id)).ToArray());
                if (!user.UserName.IsNullOrEmpty())
                {
                    Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/users/show/{0}", user.UserName)).ToArray());
                    Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/users/show/{0}", user.UserName)).ToArray());
                    Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/users/basic/show/{0}", user.UserName)).ToArray());
                    Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/users/basic/show/{0}", user.UserName)).ToArray());
                }
                if (!user.Email.IsNullOrEmpty())
                {
                    Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/users/show/{0}", user.Email)).ToArray());
                    Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/users/show/{0}", user.Email)).ToArray());
                    Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/users/basic/show/{0}", user.Email)).ToArray());
                    Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/users/basic/show/{0}", user.Email)).ToArray());
                }
                return new AccountChangeCoverPhotoResponse
                       {
                           CoverPhotoUrl = coverphotoUrl
                       };
            }
        }

        #endregion
    }
}