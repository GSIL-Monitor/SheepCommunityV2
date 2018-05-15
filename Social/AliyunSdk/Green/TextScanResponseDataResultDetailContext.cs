using Newtonsoft.Json;

namespace Aliyun.Green
{
    /// <summary>
    ///     检测文本的命中风险的上下文信息。
    /// </summary>
    [JsonObject]
    public class TextScanResponseDataResultDetailContext
    {
        /// <summary>
        ///     命中风险的内容。
        /// </summary>
        [JsonProperty(Order = 1, PropertyName = "context", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Context { get; set; }

        /// <summary>
        ///     命中自定义词库，才有本字段。值为创建词库时填写的词库名称。
        /// </summary>
        [JsonProperty(Order = 2, PropertyName = "libName", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string LibName { get; set; }

        /// <summary>
        ///     命中行为规则，才有该字段。可能取值user_id,ip,umid,content。
        /// </summary>
        [JsonProperty(Order = 3, PropertyName = "ruleType", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string RuleType { get; set; }
    }
}