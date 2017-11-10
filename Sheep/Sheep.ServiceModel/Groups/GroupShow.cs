﻿using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Groups.Entities;

namespace Sheep.ServiceModel.Groups
{
    /// <summary>
    ///     显示一个群组的请求。
    /// </summary>
    [Route("/groups/{GroupId}", HttpMethods.Get)]
    [DataContract]
    public class GroupShow : IReturn<GroupShowResponse>
    {
        /// <summary>
        ///     群组的编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public string GroupId { get; set; }
    }

    /// <summary>
    ///     根据关联的第三方编号显示一个群组的请求。
    /// </summary>
    [Route("/groups/show/{RefId}", HttpMethods.Get)]
    [DataContract]
    public class GroupShowByRefId : IReturn<GroupShowResponse>
    {
        /// <summary>
        ///     关联的第三方编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public string RefId { get; set; }
    }

    /// <summary>
    ///     显示一个群组的响应。
    /// </summary>
    [DataContract]
    public class GroupShowResponse : IHasResponseStatus
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