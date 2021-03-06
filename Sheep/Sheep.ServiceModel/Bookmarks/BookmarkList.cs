﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Bookmarks.Entities;

namespace Sheep.ServiceModel.Bookmarks
{
    /// <summary>
    ///     根据上级查询并列举一组收藏信息的请求。
    /// </summary>
    [Route("/bookmarks/query/byparent", HttpMethods.Get, Summary = "根据上级查询并列举一组收藏信息")]
    [DataContract]
    public class BookmarkListByParent : IReturn<BookmarkListResponse>
    {
        /// <summary>
        ///     上级编号。（如帖子编号）
        /// </summary>
        [DataMember(Order = 1, Name = "parentid", IsRequired = true)]
        [ApiMember(Description = "上级编号（如帖子编号）")]
        public string ParentId { get; set; }

        /// <summary>
        ///     创建日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 2, Name = "createdsince")]
        [ApiMember(Description = "创建日期在指定的时间之后")]
        public long? CreatedSince { get; set; }

        /// <summary>
        ///     排序的字段。（可选值：ParentId, CreatedDate 默认为 CreatedDate）
        /// </summary>
        [DataMember(Order = 3, Name = "orderby")]
        [ApiMember(Description = "排序的字段（可选值：ParentId, CreatedDate 默认为 CreatedDate）")]
        public string OrderBy { get; set; }

        /// <summary>
        ///     是否按降序排序。
        /// </summary>
        [DataMember(Order = 4, Name = "descending")]
        [ApiMember(Description = "是否按降序排序")]
        public bool? Descending { get; set; }

        /// <summary>
        ///     忽略的行数。
        /// </summary>
        [DataMember(Order = 5, Name = "skip")]
        [ApiMember(Description = "忽略的行数")]
        public int? Skip { get; set; }

        /// <summary>
        ///     获取的行数。
        /// </summary>
        [DataMember(Order = 6, Name = "limit")]
        [ApiMember(Description = "获取的行数")]
        public int? Limit { get; set; }
    }

    /// <summary>
    ///     根据用户查询并列举一组收藏信息的请求。
    /// </summary>
    [Route("/bookmarks/query/byuser", HttpMethods.Get, Summary = "根据用户查询并列举一组收藏信息")]
    [DataContract]
    public class BookmarkListByUser : IReturn<BookmarkListResponse>
    {
        /// <summary>
        ///     用户编号。
        /// </summary>
        [DataMember(Order = 1, Name = "userid", IsRequired = true)]
        [ApiMember(Description = "用户编号")]
        public int UserId { get; set; }

        /// <summary>
        ///     上级类型。（可选值：帖子, 章, 节）
        /// </summary>
        [DataMember(Order = 2, Name = "parenttype")]
        [ApiMember(Description = "上级类型（可选值：帖子, 章, 节）")]
        public string ParentType { get; set; }

        /// <summary>
        ///     创建日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 3, Name = "createdsince")]
        [ApiMember(Description = "创建日期在指定的时间之后")]
        public long? CreatedSince { get; set; }

        /// <summary>
        ///     排序的字段。（可选值：ParentId, CreatedDate 默认为 CreatedDate）
        /// </summary>
        [DataMember(Order = 4, Name = "orderby")]
        [ApiMember(Description = "排序的字段（可选值：ParentId, CreatedDate 默认为 CreatedDate）")]
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
    ///     列举一组收藏信息的响应。
    /// </summary>
    [DataContract]
    public class BookmarkListResponse : IHasResponseStatus
    {
        /// <summary>
        ///     收藏信息列表。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "收藏信息列表")]
        public List<BookmarkDto> Bookmarks { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}