using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Views
{
    /// <summary>
    ///     删除一个阅读的请求。
    /// </summary>
    [Route("/views/{ViewId}", HttpMethods.Delete, Summary = "删除一个阅读")]
    [DataContract]
    public class ViewDelete : IReturn<ViewDeleteResponse>
    {
        /// <summary>
        ///     阅读编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "阅读编号")]
        public string ViewId { get; set; }
    }

    /// <summary>
    ///     删除一个阅读的响应。
    /// </summary>
    [DataContract]
    public class ViewDeleteResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}