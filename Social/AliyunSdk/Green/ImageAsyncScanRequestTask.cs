using Newtonsoft.Json;

namespace Aliyun.Green
{
    /// <summary>
    ///     异步图片检测任务。
    /// </summary>
    [JsonObject]
    public class ImageAsyncScanRequestTask
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

        /// <summary>
        ///     GIF图/长图检测专用。
        ///     截帧频率，GIF图可理解为图片数组，每interval张图片抽取一张进行检测。只有该值存在时，才会对GIF进行截帧。
        ///     长图同时支持长竖图和长横图。对长竖图，按照9:16（宽:高)来计算总图数，并进行切割。长横图会按照16:9（宽:高)来计算总图数，并进行切割。
        ///     这里的interval指示后台检测时可按照该间隔跳着检测，以节省检测成本。
        /// </summary>
        [JsonProperty(Order = 5, PropertyName = "interval", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Interval { set; get; }

        /// <summary>
        ///     GIF图/长图检测专用。
        ///     最大截帧数量。当interval*maxFrames小于该图片所包含的图片数量时，截帧间隔会自动修改为“该图片所包含的图片数/maxFrames”,以提高整体检测效果。默认值为100。
        /// </summary>
        [JsonProperty(Order = 6, PropertyName = "maxFrames", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? MaxFrames { set; get; }
    }
}