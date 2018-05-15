using Newtonsoft.Json;

namespace Aliyun.Green
{
    /// <summary>
    ///     图片检测任务。
    /// </summary>
    [JsonObject]
    public class ImageScanRequestTask
    {
        /// <summary>
        ///     客户端信息，参考[调用方式/公共请求参数/公共查询参数]小节中ClientInfo结构体描述。
        ///     服务器会把[调用方式/公共请求参数/公共查询参数]小节中全局的clientInfo和这里的独立的clientInfo合并。独立的clientInfo优先级更高。。
        /// </summary>
        [JsonProperty(Order = 1, PropertyName = "clientInfo", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ClientInfo ClientInfo { set; get; }

        /// <summary>
        ///     调用者通常保证一次请求中，所有的dataId不重复。
        /// </summary>
        [JsonProperty(Order = 2, PropertyName = "dataId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string DataId { set; get; }

        /// <summary>
        ///     待检测图像URL。
        /// </summary>
        [JsonProperty(Order = 3, PropertyName = "url", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Include)]
        public string Url { set; get; }

        /// <summary>
        ///     图片创建/编辑时间，单位为ms。
        /// </summary>
        [JsonProperty(Order = 4, PropertyName = "time", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long? Time { set; get; }
    }
}