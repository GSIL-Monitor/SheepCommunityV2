using System.Runtime.Serialization;

namespace Tencent.Weixin
{
    /// <summary>
    ///     错误信息响应。
    /// </summary>
    [DataContract]
    public class ErrorResponse
    {
        #region 属性

        /// <summary>
        ///     错误内部编号，0为成功，其他为失败。
        /// </summary>
        [DataMember(Order = 1, Name = "errcode")]
        public int ErrorCode { get; set; }

        /// <summary>
        ///     错误的描述信息 。
        /// </summary>
        [DataMember(Order = 2, Name = "errmsg")]
        public string ErrorMessage { get; set; }

        #endregion
    }
}