using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Groups.Entities;

namespace Sheep.ServiceModel.Groups
{
    /// <summary>
    ///     查询并列举一组群组排行的请求。
    /// </summary>
    [Route("/groups/ranks/query", HttpMethods.Get, Summary = "查询并列举一组群组排行")]
    [DataContract]
    public class GroupRankList : IReturn<GroupRankListResponse>
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
    ///     根据编号列表查询并列举一组群组排行的请求。
    /// </summary>
    [Route("/groups/ranks/query/byids", HttpMethods.Get, Summary = "根据编号列表查询并列举一组群组排行")]
    [DataContract]
    public class GroupRankListByIds : IReturn<GroupRankListResponse>
    {
        /// <summary>
        ///     群组编号列表。
        /// </summary>
        [DataMember(Order = 1, Name = "groupids", IsRequired = true)]
        [ApiMember(Description = "群组编号列表")]
        public List<string> GroupIds { get; set; }

        /// <summary>
        ///     排序的字段。（可选值：PostViewsCount, PostViewsRank, ParagraphViewsCount, ParagraphViewsRank 默认为 ParagraphViewsRank）
        /// </summary>
        [DataMember(Order = 2, Name = "orderby")]
        [ApiMember(Description = "排序的字段（可选值：PostViewsCount, PostViewsRank, ParagraphViewsCount, ParagraphViewsRank 默认为 ParagraphViewsRank）")]
        public string OrderBy { get; set; }

        /// <summary>
        ///     是否按降序排序。
        /// </summary>
        [DataMember(Order = 3, Name = "descending")]
        [ApiMember(Description = "是否按降序排序")]
        public bool? Descending { get; set; }

        /// <summary>
        ///     忽略的行数。
        /// </summary>
        [DataMember(Order = 4, Name = "skip")]
        [ApiMember(Description = "忽略的行数")]
        public int? Skip { get; set; }

        /// <summary>
        ///     获取的行数。
        /// </summary>
        [DataMember(Order = 5, Name = "limit")]
        [ApiMember(Description = "获取的行数")]
        public int? Limit { get; set; }
    }

    /// <summary>
    ///     查询并列举一组群组排行的响应。
    /// </summary>
    [DataContract]
    public class GroupRankListResponse : IHasResponseStatus
    {
        /// <summary>
        ///     群组排行信息列表。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "群组排行信息列表")]
        public List<GroupRankDto> GroupRanks { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}