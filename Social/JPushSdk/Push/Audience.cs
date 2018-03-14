using System.Collections.Generic;
using System.Runtime.Serialization;

namespace JPush.Push
{
    /// <summary>
    ///     推送目标。
    ///     推送设备对象，表示一条推送可以被推送到哪些设备列表。确认推送设备对象，JPush 提供了多种方式，比如：别名、标签、注册ID、分群、广播等。
    ///     <see cref="http://docs.jiguang.cn/jpush/server/push/rest_api_v3_push/#audience" />
    /// </summary>
    [DataContract]
    public class Audience
    {
        #region 属性

        /// <summary>
        ///     标签数组。多个标签之间是 OR 的关系，即取并集。
        ///     用标签来进行大规模的设备属性、用户属性分群。 一次推送最多 20 个。
        ///     1、有效的 tag 组成：字母（区分大小写）、数字、下划线、汉字、特殊字符@!#$&*+=.|￥。
        ///     2、限制：每一个 tag 的长度限制为 40 字节。（判断长度需采用UTF-8编码）
        /// </summary>
        [DataMember(Order = 1, Name = "tag")]
        public List<string> Tags { get; set; }

        /// <summary>
        ///     标签数组。多个标签之间是 AND 关系，即取交集。
        ///     注意与 tag 区分。一次推送最多 20 个。
        /// </summary>
        [DataMember(Order = 2, Name = "tag_and")]
        public List<string> TagsAnd { get; set; }

        /// <summary>
        ///     标签数组。多个标签之间，先取多标签的并集，再对该结果取补集。
        ///     一次推送最多 20 个。
        /// </summary>
        [DataMember(Order = 3, Name = "tag_not")]
        public List<string> TagsNot { get; set; }

        /// <summary>
        ///     别名数组。多个别名之间是 OR 关系，即取并集。
        ///     用别名来标识一个用户。一个设备只能绑定一个别名，但多个设备可以绑定同一个别名。一次推送最多 1000 个。
        ///     1、有效的 alias 组成：字母（区分大小写）、数字、下划线、汉字、特殊字符@!#$&*+=.|￥。
        ///     2、限制：每一个 alias 的长度限制为 40 字节。（判断长度需采用UTF-8编码）
        /// </summary>
        [DataMember(Order = 4, Name = "alias")]
        public List<string> Aliases { get; set; }

        /// <summary>
        ///     注册ID数组。多个注册ID之间是 OR 关系，即取并集。
        /// </summary>
        [DataMember(Order = 5, Name = "registration_id")]
        public List<string> RegistrationIds { get; set; }

        /// <summary>
        ///     在页面创建的用户分群的 ID。定义为数组，但目前限制一次只能推送一个。
        ///     目前限制是一次只能推送一个。
        /// </summary>
        [DataMember(Order = 6, Name = "segment")]
        public List<string> Segments { get; set; }

        /// <summary>
        ///     在页面创建的 A/B 测试的 ID。定义为数组，但目前限制是一次只能推送一个。
        ///     目前限制一次只能推送一个。
        /// </summary>
        [DataMember(Order = 7, Name = "abtest")]
        public List<string> Abtests { get; set; }

        #endregion
    }
}