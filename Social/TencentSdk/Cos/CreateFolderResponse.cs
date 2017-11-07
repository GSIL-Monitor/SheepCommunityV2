using System.Runtime.Serialization;

namespace Tencent.Cos
{
    /// <summary>
    ///     在 COS 的 Bucket 中创建一个新文件夹的响应。
    /// </summary>
    [DataContract]
    public class CreateFolderResponse : Response
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
        public CreateFolderData Data { get; set; }
    }
}