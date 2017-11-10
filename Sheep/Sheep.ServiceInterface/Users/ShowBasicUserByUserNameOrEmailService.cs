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
    ///     根据用户名称或电子邮件地址显示一个用户基本信息服务接口。
    /// </summary>
    public class ShowBasicUserByUserNameOrEmailService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowBasicUserByUserNameOrEmailService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置根据用户名称或电子邮件地址显示一个用户基本信息的校验器。
        /// </summary>
        public IValidator<BasicUserShowByUserNameOrEmail> BasicUserShowByUserNameOrEmailValidator { get; set; }

        #endregion

        #region 根据用户名称或电子邮件地址显示一个用户基本信息

        /// <summary>
        ///     根据用户名称或电子邮件地址显示一个用户基本信息。
        /// </summary>
        [CacheResponse(Duration = 7200, MaxAge = 3600)]
        public object Get(BasicUserShowByUserNameOrEmail request)
        {
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                BasicUserShowByUserNameOrEmailValidator.ValidateAndThrow(request, ApplyTo.Get);
            }
            var authRepo = HostContext.AppHost.GetAuthRepository(Request);
            using (authRepo as IDisposable)
            {
                var existingUserAuth = authRepo.GetUserAuthByUserName(request.UserNameOrEmail);
                if (existingUserAuth == null)
                {
                    throw HttpError.NotFound(string.Format(Resources.UserNotFound, request.UserNameOrEmail));
                }
                var userDto = MapToBasicUserDto(existingUserAuth);
                return new BasicUserShowResponse
                       {
                           User = userDto
                       };
            }
        }

        #endregion

        #region 转换

        public BasicUserDto MapToBasicUserDto(IUserAuth userAuth)
        {
            if (userAuth.Meta == null)
            {
                userAuth.Meta = new Dictionary<string, string>();
            }
            var userDto = new BasicUserDto
                       {
                           Id = userAuth.Id,
                           UserName = userAuth.UserName,
                           DisplayName = userAuth.DisplayName,
                           AvatarUrl = userAuth.Meta.GetValueOrDefault("AvatarUrl"),
                           Gender = userAuth.Gender
                       };
            return userDto;
        }

        #endregion
    }
}