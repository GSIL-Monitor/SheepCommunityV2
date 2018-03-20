using System.Runtime.Serialization;

namespace Aliyun.Green
{
    /// <summary>
    ///     图片检测任务。
    /// </summary>
    [DataContract]
    public class ImageScanTask
    {
        #region 属性

        /// <summary>
        ///     客户端信息。
        /// </summary>
        [DataMember(Order = 1, Name = "clientInfo")]
        public ClientInfo ClientInfo { set; get; }

        /// <summary>
        ///     调用者通常保证一次请求中，所有的dataId不重复。
        /// </summary>
        [DataMember(Order = 2, Name = "dataId")]
        public string DataId { set; get; }

        /// <summary>
        ///     待检测图像URL。
        /// </summary>
        [DataMember(Order = 3, Name = "url", IsRequired = true)]
        public string Url { set; get; }

        /// <summary>
        ///     图片创建/编辑时间，单位为ms。
        /// </summary>
        [DataMember(Order = 4, Name = "time")]
        public long? Time { set; get; }

        #endregion
    }
}