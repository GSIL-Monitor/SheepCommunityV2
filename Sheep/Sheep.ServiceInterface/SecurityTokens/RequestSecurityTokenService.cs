using System.Net;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Model.Security;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.SecurityTokens;

namespace Sheep.ServiceInterface.SecurityTokens
{
    /// <summary>
    ///     请求发送验证码服务接口。
    /// </summary>
    public class RequestSecurityTokenService : Service
    {
        #region 常量

        public const string SignatureName = "token.mobile.Signature";
        public const string LoginTemplateName = "token.mobile.LoginTemplate";
        public const string RegisterTemplateName = "token.mobile.RegisterTemplate";
        public const string BindTemplateName = "token.mobile.BindTemplate";
        public const string ResetPasswordTemplateName = "token.mobile.ResetPasswordTemplate";

        #endregion

        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RequestSecurityTokenService));

        /// <summary>
        ///     自定义校验函数。
        /// </summary>
        public static ValidateFn ValidateFn { get; set; }

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置请求发送验证码的校验器。
        /// </summary>
        public IValidator<SecurityTokenRequest> SecurityTokenRequestValidator { get; set; }

        /// <summary>
        ///     安全验证码提供程序。
        /// </summary>
        public ISecurityTokenProvider SecurityTokenProvider { get; set; }

        #endregion

        #region 请求发送验证码

        /// <summary>
        ///     请求发送验证码。
        /// </summary>
        public async Task<object> Post(SecurityTokenRequest request)
        {
            var validateResponse = ValidateFn?.Invoke(this, HttpMethods.Post, request);
            if (validateResponse != null)
            {
                return validateResponse;
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                SecurityTokenRequestValidator.ValidateAndThrow(request, ApplyTo.Post);
            }
            var securityToken = await SecurityTokenProvider.GenerateTokenAsync(request.PhoneNumber, request.Purpose);
            if (securityToken.IsNullOrEmpty())
            {
                throw new HttpError(HttpStatusCode.InternalServerError, "GenerateTokenError", Resources.GenerateTokenError);
            }
            var templateName = string.Empty;
            switch (request.Purpose)
            {
                case "Login":
                    templateName = LoginTemplateName;
                    break;
                case "Register":
                    templateName = RegisterTemplateName;
                    break;
                case "Bind":
                    templateName = BindTemplateName;
                    break;
                case "ResetPassword":
                    templateName = ResetPasswordTemplateName;
                    break;
            }
            var sendSuccess = await SecurityTokenProvider.SendTokenAsync(request.PhoneNumber, AppSettings.GetString(SignatureName), securityToken, AppSettings.GetString(templateName));
            if (!sendSuccess)
            {
                throw new HttpError(HttpStatusCode.InternalServerError, "SendTokenError", Resources.SendTokenError);
            }
            return new SecurityTokenRequestResponse();
        }

        #endregion
    }
}