﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Common.Auth;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Users;
using Sheep.ServiceModel.Users.Entities;

namespace Sheep.ServiceInterface.Users
{
    /// <summary>
    ///     显示一个用户服务接口。
    /// </summary>
    public class ShowUserService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowUserService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置显示一个用户的校验器。
        /// </summary>
        public IValidator<UserShow> UserShowValidator { get; set; }

        #endregion

        #region 显示一个用户

        /// <summary>
        ///     显示一个用户。
        /// </summary>
        [CacheResponse(Duration = 600, MaxAge = 300)]
        public async Task<object> Get(UserShow request)
        {
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                UserShowValidator.ValidateAndThrow(request, ApplyTo.Get);
            }
            var authRepo = HostContext.AppHost.GetAuthRepository(Request);
            using (authRepo as IDisposable)
            {
                var existingUserAuth = await ((IUserAuthRepositoryExtended) authRepo).GetUserAuthAsync(request.UserId.ToString());
                if (existingUserAuth == null)
                {
                    throw HttpError.NotFound(string.Format(Resources.UserNotFound, request.UserId));
                }
                var userDto = MapToUserDto(existingUserAuth);
                return new UserShowResponse
                       {
                           User = userDto
                       };
            }
        }

        #endregion

        #region 转换

        public UserDto MapToUserDto(IUserAuth userAuth)
        {
            if (userAuth.Meta == null)
            {
                userAuth.Meta = new Dictionary<string, string>();
            }
            var userDto = new UserDto
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
                              BirthDate = userAuth.BirthDate?.ToString("u"),
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
                              Language = userAuth.Language,
                              PrivateMessagesSource = userAuth.Meta.GetValueOrDefault("PrivateMessagesSource"),
                              ReceiveEmails = userAuth.Meta.GetValueOrDefault("ReceiveEmails").To<bool?>(),
                              ReceiveSms = userAuth.Meta.GetValueOrDefault("ReceiveSms").To<bool?>(),
                              ReceiveCommentNotifications = userAuth.Meta.GetValueOrDefault("ReceiveCommentNotifications").To<bool?>(),
                              ReceiveConversationNotifications = userAuth.Meta.GetValueOrDefault("ReceiveConversationNotifications").To<bool?>(),
                              TrackPresence = userAuth.Meta.GetValueOrDefault("TrackPresence").To<bool?>(),
                              AccountStatus = userAuth.Meta.GetValueOrDefault("AccountStatus"),
                              BanReason = userAuth.Meta.GetValueOrDefault("BanReason"),
                              BannedUntil = userAuth.Meta.GetValueOrDefault("BannedUntil").To<DateTime?>()?.ToString("u"),
                              RequireModeration = userAuth.Meta.GetValueOrDefault("RequireModeration").To<bool?>(),
                              CreatedDate = userAuth.CreatedDate.ToString("u"),
                              ModifiedDate = userAuth.ModifiedDate.ToString("u"),
                              LockedDate = userAuth.LockedDate?.ToString("u"),
                              Points = 0
                          };
            return userDto;
        }

        #endregion
    }
}