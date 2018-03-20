using System.Runtime.Serialization;

namespace Aliyun.Green
{
    /// <summary>
    ///     检测图片的数据。
    /// </summary>
    [DataContract]
    public class ImageScanData
    {
        #region 属性

        /// <summary>
        ///     错误码，和http的status code一致。
        /// </summary>
        [DataMember(Order = 1, Name = "code", IsRequired = true)]
        public int Code { get; set; }

        /// <summary>
        ///     错误描述信息。
        /// </summary>
        [DataMember(Order = 2, Name = "msg", IsRequired = true)]
        public string Message { get; set; }

        /// <summary>
        ///     对应的请求中的dataId。
        /// </summary>
        [DataMember(Order = 3, Name = "dataId")]
        public string DataId { get; set; }

        /// <summary>
        ///     云盾内容安全服务器返回的唯一标识该检测任务的ID。
        /// </summary>
        [DataMember(Order = 4, Name = "taskId", IsRequired = true)]
        public string TaskId { get; set; }

        /// <summary>
        ///     对应的请求中的url。
        /// </summary>
        [DataMember(Order = 5, Name = "url", IsRequired = true)]
        public string Url { get; set; }

        /// <summary>
        ///     当成功时（code == 200）,该结果的包含一个或多个元素。每个元素是个结构体。
        /// </summary>
        [DataMember(Order = 6, Name = "results")]
        public ImageScanResult[] Results { get; set; }

        #endregion
    }
}