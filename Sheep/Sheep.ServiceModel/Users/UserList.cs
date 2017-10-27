using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using Sheep.Common.Infrastructure;
using Sheep.ServiceModel.Users.Entities;

namespace Sheep.ServiceModel.Users
{
    /// <summary>
    ///     列举一组用户的请求。
    /// </summary>
    [Route("/users/query", HttpMethods.Get)]
    [DataContract]
    public class UserList : IReturn<UserListResponse>
    {
        /// <summary>
        ///     过滤用户名称或电子邮件地址。
        /// </summary>
        [DataMember(Order = 1)]
        public string UserNameOrEmail { get; set; }

        /// <summary>
        ///     过滤手机号码。
        /// </summary>
        [DataMember(Order = 2)]
        public string PhoneNumber { get; set; }

        /// <summary>
        ///     过滤姓名。（包括显示名称、姓名）
        /// </summary>
        [DataMember(Order = 3)]
        public string Name { get; set; }

        /// <summary>
        ///     过滤创建日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 4)]
        public DateTime? CreatedSince { get; set; }

        /// <summary>
        ///     过滤修改日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 5)]
        public DateTime? ModifiedSince { get; set; }

        /// <summary>
        ///     过滤是否已被锁定。
        /// </summary>
        [DataMember(Order = 6)]
        public bool? IsLocked { get; set; }

        /// <summary>
        ///     过滤帐户状态。（可选值：PendingApproval, Approved, Banned, Disapproved, PendingDeletion）
        /// </summary>
        [DataMember(Order = 7)]
        public string AccountStatus { get; set; }

        /// <summary>
        ///     过滤所属的角色。
        /// </summary>
        [DataMember(Order = 8)]
        public string Role { get; set; }

        /// <summary>
        ///     排序的字段。（可选值：UserName, Email, DisplayName, CreatedDate, ModifiedDate, 默认为 UserName）
        /// </summary>
        [DataMember(Order = 9)]
        public string OrderBy { get; set; }

        /// <summary>
        ///     是否按降序排序。
        /// </summary>
        [DataMember(Order = 10)]
        public bool? OrderDescending { get; set; }

        /// <summary>
        ///     忽略的行数。
        /// </summary>
        [DataMember(Order = 11)]
        public int? Skip { get; set; }

        /// <summary>
        ///     获取的行数。
        /// </summary>
        [DataMember(Order = 12)]
        public int? Take { get; set; }
    }

    /// <summary>
    ///     列举一组用户的响应。
    /// </summary>
    [DataContract]
    public class UserListResponse : IHasResponseStatus, IPaged
    {
        /// <summary>
        ///     用户信息列表。
        /// </summary>
        [DataMember(Order = 1)]
        public List<UserDto> Users { get; set; }

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