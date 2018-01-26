using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.ChapterReads.Entities;

namespace Sheep.ServiceModel.ChapterReads
{
    /// <summary>
    ///     新建一个阅读的请求。
    /// </summary>
    [Route("/chapterreads", HttpMethods.Post, Summary = "新建一个阅读")]
    [DataContract]
    public class ChapterReadCreate : IReturn<ChapterReadCreateResponse>
    {
        /// <summary>
        ///     章编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "章编号")]
        public string ChapterId { get; set; }
    }

    /// <summary>
    ///     新建一个阅读的响应。
    /// </summary>
    [DataContract]
    public class ChapterReadCreateResponse : IHasResponseStatus
    {
        /// <summary>
        ///     阅读信息。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "阅读信息")]
        public ChapterReadDto ChapterRead { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}