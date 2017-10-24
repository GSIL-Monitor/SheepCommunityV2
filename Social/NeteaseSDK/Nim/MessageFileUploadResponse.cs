using System.Runtime.Serialization;

namespace Netease.Nim
{
    /// <summary>
    ///     文件上传的响应。
    /// </summary>
    [DataContract]
    public class MessageFileUploadResponse : ErrorResponse
    {
        #region 属性

        /// <summary>
        ///     文件的地址。
        /// </summary>
        [DataMember(Order = 101, Name = "url")]
        public string Url { get; set; }

        #endregion
    }
}