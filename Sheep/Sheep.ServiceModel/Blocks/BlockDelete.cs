using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Blocks
{
    /// <summary>
    ///     取消一个屏蔽的请求。
    /// </summary>
    [Route("/blocks", HttpMethods.Delete, Summary = "取消一个屏蔽")]
    [DataContract]
    public class BlockDelete : IReturn<BlockDeleteResponse>
    {
        /// <summary>
        ///     被屏蔽者编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "被屏蔽者编号")]
        public int BlockeeId { get; set; }
    }

    /// <summary>
    ///     取消一个屏蔽的响应。
    /// </summary>
    [DataContract]
    public class BlockDeleteResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}