using System.Threading.Tasks;

namespace Tencent.Weixin
{
    /// <summary>
    ///     腾讯微信服务客户端封装库的接口定义。
    /// </summary>
    public interface IWeixinClient
    {
        #region 授权

        /// <summary>
        ///     获取接口调用凭证。
        /// </summary>
        AccessTokenResponse Get(AccessTokenRequest request);

        /// <summary>
        ///     异步获取接口调用凭证。
        /// </summary>
        Task<AccessTokenResponse> GetAsync(AccessTokenRequest request);

        /// <summary>
        ///     刷新或续期接口调用凭证使用。
        /// </summary>
        RefreshTokenResponse Get(RefreshTokenRequest request);

        /// <summary>
        ///     异步刷新或续期接口调用凭证使用。
        /// </summary>
        Task<RefreshTokenResponse> GetAsync(RefreshTokenRequest request);

        #endregion

        #region 用户身份

        /// <summary>
        ///     检验接口调用凭证是否有效。
        /// </summary>
        AuthenticateTokenResponse Get(AuthenticateTokenRequest request);

        /// <summary>
        ///     异步检验接口调用凭证是否有效。
        /// </summary>
        Task<AuthenticateTokenResponse> GetAsync(AuthenticateTokenRequest request);

        /// <summary>
        ///     获取用户个人信息。
        /// </summary>
        UserInfoResponse Get(UserInfoRequest request);

        /// <summary>
        ///     异步获取用户个人信息。
        /// </summary>
        Task<UserInfoResponse> GetAsync(UserInfoRequest request);

        #endregion
    }
}