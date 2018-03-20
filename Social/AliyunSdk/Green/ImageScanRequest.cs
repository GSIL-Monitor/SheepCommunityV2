using System.Runtime.Serialization;

namespace Aliyun.Green
{
    /// <summary>
    ///     检测图片的请求，以 JSON 格式表达。
    /// </summary>
    [DataContract]
    public class ImageScanRequest
    {
        #region 属性

        /// <summary>
        ///     业务类型，由调用方提供。根据配置，后端可根据该字段对请求做不同处理。属于高级用法。
        /// </summary>
        [DataMember(Order = 1, Name = "bizType")]
        public string BizType { get; set; }

        /// <summary>
        ///     字符串数组，场景定义参考1.1小节, 图片鉴黄的scene取值为:porn.图片检测支持多场景（scenes）一起检测， 比如对一张图片进行黄图和暴恐的同时识别，scenes为[“porn”, “terrorism”],
        ///     其他更多图片检测场景同时检测类似添加。
        /// </summary>
        [DataMember(Order = 2, Name = "scenes", IsRequired = true)]
        public string[] Scenes { get; set; }

        /// <summary>
        ///     JSON数组中的每个元素是一个图片检测任务结构体，最多支持100个，即100张图片的检测。
        /// </summary>
        [DataMember(Order = 3, Name = "tasks", IsRequired = true)]
        public ImageScanTask[] Tasks { get; set; }

        #endregion
    }
}