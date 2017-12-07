using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Subjects.Entities;

namespace Sheep.ServiceModel.Subjects
{
    /// <summary>
    ///     显示一个主题的请求。
    /// </summary>
    [Route("/books/{BookId}/volumes/{VolumeNumber}/subjects/{SubjectNumber}", HttpMethods.Get, Summary = "显示一个主题")]
    [DataContract]
    public class SubjectShow : IReturn<SubjectShowResponse>
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
        ///     主题序号。
        /// </summary>
        [DataMember(Order = 3, IsRequired = true)]
        [ApiMember(Description = "主题序号")]
        public int SubjectNumber { get; set; }
    }

    /// <summary>
    ///     显示一个主题的响应。
    /// </summary>
    [DataContract]
    public class SubjectShowResponse : IHasResponseStatus
    {
        /// <summary>
        ///     主题信息。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "主题信息")]
        public SubjectDto Subject { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}