using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Subjects.Entities;

namespace Sheep.ServiceModel.Subjects
{
    /// <summary>
    ///     查询并列举一组主题的请求。
    /// </summary>
    [Route("/books/{BookId}/volumes/{VolumeNumber}/subjects/query", HttpMethods.Get, Summary = "查询并列举一组主题信息")]
    [DataContract]
    public class SubjectList : IReturn<SubjectListResponse>
    {
        /// <summary>
        ///     书籍编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "书籍编号")]
        public string BookId { get; set; }

        /// <summary>
        ///     卷序号。
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        [ApiMember(Description = "卷序号")]
        public int VolumeNumber { get; set; }

        /// <summary>
        ///     过滤标题。
        /// </summary>
        [DataMember(Order = 3, Name = "titlefilter")]
        [ApiMember(Description = "过滤标题")]
        public string TitleFilter { get; set; }

        /// <summary>
        ///     排序的字段。（可选值： Number 默认为 Number）
        /// </summary>
        [DataMember(Order = 4, Name = "orderby")]
        [ApiMember(Description = "排序的字段（可选值： Number 默认为 Number）")]
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
    ///     查询并列举一组主题的响应。
    /// </summary>
    [DataContract]
    public class SubjectListResponse : IHasResponseStatus
    {
        /// <summary>
        ///     主题信息列表。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "主题信息列表")]
        public List<SubjectDto> Subjects { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}