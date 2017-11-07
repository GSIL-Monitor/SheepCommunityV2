using System.Runtime.Serialization;

namespace Tencent.Cos
{
    /// <summary>
    ///     服务端响应。
    /// </summary>
    [DataContract]
    public class Response
    {
        #region 属性

        /// <summary>
        ///     服务端返回码，0为成功，其他为失败。
        /// </summary>
        [DataMember(Order = 1, Name = "code")]
        public int Code { get; set; }

        /// <summary>
        ///     服务端提示内容 。
        /// </summary>
        [DataMember(Order = 2, Name = "message")]
        public string Message { get; set; }

        #endregion
    }
}