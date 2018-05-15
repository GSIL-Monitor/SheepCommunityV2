using System.Collections.Generic;
using Newtonsoft.Json;

namespace Aliyun.Green
{
    /// <summary>
    ///     检测文本的结果。
    /// </summary>
    [JsonObject]
    public class TextScanResponseDataResult
    {
        /// <summary>
        ///     风险场景。
        /// </summary>
        [JsonProperty(Order = 1, PropertyName = "scene", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Scene { get; set; }

        /// <summary>
        ///     建议用户处理，取值范围：[“pass”, “review”, “block”], pass:文本正常，review：需要人工审核，block：文本违规，可以直接删除或者做限制处理。
        /// </summary>
        [JsonProperty(Order = 2, PropertyName = "suggestion", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Suggestion { get; set; }

        /// <summary>
        ///     该文本的分类。
        /// </summary>
        [JsonProperty(Order = 3, PropertyName = "label", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Label { get; set; }

        /// <summary>
        ///     结果为该分类的概率；值越高，越趋于该分类；取值为[0.00-100.00], 分值仅供参考，您只需要关注label和suggestion的取值即可。
        /// </summary>
        [JsonProperty(Order = 4, PropertyName = "rate", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public float Rate { get; set; }

        /// <summary>
        ///     附加信息，比如命中了您自定义的词库,返回词库code.该值将来可能会调整，建议不要在业务上进行依赖。
        /// </summary>
        [JsonProperty(Order = 5, PropertyName = "extras", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Dictionary<string, object> Extras { get; set; }

        /// <summary>
        ///     命中风险的详细信息。
        /// </summary>
        [JsonProperty(Order = 6, PropertyName = "details", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public TextScanResponseDataResultDetail[] Details { get; set; }
    }
}