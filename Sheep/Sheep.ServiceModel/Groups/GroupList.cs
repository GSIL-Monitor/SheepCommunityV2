using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Groups.Entities;

namespace Sheep.ServiceModel.Groups
{
    /// <summary>
    ///     查询并列举一组群组的请求。
    /// </summary>
    [Route("/groups/query", HttpMethods.Get, Summary = "查询并列举一组群组")]
    [DataContract]
    public class GroupList : IReturn<GroupListResponse>
    {
        /// <summary>
        ///     名称过滤（包括显示名称、名称全称）。
        /// </summary>
        [DataMember(Order = 1, Name = "namefilter")]
        [ApiMember(Description = "名称过滤（包括显示名称、名称全称）")]
        public string NameFilter { get; set; }

        /// <summary>
        ///     创建日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 2, Name = "createdsince")]
        [ApiMember(Description = "创建日期在指定的时间之后")]
        public long? CreatedSince { get; set; }

        /// <summary>
        ///     修改日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 3, Name = "modifiedsince")]
        [ApiMember(Description = "创建日期在指定的时间之后")]
        public long? ModifiedSince { get; set; }

        /// <summary>
        ///     排序的字段。（可选值：DisplayName, FullName, CreatedDate, ModifiedDate, 默认为 CreatedDate）
        /// </summary>
        [DataMember(Order = 4, Name = "orderby")]
        [ApiMember(Description = "排序的字段（可选值：DisplayName, FullName, CreatedDate, ModifiedDate, 默认为 CreatedDate）")]
        public string OrderBy { get; set; }

        /// <summary>
        ///     是否按降序排序。
        /// </summary>
        [DataMember(Order = 5, Name = "descending")]
        [ApiMember(Description = "是否按降序排序")]
        public bool? Descending { get; set; }

        /// <summary>
        ///     忽略的行数。
        /// </summary>
        [DataMember(Order = 6, Name = "skip")]
        [ApiMember(Description = "忽略的行数")]
        public int? Skip { get; set; }

        /// <summary>
        ///     获取的行数。
        /// </summary>
        [DataMember(Order = 7, Name = "limit")]
        [ApiMember(Description = "获取的行数")]
        public int? Limit { get; set; }
    }

    /// <summary>
    ///     查询并列举一组群组的响应。
    /// </summary>
    [DataContract]
    public class GroupListResponse : IHasResponseStatus
    {
        /// <summary>
        ///     群组信息列表。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "群组信息列表")]
        public List<GroupDto> Groups { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}