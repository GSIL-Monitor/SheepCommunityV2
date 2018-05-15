using Newtonsoft.Json;

namespace Aliyun.Green
{
    /// <summary>
    ///     检测图片的响应。
    /// </summary>
    [JsonObject]
    public class ImageScanResponse
    {
        /// <summary>
        ///     错误码，和HTTP状态码一致（但有扩展），2xx表示成功，4xx表示请求有误，而5xx表示后端有误。
        /// </summary>
        [JsonProperty(Order = 1, PropertyName = "code", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public int Code { get; set; }

        /// <summary>
        ///     错误描述信息。
        /// </summary>
        [JsonProperty(Order = 2, PropertyName = "msg", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Message { get; set; }

        /// <summary>
        ///     唯一标识该请求的ID，可用于定位问题。
        /// </summary>
        [JsonProperty(Order = 3, PropertyName = "requestId", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string RequestId { get; set; }

        /// <summary>
        ///     相关的返回数据。出错情况下，该字段可能为空。一般来说，该字段为一个json结构体或数组。
        /// </summary>
        [JsonProperty(Order = 4, PropertyName = "data", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ImageScanResponseData[] Data { get; set; }
    }
}