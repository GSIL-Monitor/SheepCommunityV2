﻿using System;
using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.Job.ServiceModel.Replies
{
    /// <summary>
    ///     查询并计算一组回复分数的请求。
    /// </summary>
    [Route("/replies/calculate", HttpMethods.Put, Summary = "查询并计算一组回复分数信息")]
    [DataContract]
    public class ReplyCalculate : IReturn<ReplyCalculateResponse>
    {
        /// <summary>
        ///     上级类型。（可选值：评论）
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "上级类型（可选值：评论）")]
        public string ParentType { get; set; }

        /// <summary>
        ///     创建日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "创建日期在指定的时间之后")]
        public long? CreatedSince { get; set; }

        /// <summary>
        ///     修改日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 3)]
        [ApiMember(Description = "修改日期在指定的时间之后")]
        public long? ModifiedSince { get; set; }

        /// <summary>
        ///     排序的字段。（可选值：CreatedDate, ModifiedDate, VotesCount, YesVotesCount, NoVotesCount, ContentQuality 默认为 CreatedDate）
        /// </summary>
        [DataMember(Order = 4)]
        [ApiMember(Description = "排序的字段（可选值：CreatedDate, ModifiedDate, VotesCount, YesVotesCount, NoVotesCount, ContentQuality 默认为 CreatedDate）")]
        public string OrderBy { get; set; }

        /// <summary>
        ///     是否按降序排序。
        /// </summary>
        [DataMember(Order = 5)]
        [ApiMember(Description = "是否按降序排序")]
        public bool? Descending { get; set; }

        /// <summary>
        ///     忽略的行数。
        /// </summary>
        [DataMember(Order = 6)]
        [ApiMember(Description = "忽略的行数")]
        public int? Skip { get; set; }

        /// <summary>
        ///     获取的行数。
        /// </summary>
        [DataMember(Order = 7)]
        [ApiMember(Description = "获取的行数")]
        public int? Limit { get; set; }
    }

    /// <summary>
    ///     查询并计算一组回复分数的响应。
    /// </summary>
    [DataContract]
    public class ReplyCalculateResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}