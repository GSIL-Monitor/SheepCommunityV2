using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Replies
{
    /// <summary>
    ///     删除一个回复的请求。
    /// </summary>
    [Route("/replies/{ReplyId}", HttpMethods.Delete, Summary = "删除一个回复")]
    [DataContract]
    public class ReplyDelete : IReturn<ReplyDeleteResponse>
    {
        /// <summary>
        ///     回复编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "回复编号")]
        public string ReplyId { get; set; }
    }

    /// <summary>
    ///     删除一个回复的响应。
    /// </summary>
    [DataContract]
    public class ReplyDeleteResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}