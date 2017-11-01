using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace Sina.Weibo
{
    /// <summary>
    ///     错误信息响应。
    /// </summary>
    [DataContract]
    public class ErrorResponse
    {
        /// <summary>
        ///     请求处理的结果，空表示处理成功，其他表示错误码。
        /// </summary>
        [DataMember(Order = 1, Name = "error")]
        public string Error { get; set; }

        /// <summary>
        ///     错误内部编号，0为成功，其他为失败。
        /// </summary>
        [DataMember(Order = 2, Name = "error_code")]
        public int ErrorCode { get; set; }

        /// <summary>
        ///     错误的描述信息 。
        /// </summary>
        [DataMember(Order = 3, Name = "error_description")]
        public string ErrorDescription { get; set; }

        /// <summary>
        ///     可读的网页URI，带有关于错误的信息，用于为终端用户提供与错误有关的额外信息。
        /// </summary>
        [DataMember(Order = 4, Name = "error_url")]
        public string ErrorUrl { get; set; }

        /// <summary>
        ///     原始请求地址。
        /// </summary>
        [DataMember(Order = 5, Name = "request")]
        public string Request { get; set; }
    }
}