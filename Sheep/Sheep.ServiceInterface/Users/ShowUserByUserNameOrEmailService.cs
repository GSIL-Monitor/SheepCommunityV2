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
    ///     根据用户名称或电子邮件地址显示一个用户服务接口。
    /// </summary>
    public class ShowUserByUserNameOrEmailService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowUserByUserNameOrEmailService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置根据用户名称或电子邮件地址显示一个用户的校验器。
        /// </summary>
        public IValidator<UserShowByUserNameOrEmail> UserShowByUserNameOrEmailValidator { get; set; }

        #endregion

        #region 根据用户名称或电子邮件地址显示一个用户

        /// <summary>
        ///     根据用户名称或电子邮件地址显示一个用户。
        /// </summary>
        [CacheResponse(Duration = 600, MaxAge = 300)]
        public async Task<object> Get(UserShowByUserNameOrEmail request)
        {
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                UserShowByUserNameOrEmailValidator.ValidateAndThrow(request, ApplyTo.Get);
            }
            var authRepo = HostContext.AppHost.GetAuthRepository(Request);
            using (authRepo as IDisposable)
            {
                var existingUserAuth = await ((IUserAuthRepositoryExtended) authRepo).GetUserAuthByUserNameAsync(request.UserNameOrEmail);
                if (existingUserAuth == null)
                {
                    throw HttpError.NotFound(string.Format(Resources.UserNotFound, request.UserNameOrEmail));
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
                              Country = userAuth.Country,
                              State = userAuth.State,
                              City = userAuth.City,
                              Guild = userAuth.Meta.GetValueOrDefault("Guild"),
                              AccountStatus = userAuth.Meta.GetValueOrDefault("AccountStatus"),
                              BanReason = userAuth.Meta.GetValueOrDefault("BanReason"),
                              BannedUntil = userAuth.Meta.GetValueOrDefault("BannedUntil").To<DateTime?>()?.ToString("u"),
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