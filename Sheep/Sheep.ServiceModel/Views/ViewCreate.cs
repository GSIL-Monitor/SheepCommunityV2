using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Views.Entities;

namespace Sheep.ServiceModel.Views
{
    /// <summary>
    ///     新建一个查看的请求。
    /// </summary>
    [Route("/views", HttpMethods.Post, Summary = "新建一个查看")]
    [DataContract]
    public class ViewCreate : IReturn<ViewCreateResponse>
    {
        /// <summary>
        ///     上级类型。（可选值：帖子, 章, 节）
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "上级类型（可选值：帖子, 章, 节）")]
        public string ParentType { get; set; }

        /// <summary>
        ///     上级编号。（如帖子编号）
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        [ApiMember(Description = "上级编号（如帖子编号）")]
        public string ParentId { get; set; }
    }

    /// <summary>
    ///     新建一个查看的响应。
    /// </summary>
    [DataContract]
    public class ViewCreateResponse : IHasResponseStatus
    {
        /// <summary>
        ///     查看信息。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "查看信息")]
        public ViewDto View { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}