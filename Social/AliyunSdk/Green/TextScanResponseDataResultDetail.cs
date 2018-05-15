using Newtonsoft.Json;

namespace Aliyun.Green
{
    /// <summary>
    ///     检测文本的命中风险的详细信息。
    /// </summary>
    [JsonObject]
    public class TextScanResponseDataResultDetail
    {
        /// <summary>
        ///     文本命中风险的分类。
        /// </summary>
        [JsonProperty(Order = 1, PropertyName = "label", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Label { get; set; }

        /// <summary>
        ///     命中该风险的上下文信息。
        /// </summary>
        [JsonProperty(Order = 2, PropertyName = "contexts", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public TextScanResponseDataResultDetailContext[] Contexts { get; set; }
    }
}