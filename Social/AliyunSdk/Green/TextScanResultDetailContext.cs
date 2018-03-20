using System.Runtime.Serialization;

namespace Aliyun.Green
{
    /// <summary>
    ///     检测文本的命中风险的上下文信息。
    /// </summary>
    [DataContract]
    public class TextScanResultDetailContext
    {
        #region 属性

        /// <summary>
        ///     命中风险的内容。
        /// </summary>
        [DataMember(Order = 1, Name = "context", IsRequired = true)]
        public string Context { get; set; }

        /// <summary>
        ///     命中自定义词库，才有本字段。值为创建词库时填写的词库名称。
        /// </summary>
        [DataMember(Order = 2, Name = "libName")]
        public string LibName { get; set; }

        /// <summary>
        ///     命中行为规则，才有该字段。可能取值user_id,ip,umid,content。
        /// </summary>
        [DataMember(Order = 3, Name = "ruleType")]
        public string RuleType { get; set; }

        #endregion
    }
}