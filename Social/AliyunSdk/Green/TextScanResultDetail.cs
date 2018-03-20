using System.Runtime.Serialization;

namespace Aliyun.Green
{
    /// <summary>
    ///     检测文本的命中风险的详细信息。
    /// </summary>
    [DataContract]
    public class TextScanResultDetail
    {
        #region 属性

        /// <summary>
        ///     文本命中风险的分类。
        /// </summary>
        [DataMember(Order = 1, Name = "label", IsRequired = true)]
        public string Label { get; set; }

        /// <summary>
        ///     命中该风险的上下文信息。
        /// </summary>
        [DataMember(Order = 2, Name = "contexts")]
        public TextScanResultDetailContext[] Contexts { get; set; }

        #endregion
    }
}