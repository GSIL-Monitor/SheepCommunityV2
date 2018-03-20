using System.Runtime.Serialization;

namespace Aliyun.Green
{
    /// <summary>
    ///     检测文本的请求，以 JSON 格式表达。
    /// </summary>
    [DataContract]
    public class TextScanRequest
    {
        #region 属性

        /// <summary>
        ///     业务类型，由调用方提供。根据配置，后端可根据该字段对请求做不同处理。属于高级用法。
        /// </summary>
        [DataMember(Order = 1, Name = "bizType")]
        public string BizType { get; set; }

        /// <summary>
        ///     字符串数组，场景定义参考1.1小节；反垃圾检测，scenes请填写antispam；关键词检测，scenes请填写keyword。
        /// </summary>
        [DataMember(Order = 2, Name = "scenes", IsRequired = true)]
        public string[] Scenes { get; set; }

        /// <summary>
        ///     文本检测任务列表；每个元素是个结构体，最多支持100个，即100段文本的检测。
        /// </summary>
        [DataMember(Order = 3, Name = "tasks", IsRequired = true)]
        public TextScanTask[] Tasks { get; set; }

        #endregion
    }
}