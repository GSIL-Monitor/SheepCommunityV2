using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Views
{
    /// <summary>
    ///     新建一组查看的请求。
    /// </summary>
    [Route("/views/batch", HttpMethods.Post, Summary = "新建一组查看")]
    [DataContract]
    public class ViewBatchCreate : IReturn<ViewBatchCreateResponse>
    {
        /// <summary>
        ///     上级类型。（可选值：帖子, 章, 节）
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "上级类型（可选值：帖子, 章, 节）")]
        public string ParentType { get; set; }

        /// <summary>
        ///     上级编号列表。（如帖子编号）
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        [ApiMember(Description = "上级编号列表（如帖子编号）")]
        public List<string> ParentIds { get; set; }
    }

    /// <summary>
    ///     新建一组查看的响应。
    /// </summary>
    [DataContract]
    public class ViewBatchCreateResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}