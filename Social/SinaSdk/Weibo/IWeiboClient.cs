﻿using System.Threading.Tasks;

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
        ///     获取国家列表。
        /// </summary>
        ShowUserResponse Get(ShowUserRequest request);

        /// <summary>
        ///     异步获取国家列表。
        /// </summary>
        Task<ShowUserResponse> GetAsync(ShowUserRequest request);

        #endregion

        #region 地址服务

        /// <summary>
        ///     获取国家列表。
        /// </summary>
        GetCountryResponse Get(GetCountryRequest request);

        /// <summary>
        ///     异步获取国家列表。
        /// </summary>
        Task<GetCountryResponse> GetAsync(GetCountryRequest request);

        /// <summary>
        ///     获取省份列表。
        /// </summary>
        GetProvinceResponse Get(GetProvinceRequest request);

        /// <summary>
        ///     异步获取省份列表。
        /// </summary>
        Task<GetProvinceResponse> GetAsync(GetProvinceRequest request);

        /// <summary>
        ///     获取城市列表。
        /// </summary>
        GetCityResponse Get(GetCityRequest request);

        /// <summary>
        ///     异步获取城市列表。
        /// </summary>
        Task<GetCityResponse> GetAsync(GetCityRequest request);

        #endregion
    }
}