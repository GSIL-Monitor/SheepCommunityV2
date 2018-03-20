using System.Runtime.Serialization;

namespace Aliyun.Green
{
    /// <summary>
    ///     文本检测任务。
    /// </summary>
    [DataContract]
    public class TextScanTask
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
        ///     待检测文本，最长4000个字符。
        /// </summary>
        [DataMember(Order = 3, Name = "content", IsRequired = true)]
        public string Content { set; get; }

        /// <summary>
        ///     文本创建/编辑时间，单位为ms。
        /// </summary>
        [DataMember(Order = 4, Name = "time")]
        public long? Time { set; get; }

        /// <summary>
        ///     内容类别，取值范围为[“post”, “reply”, “comment”, “title”, “others”]；也可以自定义的其他类型，但长度不超过64字节。
        /// </summary>
        [DataMember(Order = 5, Name = "category")]
        public string Category { set; get; }

        /// <summary>
        ///     操作类型，取值范围为[“new”, “edit”, “share”, “others”]；也可以自定义的其他操作类型，但长度不超过64字节。
        /// </summary>
        [DataMember(Order = 6, Name = "action")]
        public string Action { set; get; }

        /// <summary>
        ///     相关dataId；当contentType为reply或comment时，该字段设置相关的主贴或对应的comment的dataId。
        /// </summary>
        [DataMember(Order = 7, Name = "relatedDataId")]
        public string RelatedDataId { set; get; }

        /// <summary>
        ///     相关字符串；当contentType为reply或comment时，该字段设置为主贴内容或对应的comment。
        /// </summary>
        [DataMember(Order = 8, Name = "relatedContent")]
        public string RelatedContent { set; get; }

        #endregion
    }
}