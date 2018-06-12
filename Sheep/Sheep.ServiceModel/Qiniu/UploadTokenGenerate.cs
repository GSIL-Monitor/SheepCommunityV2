using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Qiniu
{
    /// <summary>
    ///     生成上传凭证的请求。
    /// </summary>
    [Route("/qiniu/token", HttpMethods.Get, Summary = "生成上传凭证")]
    [DataContract]
    public class UploadTokenGenerate : IReturn<UploadTokenGenerateResponse>
    {
        /// <summary>
        ///     要覆盖的关键字。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "要覆盖的关键字")]
        public string KeyToOverwrite { get; set; }
    }

    /// <summary>
    ///     生成上传凭证的响应。
    /// </summary>
    [DataContract]
    public class UploadTokenGenerateResponse
    {
        /// <summary>
        ///     上传凭证。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "上传凭证")]
        public string UploadToken { get; set; }
    }
}