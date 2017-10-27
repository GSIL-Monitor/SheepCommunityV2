using System;
using System.Collections.Generic;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Users;
using Sheep.ServiceModel.Users.Entities;

namespace Sheep.ServiceInterface.Users
{
    /// <summary>
    ///     更新用户服务接口。
    /// </summary>
    public class UpdateUserService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(UpdateUserService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置更新用户的校验器。
        /// </summary>
        public IValidator<UserUpdate> UserUpdateValidator { get; set; }

        #endregion

        #region 更新用户

        /// <summary>
        ///     更新用户。
        /// </summary>
        public object Put(UserUpdate request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                UserUpdateValidator.ValidateAndThrow(request, ApplyTo.Put);
            }
            var authRepo = HostContext.AppHost.GetAuthRepository(Request);
            using (authRepo as IDisposable)
            {
                var existingUserAuth = authRepo.GetUserAuth(request.UserId.ToString());
                if (existingUserAuth == null)
                {
                    throw HttpError.NotFound(string.Format(Resources.UserNotFound, request.UserId));
                }
                var newUserAuth = MapToUserAuth(authRepo, existingUserAuth, request);
                var userAuth = ((IUserAuthRepository) authRepo).UpdateUserAuth(existingUserAuth, newUserAuth);
                return new UserUpdateResponse
                       {
                           User = MapToUserDto(userAuth)
                       };
            }
        }

        #endregion

        #region 转换成用户身份

        /// <summary>
        ///     将注册身份的请求转换成用户身份。
        /// </summary>
        public IUserAuth MapToUserAuth(IAuthRepository authRepo, IUserAuth existingUserAuth, UserUpdate request)
        {
            var newUserAuth = authRepo is ICustomUserAuth customUserAuth ? customUserAuth.CreateUserAuth() : new UserAuth();
            newUserAuth.PopulateMissingExtended(existingUserAuth);
            newUserAuth.Meta = existingUserAuth.Meta == null ? new Dictionary<string, string>() : new Dictionary<string, string>(existingUserAuth.Meta);
            newUserAuth.DisplayName = request.DisplayName;
            newUserAuth.Meta["Signature"] = request.Signature;
            newUserAuth.Meta["Description"] = request.Description;
            newUserAuth.BirthDate = request.BirthDate;
            newUserAuth.Gender = request.Gender;
            newUserAuth.PrimaryEmail = request.PrimaryEmail;
            newUserAuth.PhoneNumber = request.PhoneNumber;
            newUserAuth.Country = request.Country;
            newUserAuth.State = request.State;
            newUserAuth.City = request.City;
            newUserAuth.Meta["Guild"] = request.Guild;
            newUserAuth.Company = request.Company;
            newUserAuth.Address = request.Address;
            newUserAuth.Address2 = request.Address2;
            newUserAuth.MailAddress = request.MailAddress;
            newUserAuth.PostalCode = request.PostalCode;
            newUserAuth.TimeZone = request.TimeZone;
            newUserAuth.Culture = request.Culture;
            newUserAuth.Meta["PrivateMessagesSource"] = request.PrivateMessagesSource;
            newUserAuth.Meta["ReceiveEmails"] = request.ReceiveEmails?.ToString();
            newUserAuth.Meta["ReceiveSms"] = request.ReceiveSms?.ToString();
            newUserAuth.Meta["ReceiveCommentNotifications"] = request.ReceiveCommentNotifications?.ToString();
            newUserAuth.Meta["ReceiveConversationNotifications"] = request.ReceiveConversationNotifications?.ToString();
            newUserAuth.Meta["TrackPresence"] = request.TrackPresence?.ToString();
            newUserAuth.Meta["ShareBookmarks"] = request.ShareBookmarks?.ToString();
            newUserAuth.Meta["RequireModeration"] = request.RequireModeration?.ToString();
            return newUserAuth;
        }

        public UserDto MapToUserDto(IUserAuth userAuth)
        {
            if (userAuth.Meta == null)
            {
                userAuth.Meta = new Dictionary<string, string>();
            }
            var user = new UserDto
                       {
                           Id = userAuth.Id,
                           Type = userAuth.Meta.GetValueOrDefault("Type"),
                           UserName = userAuth.UserName,
                           Email = userAuth.Email,
                           DisplayName = userAuth.DisplayName,
                           FullName = userAuth.FullName,
                           FullNameVerified = userAuth.Meta.GetValueOrDefault("FullNameVerified").To(false),
                           Signature = userAuth.Meta.GetValueOrDefault("Signature"),
                           Description = userAuth.Meta.GetValueOrDefault("Description"),
                           AvatarUrl = userAuth.Meta.GetValueOrDefault("AvatarUrl"),
                           CoverPhotoUrl = userAuth.Meta.GetValueOrDefault("CoverPhotoUrl"),
                           BirthDate = userAuth.BirthDate,
                           Gender = userAuth.Gender,
                           PrimaryEmail = userAuth.PrimaryEmail,
                           PhoneNumber = userAuth.PhoneNumber,
                           Country = userAuth.Country,
                           State = userAuth.State,
                           City = userAuth.City,
                           Guild = userAuth.Meta.GetValueOrDefault("Guild"),
                           Company = userAuth.Company,
                           Address = userAuth.Address,
                           Address2 = userAuth.Address2,
                           MailAddress = userAuth.MailAddress,
                           PostalCode = userAuth.PostalCode,
                           TimeZone = userAuth.TimeZone,
                           Culture = userAuth.Culture,
                           PrivateMessagesSource = userAuth.Meta.GetValueOrDefault("PrivateMessagesSource"),
                           ReceiveEmails = userAuth.Meta.GetValueOrDefault("ReceiveEmails").To<bool?>(),
                           ReceiveSms = userAuth.Meta.GetValueOrDefault("ReceiveSms").To<bool?>(),
                           ReceiveCommentNotifications = userAuth.Meta.GetValueOrDefault("ReceiveCommentNotifications").To<bool?>(),
                           ReceiveConversationNotifications = userAuth.Meta.GetValueOrDefault("ReceiveConversationNotifications").To<bool?>(),
                           TrackPresence = userAuth.Meta.GetValueOrDefault("TrackPresence").To<bool?>(),
                           AccountStatus = userAuth.Meta.GetValueOrDefault("AccountStatus"),
                           BanReason = userAuth.Meta.GetValueOrDefault("BanReason"),
                           BannedUntil = userAuth.Meta.GetValueOrDefault("BannedUntil").To<DateTime?>(),
                           RequireModeration = userAuth.Meta.GetValueOrDefault("RequireModeration").To<bool?>(),
                           CreatedDate = userAuth.CreatedDate,
                           ModifiedDate = userAuth.ModifiedDate,
                           LockedDate = userAuth.LockedDate,
                           Points = 0
                       };
            return user;
        }

        #endregion
    }
}