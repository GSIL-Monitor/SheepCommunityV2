using System.Runtime.Serialization;
using ServiceStack.Text;

namespace Netease.Nim
{
    /// <summary>
    ///     设置黑名单静音的请求。
    /// </summary>
    /// <remarks>
    ///     请求说明：
    ///     http请求方式: POST
    ///     https://api.netease.im/nimserver/user/setSpecialRelation.action
    /// </remarks>
    [DataContract]
    public class UserSetSpecialRelationRequest
    {
        #region 属性

        /// <summary>
        ///     用户帐号，最大长度32字符，必须保证一个APP内唯一。
        /// </summary>
        [DataMember(Order = 1, Name = "accid")]
        public string AccountId { get; set; }

        /// <summary>
        ///     被加黑或加静音的帐号，最大长度32字符，必须保证一个APP内唯一。
        /// </summary>
        [DataMember(Order = 2, Name = "targetAcc")]
        public string TargetAccountId { get; set; }

        /// <summary>
        ///     本次操作的关系类型,1:黑名单操作，2:静音列表操作。
        /// </summary>
        [DataMember(Order = 3, Name = "relationType")]
        public int Type { get; set; }

        /// <summary>
        ///     操作值，0:取消黑名单或静音，1:加入黑名单或静音。
        /// </summary>
        [DataMember(Order = 4, Name = "value")]
        public int Value { get; set; }

        #endregion

        #region 转换

        public string ToQueryString()
        {
            var builder = StringBuilderCache.Allocate();
            builder.Append("accid=");
            builder.Append(AccountId);
            builder.Append("&targetAcc=");
            builder.Append(TargetAccountId);
            builder.Append("&relationType=");
            builder.Append(Type);
            builder.Append("&value=");
            builder.Append(Value);
            return StringBuilderCache.ReturnAndFree(builder);
        }

        #endregion
    }
}