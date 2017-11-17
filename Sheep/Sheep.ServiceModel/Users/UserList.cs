using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Users.Entities;

namespace Sheep.ServiceModel.Users
{
    /// <summary>
    ///     查询并列举一组用户的请求。
    /// </summary>
    [Route("/users/query", HttpMethods.Get, Summary = "查询并列举一组用户")]
    [DataContract]
    public class UserList : IReturn<UserListResponse>
    {
        /// <summary>
        ///     用户名称或电子邮件地址过滤。
        /// </summary>
        [DataMember(Order = 1, Name = "usernamefilter")]
        [ApiMember(Description = "用户名称或电子邮件地址过滤")]
        public string UserNameFilter { get; set; }

        /// <summary>
        ///     姓名过滤（包括显示名称、姓名全称）。
        /// </summary>
        [DataMember(Order = 2, Name = "namefilter")]
        [ApiMember(Description = "姓名过滤（包括显示名称、姓名全称）")]
        public string NameFilter { get; set; }

        /// <summary>
        ///     创建日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 3, Name = "createdsince")]
        [ApiMember(Description = "创建日期在指定的时间之后")]
        public DateTime? CreatedSince { get; set; }

        /// <summary>
        ///     修改日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 4, Name = "modifiedsince")]
        [ApiMember(Description = "创建日期在指定的时间之后")]
        public DateTime? ModifiedSince { get; set; }

        /// <summary>
        ///     锁定日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 5, Name = "lockedsince")]
        [ApiMember(Description = "锁定日期在指定的时间之后")]
        public DateTime? LockedSince { get; set; }

        /// <summary>
        ///     排序的字段。（可选值：UserName, Email, DisplayName, CreatedDate, ModifiedDate, 默认为 CreatedDate）
        /// </summary>
        [DataMember(Order = 6, Name = "orderby")]
        [ApiMember(Description = "排序的字段（可选值：UserName, Email, DisplayName, CreatedDate, ModifiedDate, 默认为 CreatedDate）")]
        public string OrderBy { get; set; }

        /// <summary>
        ///     是否按降序排序。
        /// </summary>
        [DataMember(Order = 7, Name = "descending")]
        [ApiMember(Description = "是否按降序排序")]
        public bool? Descending { get; set; }

        /// <summary>
        ///     忽略的行数。
        /// </summary>
        [DataMember(Order = 9, Name = "skip")]
        [ApiMember(Description = "忽略的行数")]
        public int? Skip { get; set; }

        /// <summary>
        ///     获取的行数。
        /// </summary>
        [DataMember(Order = 10, Name = "limit")]
        [ApiMember(Description = "获取的行数")]
        public int? Limit { get; set; }
    }

    /// <summary>
    ///     查询并列举一组用户的响应。
    /// </summary>
    [DataContract]
    public class UserListResponse : IHasResponseStatus
    {
        /// <summary>
        ///     用户信息列表。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "用户信息列表")]
        public List<UserDto> Users { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}