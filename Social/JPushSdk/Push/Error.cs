using System.Runtime.Serialization;

namespace JPush.Push
{
    /// <summary>
    ///     错误信息响应。
    /// </summary>
    [DataContract]
    public class Error
    {
        #region 属性

        /// <summary>
        ///     错误内部编号，0为成功，其他为失败。
        /// </summary>
        [DataMember(Order = 1, Name = "code")]
        public int Code { get; set; }

        /// <summary>
        ///     错误的描述信息 。
        /// </summary>
        [DataMember(Order = 2, Name = "message")]
        public string Message { get; set; }

        #endregion
    }
}