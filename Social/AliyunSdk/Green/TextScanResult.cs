using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Aliyun.Green
{
    /// <summary>
    ///     检测文本的结果。
    /// </summary>
    [DataContract]
    public class TextScanResult
    {
        #region 属性

        /// <summary>
        ///     风险场景。
        /// </summary>
        [DataMember(Order = 1, Name = "scene", IsRequired = true)]
        public string Scene { get; set; }

        /// <summary>
        ///     建议用户处理，取值范围：[“pass”, “review”, “block”], pass:文本正常，review：需要人工审核，block：文本违规，可以直接删除或者做限制处理。
        /// </summary>
        [DataMember(Order = 2, Name = "suggestion", IsRequired = true)]
        public string Suggestion { get; set; }

        /// <summary>
        ///     该文本的分类。
        /// </summary>
        [DataMember(Order = 3, Name = "label", IsRequired = true)]
        public string Label { get; set; }

        /// <summary>
        ///     结果为该分类的概率；值越高，越趋于该分类；取值为[0.00-100.00], 分值仅供参考，您只需要关注label和suggestion的取值即可。
        /// </summary>
        [DataMember(Order = 4, Name = "rate", IsRequired = true)]
        public double Rate { get; set; }

        /// <summary>
        ///     附加信息. 该值将来可能会调整，建议不要在业务上进行依赖。
        /// </summary>
        [DataMember(Order = 5, Name = "extras")]
        public Dictionary<string, object> Extras { get; set; }

        /// <summary>
        ///     命中风险的详细信息。
        /// </summary>
        [DataMember(Order = 6, Name = "details")]
        public TextScanResultDetail[] Details { get; set; }

        #endregion
    }
}