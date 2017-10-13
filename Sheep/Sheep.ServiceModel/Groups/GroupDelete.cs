using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Groups
{
    /// <summary>
    ///     删除一个群组的请求。
    /// </summary>
    [Route("/groups/{GroupId}", HttpMethods.Delete)]
    [DataContract]
    public class GroupDelete : IReturn<GroupDeleteResponse>
    {
        /// <summary>
        ///     群组的编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public string GroupId { get; set; }

        /// <summary>
        ///     是否自动删除应用程序。
        /// </summary>
        [DataMember(Order = 2)]
        public bool? DeleteApplications { get; set; }
    }

    /// <summary>
    ///     删除一个群组的响应。
    /// </summary>
    [DataContract]
    public class GroupDeleteResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}