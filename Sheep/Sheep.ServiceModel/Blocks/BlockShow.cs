using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Blocks.Entities;

namespace Sheep.ServiceModel.Blocks
{
    /// <summary>
    ///     显示一个屏蔽的请求。
    /// </summary>
    [Route("/blocks/{BlockeeId}/{BlockerId}", HttpMethods.Get, Summary = "显示一个屏蔽")]
    [DataContract]
    public class BlockShow : IReturn<BlockShowResponse>
    {
        /// <summary>
        ///     被屏蔽者编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "被屏蔽者编号")]
        public int BlockeeId { get; set; }

        /// <summary>
        ///     屏蔽者编号。
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        [ApiMember(Description = "屏蔽者编号")]
        public int BlockerId { get; set; }
    }

    /// <summary>
    ///     显示一个屏蔽的响应。
    /// </summary>
    [DataContract]
    public class BlockShowResponse : IHasResponseStatus
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