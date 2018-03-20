using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Aliyun.Green
{
    /// <summary>
    ///     检测图片的结果。
    /// </summary>
    [DataContract]
    public class ImageScanResult
    {
        #region 属性

        /// <summary>
        ///     风险场景，和传递进来的场景对应。
        /// </summary>
        [DataMember(Order = 1, Name = "scene", IsRequired = true)]
        public string Scene { get; set; }

        /// <summary>
        ///     建议用户处理，取值范围：[“pass”, “review”, “block”], pass:图片正常，review：需要人工审核，block：图片违规，可以直接删除或者做限制处理。
        /// </summary>
        [DataMember(Order = 2, Name = "suggestion", IsRequired = true)]
        public string Suggestion { get; set; }

        /// <summary>
        ///     该文本的分类。
        /// </summary>
        [DataMember(Order = 3, Name = "label", IsRequired = true)]
        public string Label { get; set; }

        /// <summary>
        ///     结果为该分类的概率；值越高，越趋于该分类；取值为[0.00-100.00]。
        /// </summary>
        [DataMember(Order = 4, Name = "rate", IsRequired = true)]
        public double Rate { get; set; }

        /// <summary>
        ///     附加信息. 该值将来可能会调整，建议不要在业务上进行依赖。
        /// </summary>
        [DataMember(Order = 5, Name = "extras")]
        public Dictionary<string, object> Extras { get; set; }

        #endregion
    }
}