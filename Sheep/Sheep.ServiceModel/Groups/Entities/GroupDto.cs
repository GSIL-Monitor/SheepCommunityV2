using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using ServiceStack.Model;
using Sheep.ServiceModel.Containers.Entities;

namespace Sheep.ServiceModel.Groups.Entities
{
    /// <summary>
    ///     群组信息。
    /// </summary>
    [DataContract]
    public class GroupDto : IHasStringId, IMeta
    {
        /// <summary>
        ///     编号。
        /// </summary>
        [DataMember(Order = 1)]
        public string Id { get; set; }

        /// <summary>
        ///     类型。（可选值：Joinless, PublicOpen, PublicClosed, PrivateUnlisted, PrivateListed）
        /// </summary>
        [DataMember(Order = 2)]
        public string Type { get; set; }

        /// <summary>
        ///     上级编号。
        /// </summary>
        [DataMember(Order = 3)]
        public string ParentId { get; set; }

        /// <summary>
        ///     所属的容器。
        /// </summary>
        [DataMember(Order = 4)]
        public ContainerDto Container { get; set; }

        /// <summary>
        ///     群组的容器编号。
        /// </summary>
        [DataMember(Order = 5)]
        public string ContainerId { get; set; }

        /// <summary>
        ///     群组的容器类型。
        /// </summary>
        [DataMember(Order = 6)]
        public string ContainerType { get; set; }

        /// <summary>
        ///     代号。
        /// </summary>
        [DataMember(Order = 7)]
        public string Code { get; set; }

        /// <summary>
        ///     名称。
        /// </summary>
        [DataMember(Order = 8)]
        public string Name { get; set; }

        /// <summary>
        ///     说明。
        /// </summary>
        [DataMember(Order = 9)]
        public string Description { get; set; }

        /// <summary>
        ///     图像网址。
        /// </summary>
        [DataMember(Order = 10)]
        public string AvatarUrl { get; set; }

        /// <summary>
        ///     创建日期。
        /// </summary>
        [DataMember(Order = 11)]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        ///     是否开启。
        /// </summary>
        [DataMember(Order = 12)]
        public bool IsEnabled { get; set; }

        /// <summary>
        ///     是否为匿名用户开启联系方式。
        /// </summary>
        [DataMember(Order = 13)]
        public bool EnableContact { get; set; }

        /// <summary>
        ///     是否开启群组消息。
        /// </summary>
        [DataMember(Order = 14)]
        public bool EnableGroupMessages { get; set; }

        /// <summary>
        ///     子群组的数量。
        /// </summary>
        [DataMember(Order = 15)]
        public int SubGroupsCount { get; set; }

        /// <summary>
        ///     所有的成员数。
        /// </summary>
        [DataMember(Order = 16)]
        public int TotalMembersCount { get; set; }

        /// <summary>
        ///     扩展字段。
        /// </summary>
        [DataMember(Order = 17)]
        public Dictionary<string, string> Meta { get; set; }
    }
}