using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Volumes.Entities;

namespace Sheep.ServiceModel.Volumes
{
    /// <summary>
    ///     查询并列举一组卷注释的请求。
    /// </summary>
    [Route("/books/{BookId}/volumes/{VolumeNumber}/annotations/query", HttpMethods.Get, Summary = "查询并列举一组卷注释信息")]
    [DataContract]
    public class VolumeAnnotationList : IReturn<VolumeAnnotationListResponse>
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
        ///     过滤注释。
        /// </summary>
        [DataMember(Order = 3, Name = "annotationfilter")]
        [ApiMember(Description = "过滤注释")]
        public string AnnotationFilter { get; set; }

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
    ///     查询并列举一组卷注释的响应。
    /// </summary>
    [DataContract]
    public class VolumeAnnotationListResponse : IHasResponseStatus
    {
        /// <summary>
        ///     卷注释信息列表。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "卷注释信息列表")]
        public List<VolumeAnnotationDto> VolumeAnnotations { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}