using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Groups.Entities;

namespace Sheep.ServiceModel.Groups
{
    /// <summary>
    ///     显示一个群组的请求。
    /// </summary>
    [Route("/groups/{GroupId}", HttpMethods.Get, Summary = "显示一个群组")]
    [DataContract]
    public class GroupShow : IReturn<GroupShowResponse>
    {
        /// <summary>
        ///     群组编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "群组编号")]
        public string GroupId { get; set; }
    }

    /// <summary>
    ///     根据显示名称显示一个群组的请求。
    /// </summary>
    [Route("/groups/showname/{DisplayName}", HttpMethods.Get, Summary = "根据显示名称显示一个群组")]
    [DataContract]
    public class GroupShowByDisplayName : IReturn<GroupShowResponse>
    {
        /// <summary>
        ///     显示名称。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "显示名称")]
        public string DisplayName { get; set; }
    }

    /// <summary>
    ///     显示一个群组的响应。
    /// </summary>
    [DataContract]
    public class GroupShowResponse : IHasResponseStatus
    {
        /// <summary>
        ///     群组信息。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "群组信息")]
        public GroupDto Group { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}