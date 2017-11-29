﻿using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Groups
{
    /// <summary>
    ///     更改图标的请求。
    /// </summary>
    [Route("/groups/{GroupId}/icon", HttpMethods.Put)]
    [DataContract]
    public class GroupChangeIcon : IReturn<GroupChangeIconResponse>
    {
        /// <summary>
        ///     群组的编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public string GroupId { get; set; }

        /// <summary>
        ///     来源图标的地址。
        /// </summary>
        [DataMember(Order = 2)]
        public string SourceIconUrl { get; set; }
    }

    /// <summary>
    ///     更改图标的响应。
    /// </summary>
    [DataContract]
    public class GroupChangeIconResponse : IHasResponseStatus
    {
        /// <summary>
        ///     图标的地址。
        /// </summary>
        [DataMember(Order = 1)]
        public string IconUrl { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}