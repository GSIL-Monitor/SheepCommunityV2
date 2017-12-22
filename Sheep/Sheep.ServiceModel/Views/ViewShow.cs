using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Views.Entities;

namespace Sheep.ServiceModel.Views
{
    /// <summary>
    ///     显示一个阅读的请求。
    /// </summary>
    [Route("/views/{ViewId}", HttpMethods.Get, Summary = "显示一个阅读")]
    [DataContract]
    public class ViewShow : IReturn<ViewShowResponse>
    {
        /// <summary>
        ///     阅读的编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "阅读编号")]
        public string ViewId { get; set; }
    }

    /// <summary>
    ///     显示一个阅读的响应。
    /// </summary>
    [DataContract]
    public class ViewShowResponse : IHasResponseStatus
    {
        /// <summary>
        ///     阅读信息。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "阅读信息")]
        public ViewDto View { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}