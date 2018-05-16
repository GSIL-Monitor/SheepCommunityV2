using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Blocks.Entities;

namespace Sheep.ServiceModel.Blocks
{
    /// <summary>
    ///     新建一个屏蔽的请求。
    /// </summary>
    [Route("/blocks", HttpMethods.Post, Summary = "新建一个屏蔽")]
    [DataContract]
    public class BlockCreate : IReturn<BlockCreateResponse>
    {
        /// <summary>
        ///     被屏蔽者编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "被屏蔽者编号")]
        public int BlockeeId { get; set; }
    }

    /// <summary>
    ///     新建一个屏蔽的响应。
    /// </summary>
    [DataContract]
    public class BlockCreateResponse : IHasResponseStatus
    {
        /// <summary>
        ///     屏蔽信息。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "屏蔽信息")]
        public BlockDto Block { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}