﻿using System.Net;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Model.Security;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.SecurityTokens;

namespace Sheep.ServiceInterface.SecurityTokens
{
    /// <summary>
    ///     校验验证码服务接口。
    /// </summary>
    public class VerifySecurityTokenService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(VerifySecurityTokenService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置校验验证码的校验器。
        /// </summary>
        public IValidator<SecurityTokenVerify> SecurityTokenVerifyValidator { get; set; }

        /// <summary>
        ///     获取及设置安全验证码提供程序。
        /// </summary>
        public ISecurityTokenProvider SecurityTokenProvider { get; set; }

        #endregion

        #region 校验验证码

        /// <summary>
        ///     校验验证码。
        /// </summary>
        public async Task<object> Post(SecurityTokenVerify request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    SecurityTokenVerifyValidator.ValidateAndThrow(request, ApplyTo.Post);
            //}
            var verifySuccess = await SecurityTokenProvider.VerifyTokenAsync(request.PhoneNumber, request.Purpose, request.Token);
            if (!verifySuccess)
            {
                throw new HttpError(HttpStatusCode.BadRequest, "InvalidSecurityToken", Resources.InvalidSecurityToken);
            }
            return new SecurityTokenVerifyResponse();
        }

        #endregion
    }
}