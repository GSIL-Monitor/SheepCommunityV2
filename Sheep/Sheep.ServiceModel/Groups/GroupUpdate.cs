﻿using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Groups.Entities;

namespace Sheep.ServiceModel.Groups
{
    /// <summary>
    ///     更新群组的请求。
    /// </summary>
    [Route("/groups/{GroupId}", HttpMethods.Put)]
    [DataContract]
    public class GroupUpdate : IReturn<GroupUpdateResponse>
    {
        /// <summary>
        ///     群组的编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public string GroupId { get; set; }

        /// <summary>
        ///     群组的类型。（可选值：Joinless, PublicOpen, PublicClosed, PrivateUnlisted, PrivateListed）
        /// </summary>
        [DataMember(Order = 2)]
        public string GroupType { get; set; }

        /// <summary>
        ///     名称。
        /// </summary>
        [DataMember(Order = 3)]
        public string Name { get; set; }

        /// <summary>
        ///     上级群组的编号。
        /// </summary>
        [DataMember(Order = 4)]
        public string ParentGroupId { get; set; }

        /// <summary>
        ///     代号。（如果没有指定将自动生成）
        /// </summary>
        [DataMember(Order = 5)]
        public string Code { get; set; }

        /// <summary>
        ///     说明。
        /// </summary>
        [DataMember(Order = 6)]
        public string Description { get; set; }

        /// <summary>
        ///     是否开启群组消息。
        /// </summary>
        [DataMember(Order = 7)]
        public bool? EnableGroupMessages { get; set; }
    }

    /// <summary>
    ///     更新群组的响应。
    /// </summary>
    [DataContract]
    public class GroupUpdateResponse : IHasResponseStatus
    {
        /// <summary>
        ///     群组信息。
        /// </summary>
        [DataMember(Order = 1)]
        public GroupDto Group { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}