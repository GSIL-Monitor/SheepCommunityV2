using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.PostBlocks.Entities;

namespace Sheep.ServiceModel.PostBlocks
{
    /// <summary>
    ///     新建一个帖子屏蔽的请求。
    /// </summary>
    [Route("/postblocks", HttpMethods.Post, Summary = "新建一个帖子屏蔽")]
    [DataContract]
    public class PostBlockCreate : IReturn<PostBlockCreateResponse>
    {
        /// <summary>
        ///     帖子编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "帖子编号")]
        public string PostId { get; set; }

        /// <summary>
        ///     原因。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "原因")]
        public string Reason { get; set; }
    }

    /// <summary>
    ///     新建一个帖子屏蔽的响应。
    /// </summary>
    [DataContract]
    public class PostBlockCreateResponse : IHasResponseStatus
    {
        /// <summary>
        ///     帖子屏蔽信息。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "帖子屏蔽信息")]
        public PostBlockDto PostBlock { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}