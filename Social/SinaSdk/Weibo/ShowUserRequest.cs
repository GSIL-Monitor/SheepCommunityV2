using System.Runtime.Serialization;

namespace Sina.Weibo
{
    /// <summary>
    ///     根据用户编号获取用户信息的请求。
    /// </summary>
    /// <remarks>
    ///     请求说明：
    ///     http请求方式: GET
    ///     https://api.weibo.com/2/users/show.json
    /// </remarks>
    [DataContract]
    public class ShowUserRequest
    {
        #region 属性

        /// <summary>
        ///     授权时生成的接口调用凭证。
        /// </summary>
        [DataMember(Order = 1, Name = "access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        ///     需要查询的用户编号。
        /// </summary>
        [DataMember(Order = 2, Name = "uid")]
        public string UserId { get; set; }

        #endregion

        #region 转换

        /// <summary>
        ///     转换成查询字符串格式的文本。
        /// </summary>
        /// <returns>查询字符串格式的文本。</returns>
        public string ToQueryString()
        {
            return string.Format("access_token={0}&uid={1}", AccessToken, UserId);
        }

        #endregion
    }
}