using System.Runtime.Serialization;
using Sheep.ServiceModel.Users.Entities;

namespace Sheep.ServiceModel.Bookmarks.Entities
{
    /// <summary>
    ///     收藏信息。
    /// </summary>
    [DataContract]
    public class BookmarkDto
    {
        /// <summary>
        ///     上级类型。（可选值：帖子, 章, 节）
        /// </summary>
        [DataMember(Order = 1)]
        public string ParentType { get; set; }

        /// <summary>
        ///     上级编号。（如帖子编号）
        /// </summary>
        [DataMember(Order = 2)]
        public string ParentId { get; set; }

        /// <summary>
        ///     上级标题。
        /// </summary>
        [DataMember(Order = 3)]
        public string ParentTitle { get; set; }

        /// <summary>
        ///     用户。
        /// </summary>
        [DataMember(Order = 4)]
        public BasicUserDto User { get; set; }

        /// <summary>
        ///     创建日期。
        /// </summary>
        [DataMember(Order = 5)]
        public long CreatedDate { get; set; }
    }
}