using System.Net;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.ServiceModel.Accounts;
using Sina.Weibo;

namespace Sheep.ServiceInterface.Accounts
{
    /// <summary>
    ///     获取微博授权码服务接口。
    /// </summary>
    public class GetAccountAccessTokenForWeiboService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(GetAccountAccessTokenForWeiboService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置获取微博授权码的校验器。
        /// </summary>
        public IValidator<AccountGetAccessTokenForWeibo> AccountAccessTokenForWeiboValidator { get; set; }

        /// <summary>
        ///     获取及设置新浪微博服务客户端封装库。
        /// </summary>
        public IWeiboClient WeiboClient { get; set; }

        #endregion

        #region 获取微博授权码

        /// <summary>
        ///     获取微博授权码。
        /// </summary>
        public async Task<object> Get(AccountGetAccessTokenForWeibo request)
        {
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                AccountAccessTokenForWeiboValidator.ValidateAndThrow(request, ApplyTo.Get);
            }
            var accessTokenResponse = await WeiboClient.PostAsync(new AccessTokenRequest
                                                                  {
                                                                      Code = request.Code
                                                                  });
            if (accessTokenResponse.ErrorCode != 0)
            {
                throw new HttpError(HttpStatusCode.BadRequest, accessTokenResponse.Error, accessTokenResponse.ErrorDescription);
            }
            return new AccountGetAccessTokenResponse
                   {
                       AccessToken = accessTokenResponse.AccessToken,
                       UserId = accessTokenResponse.UserId
                   };
        }

        #endregion
    }
}