using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Qiniu
{
    /// <summary>
    ///     生成管理凭证的请求。
    /// </summary>
    [Route("/qiniu/token/manage", HttpMethods.Get, Summary = "生成管理凭证")]
    [DataContract]
    public class ManageTokenGenerate : IReturn<ManageTokenGenerateResponse>
    {
        /// <summary>
        ///     请求的网址。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "请求的网址")]
        public string RequestUrl { get; set; }

        /// <summary>
        ///     请求的内容。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "请求的内容")]
        public Dictionary<string, string> RequestBody { get; set; }
    }

    /// <summary>
    ///     生成管理凭证的响应。
    /// </summary>
    [DataContract]
    public class ManageTokenGenerateResponse
    {
        /// <summary>
        ///     关键字。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "关键字")]
        public string Key { get; set; }

        /// <summary>
        ///     管理凭证。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "管理凭证")]
        public string ManageToken { get; set; }
    }
}