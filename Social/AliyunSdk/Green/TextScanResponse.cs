using System.Runtime.Serialization;

namespace Aliyun.Green
{
    /// <summary>
    ///     检测文本的响应。
    /// </summary>
    [DataContract]
    public class TextScanResponse
    {
        #region 属性

        /// <summary>
        ///     错误码，和HTTP状态码一致（但有扩展），2xx表示成功，4xx表示请求有误，而5xx表示后端有误。
        /// </summary>
        [DataMember(Order = 1, Name = "code", IsRequired = true)]
        public int Code { get; set; }

        /// <summary>
        ///     错误描述信息。
        /// </summary>
        [DataMember(Order = 2, Name = "msg", IsRequired = true)]
        public string Message { get; set; }

        /// <summary>
        ///     唯一标识该请求的ID，可用于定位问题。
        /// </summary>
        [DataMember(Order = 3, Name = "requestId", IsRequired = true)]
        public string RequestId { get; set; }

        /// <summary>
        ///     相关的返回数据。出错情况下，该字段可能为空。一般来说，该字段为一个json结构体或数组。
        /// </summary>
        [DataMember(Order = 4, Name = "data")]
        public TextScanData Data { get; set; }

        #endregion
    }
}