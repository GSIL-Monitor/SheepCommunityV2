using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Volumes.Entities;

namespace Sheep.ServiceModel.Volumes
{
    /// <summary>
    ///     新建一条卷注释的请求。
    /// </summary>
    [Route("/books/{BookId}/volumes/{VolumeNumber}/annotations", HttpMethods.Post, Summary = "新建一条卷注释")]
    [DataContract]
    public class VolumeAnnotationCreate : IReturn<VolumeAnnotationCreateResponse>
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
        ///     注释序号。
        /// </summary>
        [DataMember(Order = 3, IsRequired = true)]
        [ApiMember(Description = "注释序号")]
        public int AnnotationNumber { get; set; }

        /// <summary>
        ///     标题。
        /// </summary>
        [DataMember(Order = 4)]
        [ApiMember(Description = "标题")]
        public string Title { get; set; }

        /// <summary>
        ///     注释。
        /// </summary>
        [DataMember(Order = 5, IsRequired = true)]
        [ApiMember(Description = "注释")]
        public string Annotation { get; set; }
    }

    /// <summary>
    ///     新建一条卷注释的响应。
    /// </summary>
    [DataContract]
    public class VolumeAnnotationCreateResponse : IHasResponseStatus
    {
        /// <summary>
        ///     卷注释信息。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "卷注释信息")]
        public VolumeAnnotationDto VolumeAnnotation { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}