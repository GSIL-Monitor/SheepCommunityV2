using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Votes.Entities;

namespace Sheep.ServiceModel.Votes
{
    /// <summary>
    ///     新建一个投票的请求。
    /// </summary>
    [Route("/votes", HttpMethods.Post, Summary = "新建一个投票")]
    [DataContract]
    public class VoteCreate : IReturn<VoteCreateResponse>
    {
        /// <summary>
        ///     上级类型。（可选值：评论, 回复）
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "上级类型（可选值：评论, 回复）")]
        public string ParentType { get; set; }

        /// <summary>
        ///     上级编号。（如评论编号）
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        [ApiMember(Description = "上级编号（如评论编号）")]
        public string ParentId { get; set; }

        /// <summary>
        ///     赞成或反对。
        /// </summary>
        [DataMember(Order = 3, IsRequired = true)]
        [ApiMember(Description = "赞成或反对")]
        public bool Value { get; set; }
    }

    /// <summary>
    ///     新建一个投票的响应。
    /// </summary>
    [DataContract]
    public class VoteCreateResponse : IHasResponseStatus
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