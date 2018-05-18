using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.PostBlocks.Entities;

namespace Sheep.ServiceModel.PostBlocks
{
    /// <summary>
    ///     显示一个帖子屏蔽的请求。
    /// </summary>
    [Route("/postblocks/{PostId}/{BlockerId}", HttpMethods.Get, Summary = "显示一个帖子屏蔽")]
    [DataContract]
    public class PostBlockShow : IReturn<PostBlockShowResponse>
    {
        /// <summary>
        ///     帖子编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "帖子编号")]
        public string PostId { get; set; }

        /// <summary>
        ///     屏蔽者编号。
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        [ApiMember(Description = "屏蔽者编号")]
        public int BlockerId { get; set; }
    }

    /// <summary>
    ///     显示一个帖子屏蔽的响应。
    /// </summary>
    [DataContract]
    public class PostBlockShowResponse : IHasResponseStatus
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