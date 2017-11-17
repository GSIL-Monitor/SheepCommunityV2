﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Groups.Entities;

namespace Sheep.ServiceModel.Groups
{
    /// <summary>
    ///     列举一组群组的请求。
    /// </summary>
    [Route("/groups/query", HttpMethods.Get)]
    [DataContract]
    public class GroupList : IReturn<GroupListResponse>
    {
        /// <summary>
        ///     群组名称。
        /// </summary>
        [DataMember(Order = 1, Name = "namefilter")]
        public string NameFilter { get; set; }

        /// <summary>
        ///     创建日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 2, Name = "createdsince")]
        public DateTime? CreatedSince { get; set; }

        /// <summary>
        ///     修改日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 3, Name = "modifiedsince")]
        public DateTime? ModifiedSince { get; set; }

        /// <summary>
        ///     加入群组的方式。（可选值：Direct, RequireVerification, Joinless）
        /// </summary>
        [DataMember(Order = 4, Name = "joinmode")]
        public string JoinMode { get; set; }

        /// <summary>
        ///     非群组成员是否可以访问群组内容。
        /// </summary>
        [DataMember(Order = 5, Name = "ispublic")]
        public bool? IsPublic { get; set; }

        /// <summary>
        ///     状态。（可选值：待审核, 审核通过, 已禁止, 审核失败, 等待删除）
        /// </summary>
        [DataMember(Order = 6, Name = "status")]
        public string Status { get; set; }

        /// <summary>
        ///     排序的字段。（可选值：DisplayName, FullName, RefId, JoinMode, Status, CreatedDate, ModifiedDate, TotalMembers 默认为 CreatedDate）
        /// </summary>
        [DataMember(Order = 7, Name = "orderby")]
        public string OrderBy { get; set; }

        /// <summary>
        ///     是否按降序排序。
        /// </summary>
        [DataMember(Order = 8, Name = "descending")]
        public bool? Descending { get; set; }

        /// <summary>
        ///     忽略的行数。
        /// </summary>
        [DataMember(Order = 9, Name = "skip")]
        public int? Skip { get; set; }

        /// <summary>
        ///     获取的行数。
        /// </summary>
        [DataMember(Order = 10, Name = "limit")]
        public int? Limit { get; set; }
    }

    /// <summary>
    ///     列举一组群组的响应。
    /// </summary>
    [DataContract]
    public class GroupListResponse : IHasResponseStatus
    {
        /// <summary>
        ///     群组信息列表。
        /// </summary>
        [DataMember(Order = 1)]
        public List<GroupDto> Groups { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}