using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using Sheep.Common.Infrastructure;
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
        ///     上级群组的编号。
        /// </summary>
        [DataMember(Order = 1)]
        public string ParentGroupId { get; set; }

        /// <summary>
        ///     过滤群组名称。
        /// </summary>
        [DataMember(Order = 2)]
        public string NameFilter { get; set; }

        /// <summary>
        ///     容器的编号列表。
        /// </summary>
        [DataMember(Order = 3)]
        public string[] ContainerIds { get; set; }

        /// <summary>
        ///     群组的类型列表。（可选值：Joinless, PublicOpen, PublicClosed, PrivateUnlisted, PrivateListed 和 All）
        /// </summary>
        [DataMember(Order = 4)]
        public string[] GroupTypes { get; set; }

        /// <summary>
        ///     是否包含所有子群组。
        /// </summary>
        [DataMember(Order = 5)]
        public bool? IncludeAllSubGroups { get; set; }

        /// <summary>
        ///     排序的字段。（可选值：Name, SortOrder, ModifiedDate, 默认为 Name）
        /// </summary>
        [DataMember(Order = 6)]
        public string OrderBy { get; set; }

        /// <summary>
        ///     是否按降序排序。
        /// </summary>
        [DataMember(Order = 7)]
        public bool? Descending { get; set; }

        /// <summary>
        ///     忽略的行数。
        /// </summary>
        [DataMember(Order = 8)]
        public int? Skip { get; set; }

        /// <summary>
        ///     获取的行数。
        /// </summary>
        [DataMember(Order = 9)]
        public int? Take { get; set; }
    }

    /// <summary>
    ///     列举一组群组的响应。
    /// </summary>
    [DataContract]
    public class GroupListResponse : IHasResponseStatus, IPaged
    {
        /// <summary>
        ///     群组信息列表。
        /// </summary>
        [DataMember(Order = 1)]
        public List<GroupDto> Groups { get; set; }

        /// <summary>
        ///     当前分页号。
        /// </summary>
        [DataMember(Order = 2)]
        public int PageNumber { get; set; }

        /// <summary>
        ///     单页行数。
        /// </summary>
        [DataMember(Order = 3)]
        public int PageSize { get; set; }

        /// <summary>
        ///     总行数。
        /// </summary>
        [DataMember(Order = 4)]
        public long TotalCount { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 5)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}