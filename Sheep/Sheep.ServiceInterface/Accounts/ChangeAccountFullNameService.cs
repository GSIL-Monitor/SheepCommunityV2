using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Aliyun.OSS;
using Aliyun.OSS.Common;
using Aliyun.OSS.Util;
using Netease.Nim;
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
    ///     更改真实姓名服务接口。
    /// </summary>
    public class ChangeAccountFullNameService : ChangeAccountService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ChangeAccountFullNameService));

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
        ///     网易云通信服务客户端。
        /// </summary>
        public INimClient NimClient { get; set; }

        /// <summary>
        ///     获取及设置更改真实姓名的校验器。
        /// </summary>
        public IValidator<AccountChangeFullName> AccountChangeFullNameValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        #endregion

        #region 更改真实姓名

        /// <summary>
        ///     更改真实姓名。
        /// </summary>
        public async Task<object> Put(AccountChangeFullName request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                AccountChangeFullNameValidator.ValidateAndThrow(request, ApplyTo.Put);
            }
            var session = GetSession();
            var existingUserAuth = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(session, null);
            if (existingUserAuth == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, session.UserAuthId));
            }
            string idimageUrl = null;
            if (!request.SourceIdImageUrl.IsNullOrEmpty())
            {
                var imageBuffer = await request.SourceIdImageUrl.GetBytesFromUrlAsync();
                if (imageBuffer != null && imageBuffer.Length > 0)
                {
                    using (var imageStream = new MemoryStream(imageBuffer))
                    {
                        var md5Hash = OssUtils.ComputeContentMd5(imageStream, imageStream.Length);
                        var path = $"users/{session.UserAuthId}/idimages/{Guid.NewGuid():N}.{request.SourceIdImageUrl.GetImageUrlExtension()}";
                        var objectMetadata = new ObjectMetadata
                                             {
                                                 ContentMd5 = md5Hash,
                                                 ContentType = request.SourceIdImageUrl.GetImageUrlExtension().GetImageContentType(),
                                                 ContentLength = imageBuffer.Length,
                                                 CacheControl = "max-age=604800"
                                             };
                        try
                        {
                            await OssClient.PutObjectAsync(AppSettings.GetString(AppSettingsOssNames.OssBucket), path, imageStream, objectMetadata);
                            idimageUrl = $"{AppSettings.GetString(AppSettingsOssNames.OssUrl)}/{path}";
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
                        var path = $"users/{session.UserAuthId}/idimages/{Guid.NewGuid():N}.{imageFile.FileName.GetImageFileExtension()}";
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
                            idimageUrl = $"{AppSettings.GetString(AppSettingsOssNames.OssUrl)}/{path}";
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
            var newUserAuth = AuthRepo is ICustomUserAuth customUserAuth ? customUserAuth.CreateUserAuth() : new UserAuth();
            newUserAuth.PopulateMissingExtended(existingUserAuth);
            newUserAuth.Meta = existingUserAuth.Meta == null ? new Dictionary<string, string>() : new Dictionary<string, string>(existingUserAuth.Meta);
            newUserAuth.FullName = request.FullName?.Replace("\"", "'");
            newUserAuth.Meta["FullNameVerified"] = false.ToString();
            newUserAuth.Meta["IdImageUrl"] = idimageUrl;
            var userAuth = await ((IUserAuthRepositoryExtended) AuthRepo).UpdateUserAuthAsync(existingUserAuth, newUserAuth);
            ResetCache(userAuth);
            await NimClient.PostAsync(new UserUpdateInfoRequest
                                      {
                                          AccountId = userAuth.Id.ToString(),
                                          Name = userAuth.DisplayName,
                                          IconUrl = userAuth.Meta.GetValueOrDefault("AvatarUrl"),
                                          Signature = userAuth.Meta.GetValueOrDefault("Signature"),
                                          Email = userAuth.Email,
                                          BirthDate = userAuth.BirthDate?.ToString("yyyy-MM-dd"),
                                          Mobile = userAuth.PhoneNumber,
                                          Gender = userAuth.Gender.IsNullOrEmpty() ? 0 : (userAuth.Gender == "男" ? 1 : 2)
                                      });
            return new AccountChangeFullNameResponse();
        }

        #endregion
    }
}