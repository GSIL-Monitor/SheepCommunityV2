using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Votes
{
    /// <summary>
    ///     取消一个投票的请求。
    /// </summary>
    [Route("/votes", HttpMethods.Delete, Summary = "取消一个投票")]
    [DataContract]
    public class VoteDelete : IReturn<VoteDeleteResponse>
    {
        /// <summary>
        ///     上级类型。（可选值：评论）
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "上级类型（可选值：评论）")]
        public string ParentType { get; set; }

        /// <summary>
        ///     上级编号。（如评论编号）
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        [ApiMember(Description = "上级编号（如评论编号）")]
        public string ParentId { get; set; }
    }

    /// <summary>
    ///     取消一个投票的响应。
    /// </summary>
    [DataContract]
    public class VoteDeleteResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}