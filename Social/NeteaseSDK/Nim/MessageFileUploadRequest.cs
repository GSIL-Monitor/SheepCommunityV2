using System;
using System.Runtime.Serialization;
using ServiceStack;
using ServiceStack.Text;

namespace Netease.Nim
{
    /// <summary>
    ///     文件上传的请求。
    /// </summary>
    /// <remarks>
    ///     请求说明：
    ///     http请求方式: POST
    ///     https://api.netease.im/nimserver/msg/upload.action
    /// </remarks>
    [DataContract]
    public class MessageFileUploadRequest
    {
        #region 属性

        /// <summary>
        ///     字符流base64串(Base64.encode(bytes)) ，最大15M的字符流。
        /// </summary>
        [DataMember(Order = 1, Name = "content")]
        public byte[] Content { get; set; }

        /// <summary>
        ///     上传文件类型。
        /// </summary>
        [DataMember(Order = 2, Name = "type")]
        public string Type { get; set; }

        #endregion

        #region 转换

        public string ToQueryString()
        {
            var builder = StringBuilderCache.Allocate();
            builder.Append("content=");
            builder.Append(Convert.ToBase64String(Content));
            if (!Type.IsNullOrEmpty())
            {
                builder.Append("&type=");
                builder.Append(Type);
            }
            return StringBuilderCache.ReturnAndFree(builder);
        }

        #endregion
    }
}