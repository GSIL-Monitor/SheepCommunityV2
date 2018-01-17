﻿using System;
using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.Job.ServiceModel.Users
{
    /// <summary>
    ///     查询并计算一组用户声望的请求。
    /// </summary>
    [Route("/users/calculate", HttpMethods.Put, Summary = "查询并计算一组用户声望信息")]
    [DataContract]
    public class UserCalculate : IReturn<UserCalculateResponse>
    {
        /// <summary>
        ///     用户名称或电子邮件地址过滤。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "用户名称或电子邮件地址过滤")]
        public string UserNameFilter { get; set; }

        /// <summary>
        ///     姓名过滤（包括显示名称、姓名全称）。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "姓名过滤（包括显示名称、姓名全称）")]
        public string NameFilter { get; set; }

        /// <summary>
        ///     创建日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 3)]
        [ApiMember(Description = "创建日期在指定的时间之后")]
        public DateTime? CreatedSince { get; set; }

        /// <summary>
        ///     修改日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 4)]
        [ApiMember(Description = "创建日期在指定的时间之后")]
        public DateTime? ModifiedSince { get; set; }

        /// <summary>
        ///     锁定日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 5)]
        [ApiMember(Description = "锁定日期在指定的时间之后")]
        public DateTime? LockedSince { get; set; }

        /// <summary>
        ///     排序的字段。（可选值：UserName, Email, DisplayName, FullName, BirthDate, TimeZone, Language, Status, CreatedDate,
        ///     ModifiedDate, Reputation 默认为 CreatedDate）
        /// </summary>
        [DataMember(Order = 6)]
        [ApiMember(Description = "排序的字段（可选值：UserName, Email, DisplayName, FullName, BirthDate, TimeZone, Language, Status, CreatedDate, ModifiedDate, Reputation 默认为 CreatedDate）")]
        public string OrderBy { get; set; }

        /// <summary>
        ///     是否按降序排序。
        /// </summary>
        [DataMember(Order = 7)]
        [ApiMember(Description = "是否按降序排序")]
        public bool? Descending { get; set; }

        /// <summary>
        ///     忽略的行数。
        /// </summary>
        [DataMember(Order = 8)]
        [ApiMember(Description = "忽略的行数")]
        public int? Skip { get; set; }

        /// <summary>
        ///     获取的行数。
        /// </summary>
        [DataMember(Order = 9)]
        [ApiMember(Description = "获取的行数")]
        public int? Limit { get; set; }
    }

    /// <summary>
    ///     查询并计算一组用户声望的响应。
    /// </summary>
    [DataContract]
    public class UserCalculateResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}