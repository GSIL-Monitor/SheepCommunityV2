﻿using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Common.Auth;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceInterface.Users.Mappers;
using Sheep.ServiceModel.Users;

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

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        #endregion

        #region 根据用户名称或电子邮件地址显示一个用户

        /// <summary>
        ///     根据用户名称或电子邮件地址显示一个用户。
        /// </summary>
        [CacheResponse(Duration = 3600)]
        public async Task<object> Get(UserShowByUserNameOrEmail request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    UserShowByUserNameOrEmailValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var existingUserAuth = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthByUserNameAsync(request.UserNameOrEmail);
            if (existingUserAuth == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, request.UserNameOrEmail));
            }
            var userDto = existingUserAuth.MapToUserDto();
            return new UserShowResponse
                   {
                       User = userDto
                   };
        }

        #endregion
    }
}