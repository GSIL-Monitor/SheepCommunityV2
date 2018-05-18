using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.PostBlocks.Entities;

namespace Sheep.ServiceModel.PostBlocks
{
    /// <summary>
    ///     查询并列举一组帖子屏蔽的请求。
    /// </summary>
    [Route("/postblocks/query", HttpMethods.Get, Summary = "查询并列举一组帖子屏蔽信息")]
    [DataContract]
    public class PostBlockList : IReturn<PostBlockListResponse>
    {
        /// <summary>
        ///     创建日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 1, Name = "createdsince")]
        [ApiMember(Description = "创建日期在指定的时间之后")]
        public long? CreatedSince { get; set; }

        /// <summary>
        ///     修改日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 2, Name = "modifiedsince")]
        [ApiMember(Description = "修改日期在指定的时间之后")]
        public long? ModifiedSince { get; set; }

        /// <summary>
        ///     排序的字段。（可选值：CreatedDate, ModifiedDate 默认为 CreatedDate）
        /// </summary>
        [DataMember(Order = 3, Name = "orderby")]
        [ApiMember(Description = "排序的字段（可选值：CreatedDate, ModifiedDate 默认为 CreatedDate）")]
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
    ///     根据上级查询并列举一组帖子屏蔽的请求。
    /// </summary>
    [Route("/postblocks/query/bypost", HttpMethods.Get, Summary = "根据上级查询并列举一组帖子屏蔽信息")]
    [DataContract]
    public class PostBlockListByPost : IReturn<PostBlockListResponse>
    {
        /// <summary>
        ///     帖子编号。
        /// </summary>
        [DataMember(Order = 1, Name = "postid", IsRequired = true)]
        [ApiMember(Description = "帖子编号")]
        public string PostId { get; set; }

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
        ///     排序的字段。（可选值：CreatedDate, ModifiedDate 默认为 CreatedDate）
        /// </summary>
        [DataMember(Order = 5, Name = "orderby")]
        [ApiMember(Description = "排序的字段（可选值：CreatedDate, ModifiedDate 默认为 CreatedDate）")]
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
    ///     根据屏蔽者查询并列举一组帖子屏蔽的请求。
    /// </summary>
    [Route("/postblocks/query/byblocker", HttpMethods.Get, Summary = "根据屏蔽者查询并列举一组帖子屏蔽信息")]
    [DataContract]
    public class PostBlockListByBlocker : IReturn<PostBlockListResponse>
    {
        /// <summary>
        ///     屏蔽者编号。
        /// </summary>
        [DataMember(Order = 1, Name = "blockerid", IsRequired = true)]
        [ApiMember(Description = "屏蔽者编号")]
        public int BlockerId { get; set; }

        /// <summary>
        ///     创建日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 2, Name = "createdsince")]
        [ApiMember(Description = "创建日期在指定的时间之后")]
        public long? CreatedSince { get; set; }

        /// <summary>
        ///     修改日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 3, Name = "modifiedsince")]
        [ApiMember(Description = "修改日期在指定的时间之后")]
        public long? ModifiedSince { get; set; }

        /// <summary>
        ///     排序的字段。（可选值：CreatedDate, ModifiedDate 默认为 CreatedDate）
        /// </summary>
        [DataMember(Order = 4, Name = "orderby")]
        [ApiMember(Description = "排序的字段（可选值：CreatedDate, ModifiedDate 默认为 CreatedDate）")]
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
    ///     查询并列举一组帖子屏蔽的响应。
    /// </summary>
    [DataContract]
    public class PostBlockListResponse : IHasResponseStatus
    {
        /// <summary>
        ///     帖子屏蔽信息列表。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "帖子屏蔽信息列表")]
        public List<PostBlockDto> PostBlocks { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}