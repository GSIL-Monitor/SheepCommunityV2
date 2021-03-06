﻿using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Users.Entities;

namespace Sheep.ServiceModel.Users
{
    /// <summary>
    ///     查询并列举一组用户排行的请求。
    /// </summary>
    [Route("/users/ranks/query", HttpMethods.Get, Summary = "查询并列举一组用户排行")]
    [DataContract]
    public class UserRankList : IReturn<UserRankListResponse>
    {
        /// <summary>
        ///     排序的字段。（可选值：PostViewsCount, PostViewsRank, ParagraphViewsCount, ParagraphViewsRank 默认为 ParagraphViewsRank）
        /// </summary>
        [DataMember(Order = 1, Name = "orderby")]
        [ApiMember(Description = "排序的字段（可选值：PostViewsCount, PostViewsRank, ParagraphViewsCount, ParagraphViewsRank 默认为 ParagraphViewsRank）")]
        public string OrderBy { get; set; }

        /// <summary>
        ///     是否按降序排序。
        /// </summary>
        [DataMember(Order = 2, Name = "descending")]
        [ApiMember(Description = "是否按降序排序")]
        public bool? Descending { get; set; }

        /// <summary>
        ///     忽略的行数。
        /// </summary>
        [DataMember(Order = 3, Name = "skip")]
        [ApiMember(Description = "忽略的行数")]
        public int? Skip { get; set; }

        /// <summary>
        ///     获取的行数。
        /// </summary>
        [DataMember(Order = 4, Name = "limit")]
        [ApiMember(Description = "获取的行数")]
        public int? Limit { get; set; }
    }

    /// <summary>
    ///     查询并列举一组用户排行的响应。
    /// </summary>
    [DataContract]
    public class UserRankListResponse : IHasResponseStatus
    {
        /// <summary>
        ///     用户排行信息列表。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "用户排行信息列表")]
        public List<UserRankDto> UserRanks { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}