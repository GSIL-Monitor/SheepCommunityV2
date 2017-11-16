using System.Runtime.Serialization;

namespace Netease.Nim
{
    /// <summary>
    ///     错误信息响应。
    /// </summary>
    [DataContract]
    public class ErrorResponse
    {
        #region 属性

        /// <summary>
        ///     错误内部编号，200为成功，其他为失败。
        /// </summary>
        [DataMember(Order = 1, Name = "code")]
        public int Code { get; set; }

        /// <summary>
        ///     错误描述。
        /// </summary>
        [DataMember(Order = 1, Name = "desc")]
        public int Description { get; set; }

        #endregion
    }
}