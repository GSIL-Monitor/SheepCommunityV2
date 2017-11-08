using System.Runtime.Serialization;

namespace Tencent.Cos
{
    /// <summary>
    ///     上传简单文件的响应。
    /// </summary>
    [DataContract]
    public class UploadFileResponse : Response
    {
        /// <summary>
        ///     请求的编号 。
        /// </summary>
        [DataMember(Order = 101, Name = "request_id")]
        public string RequestId { get; set; }

        /// <summary>
        ///     服务端返回的应答数据。
        /// </summary>
        [DataMember(Order = 102, Name = "data")]
        public UploadFileData Data { get; set; }
    }
}