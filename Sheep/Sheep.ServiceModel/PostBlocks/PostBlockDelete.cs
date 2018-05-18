using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.PostBlocks
{
    /// <summary>
    ///     取消一个帖子屏蔽的请求。
    /// </summary>
    [Route("/postblocks", HttpMethods.Delete, Summary = "取消一个帖子屏蔽")]
    [DataContract]
    public class PostBlockDelete : IReturn<PostBlockDeleteResponse>
    {
        /// <summary>
        ///     帖子编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "帖子编号")]
        public string PostId { get; set; }
    }

    /// <summary>
    ///     取消一个帖子屏蔽的响应。
    /// </summary>
    [DataContract]
    public class PostBlockDeleteResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}