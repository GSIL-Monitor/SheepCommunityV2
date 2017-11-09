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
using Sheep.Common.Settings;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Accounts;

namespace Sheep.ServiceInterface.Accounts
{
    /// <summary>
    ///     更改头像服务接口。
    /// </summary>
    public class ChangeAvatarService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ChangeAvatarService));

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
        ///     获取及设置更改头像的校验器。
        /// </summary>
        public IValidator<AccountChangeAvatar> AccountChangeAvatarValidator { get; set; }

        #endregion

        #region 更改头像

        /// <summary>
        ///     更改头像。
        /// </summary>
        public async Task<object> Put(AccountChangeAvatar request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                AccountChangeAvatarValidator.ValidateAndThrow(request, ApplyTo.Put);
            }
            var session = GetSession();
            string avatarUrl = null;
            if (!request.SourceUrl.IsNullOrEmpty())
            {
                var imageBuffer = await request.SourceUrl.GetBytesFromUrlAsync();
                if (imageBuffer != null && imageBuffer.Length > 0)
                {
                    using (var imageStream = new MemoryStream(imageBuffer))
                    {
                        var md5Hash = OssUtils.ComputeContentMd5(imageStream, imageStream.Length);
                        var path = $"users/{session.UserAuthId}/avatars/{md5Hash}.{request.SourceUrl.GetImageUrlExtension()}";
                        var objectMetadata = new ObjectMetadata
                                             {
                                                 ContentMd5 = md5Hash,
                                                 ContentType = request.SourceUrl.GetImageUrlExtension().GetImageContentType(),
                                                 ContentLength = imageBuffer.Length,
                                                 CacheControl = "max-age=604800"
                                             };
                        try
                        {
                            await OssClient.PutObjectAsync(AppSettings.GetString(AppSettingsOssNames.OssBucket), path, imageStream, objectMetadata);
                            avatarUrl = $"{AppSettings.GetString(AppSettingsOssNames.OssUrl)}/{path}";
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
                        var path = $"users/{session.UserAuthId}/avatars/{md5Hash}.{imageFile.FileName.GetImageFileExtension()}";
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
                            avatarUrl = $"{AppSettings.GetString(AppSettingsOssNames.OssUrl)}/{path}";
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
            var authRepo = HostContext.AppHost.GetAuthRepository(Request);
            using (authRepo as IDisposable)
            {
                var existingUserAuth = authRepo.GetUserAuth(session, null);
                if (existingUserAuth == null)
                {
                    throw HttpError.NotFound(string.Format(Resources.UserNotFound, session.UserAuthId));
                }
                var newUserAuth = authRepo is ICustomUserAuth customUserAuth ? customUserAuth.CreateUserAuth() : new UserAuth();
                newUserAuth.PopulateMissingExtended(existingUserAuth);
                newUserAuth.Meta = existingUserAuth.Meta == null ? new Dictionary<string, string>() : new Dictionary<string, string>(existingUserAuth.Meta);
                newUserAuth.Meta["AvatarUrl"] = avatarUrl;
                ((IUserAuthRepository) authRepo).UpdateUserAuth(existingUserAuth, newUserAuth);
                return new AccountChangeAvatarResponse();
            }
        }

        #endregion
    }
}