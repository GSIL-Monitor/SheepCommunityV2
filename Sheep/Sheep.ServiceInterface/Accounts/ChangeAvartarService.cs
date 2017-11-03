using System;
using System.Collections.Generic;
using System.Linq;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
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
        ///     获取及设置更改头像的校验器。
        /// </summary>
        public IValidator<AccountChangeAvatar> AccountChangeAvatarValidator { get; set; }

        #endregion

        #region 更改头像

        /// <summary>
        ///     更改头像。
        /// </summary>
        public object Put(AccountChangeAvatar request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                AccountChangeAvatarValidator.ValidateAndThrow(request, ApplyTo.Put);
            }
            var imageName = string.Empty;
            if (!request.SourceUrl.IsNullOrEmpty())
            {
                var imageBuffer = request.SourceUrl.GetBytesFromUrl();
                //imageName = imageBuffer.SaveImages(request.SourceUrl.ImageUrlSuffix(), UserOriginalImagePath, UserStandardImagePath, UserThumbnailImagePath);
            }
            else
            {
                var imageFile = Request.Files.FirstOrDefault(file => file.ContentLength > 0);
                if (imageFile != null)
                {
                    var imageBuffer = imageFile.InputStream.ReadFully();
                    //imageName = imageBuffer.SaveImages(imageFile.FileName.ImageFileSuffix(), UserOriginalImagePath, UserStandardImagePath, UserThumbnailImagePath);
                }
            }
            var session = GetSession();
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
                //newUserAuth.Meta["AvatarUrl"] = request.Signature;
                ((IUserAuthRepository) authRepo).UpdateUserAuth(existingUserAuth, newUserAuth);
                return new AccountChangeAvatarResponse();
            }
        }

        #endregion
    }
}