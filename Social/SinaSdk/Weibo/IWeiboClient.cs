using System.Threading.Tasks;

namespace Sina.Weibo
{
    /// <summary>
    ///     新浪微博服务客户端封装库的接口定义。
    /// </summary>
    public interface IWeiboClient
    {
        #region 授权

        /// <summary>
        ///     获取接口调用凭证。
        /// </summary>
        AccessTokenResponse Post(AccessTokenRequest request);

        /// <summary>
        ///     异步获取接口调用凭证。
        /// </summary>
        Task<AccessTokenResponse> PostAsync(AccessTokenRequest request);

        #endregion

        #region 用户身份

        /// <summary>
        ///     获取接口调用凭证。
        /// </summary>
        GetTokenResponse Post(GetTokenRequest request);

        /// <summary>
        ///     异步获取接口调用凭证。
        /// </summary>
        Task<GetTokenResponse> PostAsync(GetTokenRequest request);

        /// <summary>
        ///     根据用户编号获取用户信息。
        /// </summary>
        ShowUserResponse Get(ShowUserRequest request);

        /// <summary>
        ///     异步根据用户编号获取用户信息。
        /// </summary>
        Task<ShowUserResponse> GetAsync(ShowUserRequest request);

        #endregion
    }
}