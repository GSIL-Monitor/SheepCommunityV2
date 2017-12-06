using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Volumes.Entities;

namespace Sheep.ServiceModel.Volumes
{
    /// <summary>
    ///     查询并列举一组卷的请求。
    /// </summary>
    [Route("/books/{BookId}/volumes/query", HttpMethods.Get, Summary = "查询并列举一组卷信息")]
    [DataContract]
    public class VolumeList : IReturn<VolumeListResponse>
    {
        /// <summary>
        ///     书籍编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "书籍编号")]
        public string BookId { get; set; }

        /// <summary>
        ///     过滤标题。
        /// </summary>
        [DataMember(Order = 2, Name = "titlefilter")]
        [ApiMember(Description = "过滤标题")]
        public string TitleFilter { get; set; }

        /// <summary>
        ///     排序的字段。（可选值： Number, ChaptersCount, SubjectsCount 默认为 Number）
        /// </summary>
        [DataMember(Order = 3, Name = "orderby")]
        [ApiMember(Description = "排序的字段（可选值： Number, ChaptersCount, SubjectsCount 默认为 Number）")]
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
    ///     查询并列举一组卷的响应。
    /// </summary>
    [DataContract]
    public class VolumeListResponse : IHasResponseStatus
    {
        /// <summary>
        ///     卷信息列表。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "卷信息列表")]
        public List<VolumeDto> Volumes { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}