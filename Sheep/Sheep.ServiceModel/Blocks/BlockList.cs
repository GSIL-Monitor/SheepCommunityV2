using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Blocks.Entities;

namespace Sheep.ServiceModel.Blocks
{
    /// <summary>
    ///     列举一组被屏蔽者的屏蔽信息的请求。
    /// </summary>
    [Route("/blocks/blockees", HttpMethods.Get, Summary = "列举一组被屏蔽者的屏蔽信息")]
    [DataContract]
    public class BlockListOfBlockee : IReturn<BlockListOfBlockeeResponse>
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
    ///     列举一组屏蔽者的屏蔽信息的请求。
    /// </summary>
    [Route("/blocks/blockers", HttpMethods.Get, Summary = "列举一组屏蔽者的屏蔽信息")]
    [DataContract]
    public class BlockListOfBlocker : IReturn<BlockListOfBlockerResponse>
    {
        /// <summary>
        ///     被屏蔽者编号。
        /// </summary>
        [DataMember(Order = 1, Name = "blockeeid", IsRequired = true)]
        [ApiMember(Description = "被屏蔽者编号")]
        public int BlockeeId { get; set; }

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
        ///     排序的字段。（可选值：IsBidirectional, CreatedDate, ModifiedDate 默认为 CreatedDate）
        /// </summary>
        [DataMember(Order = 4, Name = "orderby")]
        [ApiMember(Description = "排序的字段（可选值：IsBidirectional, CreatedDate, ModifiedDate 默认为 CreatedDate）")]
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
    ///     列举一组被屏蔽者的屏蔽信息的响应。
    /// </summary>
    [DataContract]
    public class BlockListOfBlockeeResponse : IHasResponseStatus
    {
        /// <summary>
        ///     屏蔽信息列表。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "被屏蔽者的屏蔽信息列表")]
        public List<BlockOfBlockeeDto> Blocks { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }

    /// <summary>
    ///     列举一组屏蔽者的屏蔽信息的响应。
    /// </summary>
    [DataContract]
    public class BlockListOfBlockerResponse : IHasResponseStatus
    {
        /// <summary>
        ///     屏蔽信息列表。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "屏蔽者的屏蔽信息列表")]
        public List<BlockOfBlockerDto> Blocks { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}