using System.Runtime.Serialization;

namespace Sina.Weibo
{
    /// <summary>
    ///     获取接口调用凭证的响应。
    /// </summary>
    [DataContract]
    public class GetTokenResponse : ErrorResponse
    {
        #region 属性

        /// <summary>
        ///     授权用户唯一标识。
        /// </summary>
        [DataMember(Order = 101, Name = "uid")]
        public string UserId { get; set; }

        /// <summary>
        ///     接口调用凭证的所属的应用程序标识。
        /// </summary>
        [DataMember(Order = 102, Name = "appkey")]
        public string AppKey { get; set; }

        /// <summary>
        ///     用户授权的Scope权限。
        /// </summary>
        [DataMember(Order = 103, Name = "scope")]
        public string Scope { get; set; }

        /// <summary>
        ///     接口调用凭证的创建日期，从1970年到创建日期的秒数。
        /// </summary>
        [DataMember(Order = 104, Name = "create_at")]
        public long CreateAt { get; set; }

        /// <summary>
        ///     接口调用凭证的剩余时间，单位是秒数。
        /// </summary>
        [DataMember(Order = 105, Name = "expire_in")]
        public long ExpireIn { get; set; }

        #endregion
    }
}