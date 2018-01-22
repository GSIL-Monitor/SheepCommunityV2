using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Comments.Entities;

namespace Sheep.ServiceModel.Comments
{
    /// <summary>
    ///     根据上级统计一组评论数量的请求。
    /// </summary>
    [Route("/comments/count/byparent", HttpMethods.Get, Summary = "根据上级统计一组评论数量")]
    [DataContract]
    public class CommentCountByParent : IReturn<CommentCountResponse>
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
    }

    /// <summary>
    ///     根据上级列表统计一组评论数量的请求。
    /// </summary>
    [Route("/comments/count/byparents", HttpMethods.Get, Summary = "根据上级列表统计一组评论数量")]
    [DataContract]
    public class CommentCountByParents : IReturn<CommentCountByParentsResponse>
    {
        /// <summary>
        ///     上级编号列表。
        /// </summary>
        [DataMember(Order = 1, Name = "parentids", IsRequired = true)]
        [ApiMember(Description = "上级编号列表")]
        public List<string> ParentIds { get; set; }

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
    }

    /// <summary>
    ///     统计一组评论数量的响应。
    /// </summary>
    [DataContract]
    public class CommentCountResponse : IHasResponseStatus
    {
        /// <summary>
        ///     评论数量。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "评论数量")]
        public CommentCountsDto Counts { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }

    /// <summary>
    ///     根据上级列表统计一组评论数量的响应。
    /// </summary>
    [DataContract]
    public class CommentCountByParentsResponse : IHasResponseStatus
    {
        /// <summary>
        ///     一组上级的评论数量。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "一组上级的评论数量")]
        public Dictionary<string, CommentCountsDto> ParentsCounts { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}