using Newtonsoft.Json;

namespace Aliyun.Green
{
    /// <summary>
    ///     文本检测任务。
    /// </summary>
    [JsonObject]
    public class TextScanRequestTask
    {
        /// <summary>
        ///     客户端信息。
        /// </summary>
        [JsonProperty(Order = 1, PropertyName = "clientInfo", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ClientInfo ClientInfo { set; get; }

        /// <summary>
        ///     调用者通常保证一次请求中，所有的dataId不重复。
        /// </summary>
        [JsonProperty(Order = 2, PropertyName = "dataId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string DataId { set; get; }

        /// <summary>
        ///     待检测文本，最长4000个字符。
        /// </summary>
        [JsonProperty(Order = 3, PropertyName = "content", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Include)]
        public string Content { set; get; }

        /// <summary>
        ///     文本创建/编辑时间，单位为ms。
        /// </summary>
        [JsonProperty(Order = 4, PropertyName = "time", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long? Time { set; get; }

        /// <summary>
        ///     内容类别，取值范围为[“post”, “reply”, “comment”, “title”, “others”]；也可以自定义的其他类型，但长度不超过64字节。
        /// </summary>
        [JsonProperty(Order = 5, PropertyName = "category", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Category { set; get; }

        /// <summary>
        ///     操作类型，取值范围为[“new”, “edit”, “share”, “others”]；也可以自定义的其他操作类型，但长度不超过64字节。
        /// </summary>
        [JsonProperty(Order = 6, PropertyName = "action", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Action { set; get; }

        /// <summary>
        ///     相关dataId；当contentType为reply或comment时，该字段设置相关的主贴或对应的comment的dataId。
        /// </summary>
        [JsonProperty(Order = 7, PropertyName = "relatedDataId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string RelatedDataId { set; get; }

        /// <summary>
        ///     相关字符串；当contentType为reply或comment时，该字段设置为主贴内容或对应的comment。
        /// </summary>
        [JsonProperty(Order = 8, PropertyName = "relatedContent", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string RelatedContent { set; get; }
    }
}