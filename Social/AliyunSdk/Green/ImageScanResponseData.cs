using Newtonsoft.Json;

namespace Aliyun.Green
{
    /// <summary>
    ///     检测图片的数据。
    /// </summary>
    [JsonObject]
    public class ImageScanResponseData
    {
        /// <summary>
        ///     错误码，和http的status code一致。
        /// </summary>
        [JsonProperty(Order = 1, PropertyName = "code", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public int Code { get; set; }

        /// <summary>
        ///     错误描述信息。
        /// </summary>
        [JsonProperty(Order = 2, PropertyName = "msg", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Message { get; set; }

        /// <summary>
        ///     对应的请求中的dataId。
        /// </summary>
        [JsonProperty(Order = 3, PropertyName = "dataId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string DataId { get; set; }

        /// <summary>
        ///     云盾内容安全服务器返回的唯一标识该检测任务的ID。
        /// </summary>
        [JsonProperty(Order = 4, PropertyName = "taskId", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string TaskId { get; set; }

        /// <summary>
        ///     对应的请求中的url。
        /// </summary>
        [JsonProperty(Order = 5, PropertyName = "url", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Url { get; set; }

        /// <summary>
        ///     当成功时（code == 200）,该结果的包含一个或多个元素。每个元素是个结构体。
        /// </summary>
        [JsonProperty(Order = 6, PropertyName = "results", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ImageScanResponseDataResult[] Results { get; set; }
    }
}