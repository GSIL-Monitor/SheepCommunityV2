using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
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
        [DataMember(Order = 1, Name = "usernamefilter")]
        public string UserNameFilter { get; set; }

        /// <summary>
        ///     过滤姓名。（包括显示名称、姓名）
        /// </summary>
        [DataMember(Order = 2, Name = "namefilter")]
        public string NameFilter { get; set; }

        /// <summary>
        ///     过滤创建日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 3, Name = "createdsince")]
        public DateTime? CreatedSince { get; set; }

        /// <summary>
        ///     过滤修改日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 4, Name = "modifiedsince")]
        public DateTime? ModifiedSince { get; set; }

        /// <summary>
        ///     过滤锁定日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 5, Name = "lockedsince")]
        public DateTime? LockedSince { get; set; }

        /// <summary>
        ///     过滤帐户状态。（可选值：Approved, Banned, Disapproved, PendingDeletion）
        /// </summary>
        [DataMember(Order = 6, Name = "accountstatus")]
        public string AccountStatus { get; set; }

        /// <summary>
        ///     排序的字段。（可选值：UserName, Email, DisplayName, CreatedDate, ModifiedDate, 默认为 UserName）
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
    ///     列举一组用户的响应。
    /// </summary>
    [DataContract]
    public class UserListResponse : IHasResponseStatus
    {
        /// <summary>
        ///     用户信息列表。
        /// </summary>
        [DataMember(Order = 1)]
        public List<UserDto> Users { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}