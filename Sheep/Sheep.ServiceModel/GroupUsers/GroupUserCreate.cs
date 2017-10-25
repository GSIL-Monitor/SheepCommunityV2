﻿using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.GroupUsers.Entities;

namespace Sheep.ServiceModel.GroupUsers
{
    /// <summary>
    ///     新建一个群组用户的请求。
    /// </summary>
    [Route("/groups/{GroupId}/users/{UserId}", HttpMethods.Post)]
    [DataContract]
    public class GroupUserCreate : IReturn<GroupUserCreateResponse>
    {
        /// <summary>
        ///     群组的编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public string GroupId { get; set; }

        /// <summary>
        ///     要加入群组的用户编号。
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        public int UserId { get; set; }

        /// <summary>
        ///     成员的类型。（可选值：Owner, Manager, Member, 默认为 Member）
        /// </summary>
        [DataMember(Order = 2)]
        public string MembershipType { get; set; }

        /// <summary>
        ///     通知用户的消息。
        /// </summary>
        [DataMember(Order = 3)]
        public string Message { get; set; }
    }

    /// <summary>
    ///     新建一个群组用户的响应。
    /// </summary>
    [DataContract]
    public class GroupUserCreateResponse : IHasResponseStatus
    {
        /// <summary>
        ///     群组用户信息。
        /// </summary>
        [DataMember(Order = 1)]
        public GroupUserDto GroupUser { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}