using System;
using System.Runtime.Serialization;
using Sheep.ServiceModel.Users.Entities;

namespace Sheep.ServiceModel.Bookmarks.Entities
{
    /// <summary>
    ///     书签信息。
    /// </summary>
    [DataContract]
    public class BookmarkDto
    {
        /// <summary>
        ///     类型。
        /// </summary>
        [DataMember(Order = 1)]
        public string Type { get; set; }

        ///// <summary>
        /////     书签标记的内容。
        ///// </summary>
        //[DataMember(Order = 2)]
        //public ContentDto Content { get; set; }

        /// <summary>
        ///     书签的用户。
        /// </summary>
        [DataMember(Order = 3)]
        public BasicUserDto User { get; set; }

        /// <summary>
        ///     创建日期。
        /// </summary>
        [DataMember(Order = 4)]
        public DateTime CreatedDate { get; set; }
    }
}