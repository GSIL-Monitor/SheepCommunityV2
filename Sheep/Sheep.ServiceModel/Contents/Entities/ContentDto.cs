using System;
using System.Runtime.Serialization;
using ServiceStack.Model;
using Sheep.ServiceModel.Applications.Entities;
using Sheep.ServiceModel.BasicUsers.Entities;
using Sheep.ServiceModel.Users.Entities;

namespace Sheep.ServiceModel.Contents.Entities
{
    /// <summary>
    ///     内容信息。
    /// </summary>
    [DataContract]
    public class ContentDto : IHasStringId
    {
        /// <summary>
        ///     编号。
        /// </summary>
        [DataMember(Order = 1)]
        public string Id { get; set; }

        /// <summary>
        ///     类型。
        /// </summary>
        [DataMember(Order = 2)]
        public string Type { get; set; }

        /// <summary>
        ///     所属的应用程序。
        /// </summary>
        [DataMember(Order = 3)]
        public ApplicationDto Application { get; set; }

        /// <summary>
        ///     名称。
        /// </summary>
        [DataMember(Order = 4)]
        public string Name { get; set; }

        /// <summary>
        ///     说明。
        /// </summary>
        [DataMember(Order = 5)]
        public string Description { get; set; }

        /// <summary>
        ///     图像网址。
        /// </summary>
        [DataMember(Order = 6)]
        public string AvatarUrl { get; set; }

        /// <summary>
        ///     创建用户。
        /// </summary>
        [DataMember(Order = 7)]
        public BasicUserDto CreatedByUser { get; set; }

        /// <summary>
        ///     创建日期。
        /// </summary>
        [DataMember(Order = 8)]
        public DateTime CreatedDate { get; set; }
    }
}