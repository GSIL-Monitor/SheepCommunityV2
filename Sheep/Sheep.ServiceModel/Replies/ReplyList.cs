using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Replies.Entities;

namespace Sheep.ServiceModel.Replies
{
    /// <summary>
    ///     根据上级查询并列举一组回复的请求。
    /// </summary>
    [Route("/replies/query/byparent", HttpMethods.Get, Summary = "根据上级查询并列举一组回复信息")]
    [DataContract]
    public class ReplyListByParent : IReturn<ReplyListResponse>
    {
        /// <summary>
        ///     上级编号。（如评论编号）
        /// </summary>
        [DataMember(Order = 1, Name = "parentid", IsRequired = true)]
        [ApiMember(Description = "上级编号（如评论编号）")]
        public string ParentId { get; set; }

        /// <summary>
        ///     创建日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 2, Name = "createdsince")]
        [ApiMember(Description = "创建日期在指定的时间之后")]
        public DateTime? CreatedSince { get; set; }

        /// <summary>
        ///     修改日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 3, Name = "modifiedsince")]
        [ApiMember(Description = "修改日期在指定的时间之后")]
        public DateTime? ModifiedSince { get; set; }

        /// <summary>
        ///     排序的字段。（可选值：CreatedDate, ModifiedDate, VotesCount, YesVotesCount, NoVotesCount,
        ///     ContentQuality 默认为 CreatedDate）
        /// </summary>
        [DataMember(Order = 4, Name = "orderby")]
        [ApiMember(Description = "排序的字段（可选值：CreatedDate, ModifiedDate, VotesCount, YesVotesCount, NoVotesCount, ContentQuality 默认为 CreatedDate）")]
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
    ///     根据用户查询并列举一组回复的请求。
    /// </summary>
    [Route("/replies/query/byuser", HttpMethods.Get, Summary = "根据用户查询并列举一组回复信息")]
    [DataContract]
    public class ReplyListByUser : IReturn<ReplyListResponse>
    {
        /// <summary>
        ///     用户编号。
        /// </summary>
        [DataMember(Order = 1, Name = "userid", IsRequired = true)]
        [ApiMember(Description = "用户编号")]
        public int UserId { get; set; }

        /// <summary>
        ///     上级类型。（可选值：评论）
        /// </summary>
        [DataMember(Order = 2, Name = "parenttype")]
        [ApiMember(Description = "上级类型（可选值：评论）")]
        public string ParentType { get; set; }

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
        [ApiMember(Description = "修改日期在指定的时间之后")]
        public DateTime? ModifiedSince { get; set; }

        /// <summary>
        ///     排序的字段。（可选值：CreatedDate, ModifiedDate, VotesCount, YesVotesCount, NoVotesCount,
        ///     ContentQuality 默认为 CreatedDate）
        /// </summary>
        [DataMember(Order = 5, Name = "orderby")]
        [ApiMember(Description = "排序的字段（可选值：CreatedDate, ModifiedDate, VotesCount, YesVotesCount, NoVotesCount, ContentQuality 默认为 CreatedDate）")]
        public string OrderBy { get; set; }

        /// <summary>
        ///     是否按降序排序。
        /// </summary>
        [DataMember(Order = 6, Name = "descending")]
        [ApiMember(Description = "是否按降序排序")]
        public bool? Descending { get; set; }

        /// <summary>
        ///     忽略的行数。
        /// </summary>
        [DataMember(Order = 7, Name = "skip")]
        [ApiMember(Description = "忽略的行数")]
        public int? Skip { get; set; }

        /// <summary>
        ///     获取的行数。
        /// </summary>
        [DataMember(Order = 8, Name = "limit")]
        [ApiMember(Description = "获取的行数")]
        public int? Limit { get; set; }
    }

    /// <summary>
    ///     查询并列举一组回复的响应。
    /// </summary>
    [DataContract]
    public class ReplyListResponse : IHasResponseStatus
    {
        /// <summary>
        ///     回复信息列表。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "回复信息列表")]
        public List<ReplyDto> Replies { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}