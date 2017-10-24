using System;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Extensions;
using ServiceStack.Logging;
using AsyncContext = Nito.AsyncEx.AsyncContext;

namespace Sina.Weibo
{
    /// <summary>
    ///     新浪微博服务客户端封装库。
    /// </summary>
    public class WeiboClient : IWeiboClient
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(WeiboClient));

        #endregion

        #region 属性

        /// <summary>
        ///     申请应用时分配的应用程序唯一标识。
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        ///     申请应用时分配的应用程序密钥。
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        ///     获取接口调用凭证的地址。
        /// </summary>
        public string AccessTokenUrl { get; set; }

        /// <summary>
        ///     获取接口调用凭证的地址。
        /// </summary>
        public string GetTokenUrl { get; set; }

        /// <summary>
        ///     根据用户编号获取用户信息的地址。
        /// </summary>
        public string ShowUserUrl { get; set; }

        /// <summary>
        ///     重定向返回的地址。
        /// </summary>
        public string RedirectUrl { get; set; }

        #endregion

        #region IWeiboClient 接口实现

        /// <summary>
        ///     获取接口调用凭证。
        /// </summary>
        public AccessTokenResponse Post(AccessTokenRequest request)
        {
            return AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步获取接口调用凭证。
        /// </summary>
        public async Task<AccessTokenResponse> PostAsync(AccessTokenRequest request)
        {
            try
            {
                var responseJson = await AccessTokenUrl.HttpPostStringAsync(string.Format("client_id={0}&client_secret={1}&grant_type=authorization_code&code={2}&redirect_uri={3}", AppKey, AppSecret, request.Code, RedirectUrl), "application/x-www-form-urlencoded");
                var response = responseJson.FromJson<AccessTokenResponse>();
                if (response != null && response.ErrorCode != 0)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}-{4}", GetType().Name, request.GetType().Name, response.Error, response.ErrorCode, response.ErrorDescription);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new AccessTokenResponse
                       {
                           Error = ex.GetType().Name,
                           ErrorCode = -1,
                           ErrorDescription = errorMessage
                       };
            }
        }

        /// <summary>
        ///     获取接口调用凭证。
        /// </summary>
        public GetTokenResponse Post(GetTokenRequest request)
        {
            return AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步获取接口调用凭证。
        /// </summary>
        public async Task<GetTokenResponse> PostAsync(GetTokenRequest request)
        {
            try
            {
                var responseJson = await GetTokenUrl.HttpPostStringAsync(string.Format("access_token={0}", request.AccessToken), "application/x-www-form-urlencoded");
                var response = responseJson.FromJson<GetTokenResponse>();
                if (response != null && response.ErrorCode != 0)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}-{4}", GetType().Name, request.GetType().Name, response.Error, response.ErrorCode, response.ErrorDescription);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new GetTokenResponse
                       {
                           Error = ex.GetType().Name,
                           ErrorCode = -1,
                           ErrorDescription = errorMessage
                       };
            }
        }

        /// <summary>
        ///     根据用户编号获取用户信息。
        /// </summary>
        public ShowUserResponse Get(ShowUserRequest request)
        {
            return AsyncContext.Run(() => GetAsync(request));
        }

        /// <summary>
        ///     异步根据用户编号获取用户信息。
        /// </summary>
        public async Task<ShowUserResponse> GetAsync(ShowUserRequest request)
        {
            try
            {
                var responseJson = await "{0}?access_token={1}&uid={2}".Fmt(ShowUserUrl, request.AccessToken, request.UserId).HttpGetAsync();
                var response = responseJson.FromJson<ShowUserResponse>();
                if (response != null && response.ErrorCode != 0)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}-{4}", GetType().Name, request.GetType().Name, response.Error, response.ErrorCode, response.ErrorDescription);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new ShowUserResponse
                       {
                           Error = ex.GetType().Name,
                           ErrorCode = -1,
                           ErrorDescription = errorMessage
                       };
            }
        }

        #endregion
    }
}