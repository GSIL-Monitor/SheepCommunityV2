using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Votes.Entities;

namespace Sheep.ServiceModel.Votes
{
    /// <summary>
    ///     显示一个投票的请求。
    /// </summary>
    [Route("/votes/{ParentId}/{UserId}", HttpMethods.Get, Summary = "显示一个投票")]
    [DataContract]
    public class VoteShow : IReturn<VoteShowResponse>
    {
        /// <summary>
        ///     上级编号。（如评论编号）
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "上级编号（如评论编号）")]
        public string ParentId { get; set; }

        /// <summary>
        ///     用户编号。
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        [ApiMember(Description = "用户编号")]
        public int UserId { get; set; }
    }

    /// <summary>
    ///     显示一个投票的响应。
    /// </summary>
    [DataContract]
    public class VoteShowResponse : IHasResponseStatus
    {
        /// <summary>
        ///     投票信息。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "投票信息")]
        public VoteDto Vote { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}