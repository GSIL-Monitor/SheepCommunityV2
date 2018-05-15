using System;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Extensions;
using ServiceStack.Logging;

namespace Tencent.Weixin
{
    /// <summary>
    ///     腾讯微信服务客户端封装库。
    /// </summary>
    public class WeixinClient : IWeixinClient
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(WeixinClient));

        #endregion

        #region 属性

        /// <summary>
        ///     申请应用时分配的应用程序唯一标识。
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        ///     申请应用时分配的应用程序密钥。
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        ///     获取接口调用凭证的地址。
        /// </summary>
        public string AccessTokenUrl { get; set; }

        /// <summary>
        ///     刷新或续期接口调用凭证使用的地址。
        /// </summary>
        public string RefreshTokenUrl { get; set; }

        /// <summary>
        ///     检验接口调用凭证是否有效的地址。
        /// </summary>
        public string AuthenticateTokenUrl { get; set; }

        /// <summary>
        ///     获取用户个人信息的地址。
        /// </summary>
        public string UserInfoUrl { get; set; }

        #endregion

        #region 构造器

        #endregion

        #region IWeixinClient 接口实现

        /// <summary>
        ///     获取接口调用凭证。
        /// </summary>
        public AccessTokenResponse Get(AccessTokenRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => GetAsync(request));
        }

        /// <summary>
        ///     异步获取接口调用凭证。
        /// </summary>
        public async Task<AccessTokenResponse> GetAsync(AccessTokenRequest request)
        {
            try
            {
                var responseJson = await "{0}?appid={1}&secret={2}&code={3}&grant_type=authorization_code".Fmt(AccessTokenUrl, AppId, AppSecret, request.Code).HttpGetAsync();
                var response = responseJson.FromJson<AccessTokenResponse>();
                if (response != null && response.ErrorCode != 0)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}", GetType().Name, request.GetType().Name, response.ErrorCode, response.ErrorMessage);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new AccessTokenResponse
                       {
                           ErrorCode = -1,
                           ErrorMessage = errorMessage
                       };
            }
        }

        /// <summary>
        ///     刷新或续期接口调用凭证使用。
        /// </summary>
        public RefreshTokenResponse Get(RefreshTokenRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => GetAsync(request));
        }

        /// <summary>
        ///     异步刷新或续期接口调用凭证使用。
        /// </summary>
        public async Task<RefreshTokenResponse> GetAsync(RefreshTokenRequest request)
        {
            try
            {
                var responseJson = await "{0}?appid={1}&grant_type=refresh_token&refresh_token={2}".Fmt(RefreshTokenUrl, AppId, request.RefreshToken).HttpGetAsync();
                var response = responseJson.FromJson<RefreshTokenResponse>();
                if (response != null && response.ErrorCode != 0)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}", GetType().Name, request.GetType().Name, response.ErrorCode, response.ErrorMessage);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new RefreshTokenResponse
                       {
                           ErrorCode = -1,
                           ErrorMessage = errorMessage
                       };
            }
        }

        /// <summary>
        ///     检验接口调用凭证是否有效。
        /// </summary>
        public AuthenticateTokenResponse Get(AuthenticateTokenRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => GetAsync(request));
        }

        /// <summary>
        ///     异步检验接口调用凭证是否有效。
        /// </summary>
        public async Task<AuthenticateTokenResponse> GetAsync(AuthenticateTokenRequest request)
        {
            try
            {
                var responseJson = await "{0}?access_token={1}&openid={2}".Fmt(AuthenticateTokenUrl, request.AccessToken, request.OpenId).HttpGetAsync();
                var response = responseJson.FromJson<AuthenticateTokenResponse>();
                if (response != null && response.ErrorCode != 0)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}", GetType().Name, request.GetType().Name, response.ErrorCode, response.ErrorMessage);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new AuthenticateTokenResponse
                       {
                           ErrorCode = -1,
                           ErrorMessage = errorMessage
                       };
            }
        }

        /// <summary>
        ///     获取用户个人信息。
        /// </summary>
        public UserInfoResponse Get(UserInfoRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => GetAsync(request));
        }

        /// <summary>
        ///     异步获取用户个人信息。
        /// </summary>
        public async Task<UserInfoResponse> GetAsync(UserInfoRequest request)
        {
            try
            {
                var responseJson = await "{0}?access_token={1}&openid={2}&lang={3}".Fmt(UserInfoUrl, request.AccessToken, request.OpenId, request.Language).HttpGetAsync();
                var response = responseJson.FromJson<UserInfoResponse>();
                if (response != null && response.ErrorCode != 0)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}", GetType().Name, request.GetType().Name, response.ErrorCode, response.ErrorMessage);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new UserInfoResponse
                       {
                           ErrorCode = -1,
                           ErrorMessage = errorMessage
                       };
            }
        }

        #endregion
    }
}