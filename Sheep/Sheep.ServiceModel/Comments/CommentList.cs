using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Comments.Entities;

namespace Sheep.ServiceModel.Comments
{
    /// <summary>
    ///     根据上级查询并列举一组评论的请求。
    /// </summary>
    [Route("/comments/query/byparent", HttpMethods.Get, Summary = "根据上级查询并列举一组评论信息")]
    [DataContract]
    public class CommentListByParent : IReturn<CommentListResponse>
    {
        /// <summary>
        ///     上级编号。（如帖子编号）
        /// </summary>
        [DataMember(Order = 1, Name = "parentid", IsRequired = true)]
        [ApiMember(Description = "上级编号（如帖子编号）")]
        public string ParentId { get; set; }

        /// <summary>
        ///     是否为我的。
        /// </summary>
        [DataMember(Order = 2, Name = "ismine")]
        [ApiMember(Description = "是否为我的")]
        public bool? IsMine { get; set; }

        /// <summary>
        ///     创建日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 3, Name = "createdsince")]
        [ApiMember(Description = "创建日期在指定的时间之后")]
        public long? CreatedSince { get; set; }

        /// <summary>
        ///     修改日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 4, Name = "modifiedsince")]
        [ApiMember(Description = "修改日期在指定的时间之后")]
        public long? ModifiedSince { get; set; }

        /// <summary>
        ///     是否标记为精选。
        /// </summary>
        [DataMember(Order = 5, Name = "isfeatured")]
        [ApiMember(Description = "是否标记为精选")]
        public bool? IsFeatured { get; set; }

        /// <summary>
        ///     排序的字段。（可选值：CreatedDate, ModifiedDate, RepliesCount, VotesCount, YesVotesCount, NoVotesCount,
        ///     ContentQuality 默认为 CreatedDate）
        /// </summary>
        [DataMember(Order = 6, Name = "orderby")]
        [ApiMember(Description = "排序的字段（可选值：CreatedDate, ModifiedDate, RepliesCount, VotesCount, YesVotesCount, NoVotesCount, ContentQuality 默认为 CreatedDate）")]
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
        [DataMember(Order = 8, Name = "skip")]
        [ApiMember(Description = "忽略的行数")]
        public int? Skip { get; set; }

        /// <summary>
        ///     获取的行数。
        /// </summary>
        [DataMember(Order = 9, Name = "limit")]
        [ApiMember(Description = "获取的行数")]
        public int? Limit { get; set; }
    }

    /// <summary>
    ///     根据用户查询并列举一组评论的请求。
    /// </summary>
    [Route("/comments/query/byuser", HttpMethods.Get, Summary = "根据用户查询并列举一组评论信息")]
    [DataContract]
    public class CommentListByUser : IReturn<CommentListResponse>
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
        ///     修改日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 4, Name = "modifiedsince")]
        [ApiMember(Description = "修改日期在指定的时间之后")]
        public long? ModifiedSince { get; set; }

        /// <summary>
        ///     是否标记为精选。
        /// </summary>
        [DataMember(Order = 5, Name = "isfeatured")]
        [ApiMember(Description = "是否标记为精选")]
        public bool? IsFeatured { get; set; }

        /// <summary>
        ///     排序的字段。（可选值：CreatedDate, ModifiedDate, RepliesCount, VotesCount, YesVotesCount, NoVotesCount,
        ///     ContentQuality 默认为 CreatedDate）
        /// </summary>
        [DataMember(Order = 6, Name = "orderby")]
        [ApiMember(Description = "排序的字段（可选值：CreatedDate, ModifiedDate, RepliesCount, VotesCount, YesVotesCount, NoVotesCount, ContentQuality 默认为 CreatedDate）")]
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
        [DataMember(Order = 8, Name = "skip")]
        [ApiMember(Description = "忽略的行数")]
        public int? Skip { get; set; }

        /// <summary>
        ///     获取的行数。
        /// </summary>
        [DataMember(Order = 9, Name = "limit")]
        [ApiMember(Description = "获取的行数")]
        public int? Limit { get; set; }
    }

    /// <summary>
    ///     根据作者帖子列表查询并列举一组评论信息的请求。
    /// </summary>
    [Route("/comments/query/bypostsofauthor", HttpMethods.Get, Summary = "根据作者帖子列表查询并列举一组评论信息")]
    [DataContract]
    public class CommentListByPostsOfAuthor : IReturn<CommentListResponse>
    {
        /// <summary>
        ///     是否仅列举已关注的用户的评论。
        /// </summary>
        [DataMember(Order = 1, Name = "following")]
        [ApiMember(Description = "是否仅列举已关注的用户的评论")]
        public bool? Following { get; set; }

        /// <summary>
        ///     忽略的行数。
        /// </summary>
        [DataMember(Order = 2, Name = "skip")]
        [ApiMember(Description = "忽略的行数")]
        public int? Skip { get; set; }

        /// <summary>
        ///     获取的行数。
        /// </summary>
        [DataMember(Order = 3, Name = "limit")]
        [ApiMember(Description = "获取的行数")]
        public int? Limit { get; set; }
    }

    /// <summary>
    ///     查询并列举一组评论的响应。
    /// </summary>
    [DataContract]
    public class CommentListResponse : IHasResponseStatus
    {
        /// <summary>
        ///     评论信息列表。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "评论信息列表")]
        public List<CommentDto> Comments { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}