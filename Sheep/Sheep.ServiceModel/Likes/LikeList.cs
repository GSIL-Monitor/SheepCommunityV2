using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Likes.Entities;

namespace Sheep.ServiceModel.Likes
{
    /// <summary>
    ///     列举一组点赞信息的请求。
    /// </summary>
    [Route("/likes/query", HttpMethods.Get, Summary = "列举一组点赞信息")]
    [DataContract]
    public class LikeList : IReturn<LikeListResponse>
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
        public DateTime? CreatedSince { get; set; }

        /// <summary>
        ///     排序的字段。（可选值：IsBidirectional, CreatedDate, ModifiedDate 默认为 CreatedDate）
        /// </summary>
        [DataMember(Order = 3, Name = "orderby")]
        [ApiMember(Description = "排序的字段（可选值：CreatedDate 默认为 CreatedDate）")]
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
    ///     列举一组点赞信息的响应。
    /// </summary>
    [DataContract]
    public class LikeListResponse : IHasResponseStatus
    {
        /// <summary>
        ///     点赞信息列表。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "点赞信息列表")]
        public List<LikeDto> Likes { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}