using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Volumes.Entities;

namespace Sheep.ServiceModel.Volumes
{
    /// <summary>
    ///     更新一个卷的请求。
    /// </summary>
    [Route("/books/{BookId}/volumes/{VolumeNumber}", HttpMethods.Put, Summary = "更新一个卷")]
    [DataContract]
    public class VolumeUpdate : IReturn<VolumeUpdateResponse>
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
        ///     标题。
        /// </summary>
        [DataMember(Order = 3, IsRequired = true)]
        [ApiMember(Description = "标题")]
        public string Title { get; set; }

        /// <summary>
        ///     缩写。
        /// </summary>
        [DataMember(Order = 4)]
        [ApiMember(Description = "缩写")]
        public string Abbreviation { get; set; }
    }

    /// <summary>
    ///     更新一个卷的响应。
    /// </summary>
    [DataContract]
    public class VolumeUpdateResponse : IHasResponseStatus
    {
        /// <summary>
        ///     卷信息。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "卷信息")]
        public VolumeDto Volume { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}