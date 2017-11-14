using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Files
{
    /// <summary>
    ///     上传图像的请求。
    /// </summary>
    [Route("/files/image")]
    [DataContract]
    public class FileUploadImage : IReturn<FileUploadImageResponse>
    {
    }

    /// <summary>
    ///     上传图像的响应。
    /// </summary>
    [DataContract]
    public class FileUploadImageResponse
    {
        /// <summary>
        ///     响应的状态。
        /// </summary>
        [DataMember(Order = 1, Name = "status")]
        public int Status { get; set; }

        /// <summary>
        ///     图像的地址。
        /// </summary>
        [DataMember(Order = 2, Name = "url")]
        public string Url { get; set; }

        /// <summary>
        ///     响应的消息。
        /// </summary>
        [DataMember(Order = 3, Name = "msg")]
        public string Message { get; set; }
    }
}