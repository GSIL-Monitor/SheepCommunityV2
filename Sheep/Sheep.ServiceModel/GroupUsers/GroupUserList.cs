using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using Sheep.Common.Infrastructure;
using Sheep.ServiceModel.GroupUsers.Entities;

namespace Sheep.ServiceModel.GroupUsers
{
    /// <summary>
    ///     列举一组群组用户的请求。
    /// </summary>
    [Route("/groups/{GroupId}/users/query", HttpMethods.Get)]
    [DataContract]
    public class GroupUserList : IReturn<GroupUserListResponse>
    {
        /// <summary>
        ///     群组的编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public string GroupId { get; set; }

        /// <summary>
        ///     过滤用户名称或显示名称。
        /// </summary>
        [DataMember(Order = 2)]
        public string NameFilter { get; set; }

        /// <summary>
        ///     过滤成员的类型。（可选值：Owner, Manager, Member, 默认为 Member）
        /// </summary>
        [DataMember(Order = 3)]
        public string MembershipType { get; set; }

        /// <summary>
        ///     过滤用户注册日期在指定日期之后。
        /// </summary>
        [DataMember(Order = 4)]
        public DateTime? CreatedAfter { get; set; }

        /// <summary>
        ///     排序的字段。（可选值：UserName, DisplayName, CreatedDate, MembershipType, 默认为 UserName）
        /// </summary>
        [DataMember(Order = 5)]
        public string OrderBy { get; set; }

        /// <summary>
        ///     是否按降序排序。
        /// </summary>
        [DataMember(Order = 6)]
        public bool? OrderDescending { get; set; }

        /// <summary>
        ///     忽略的行数。
        /// </summary>
        [DataMember(Order = 7)]
        public int? Skip { get; set; }

        /// <summary>
        ///     获取的行数。
        /// </summary>
        [DataMember(Order = 8)]
        public int? Take { get; set; }
    }

    /// <summary>
    ///     列举一组群组用户的响应。
    /// </summary>
    [DataContract]
    public class GroupUserListResponse : IHasResponseStatus, IPaged
    {
        /// <summary>
        ///     群组用户信息列表。
        /// </summary>
        [DataMember(Order = 1)]
        public List<GroupUserDto> GroupUsers { get; set; }

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