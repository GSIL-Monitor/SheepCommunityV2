﻿using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Accounts
{
    /// <summary>
    ///     更改帐户显示语言的请求。
    /// </summary>
    [Route("/account/language", HttpMethods.Put, Summary = "更改帐户显示语言")]
    [DataContract]
    public class AccountChangeLanguage : IReturn<AccountChangeLanguageResponse>
    {
        /// <summary>
        ///     更改的显示语言。（可选的值： 简体中文, 繁体中文 英语, 法语, 西班牙语, 日语, 韩语）
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "更改的显示语言（可选的值： 简体中文, 繁体中文 英语, 法语, 西班牙语, 日语, 韩语）")]
        public string Language { get; set; }
    }

    /// <summary>
    ///     更改帐户显示语言的响应。
    /// </summary>
    [DataContract]
    public class AccountChangeLanguageResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}