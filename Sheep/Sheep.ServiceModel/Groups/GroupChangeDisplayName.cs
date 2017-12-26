using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Groups
{
    /// <summary>
    ///     更改群组显示名称的请求。
    /// </summary>
    [Route("/groups/{GroupId}/displayname", HttpMethods.Put, Summary = "更改群组显示名称")]
    [DataContract]
    public class GroupChangeDisplayName : IReturn<GroupChangeDisplayNameResponse>
    {
        /// <summary>
        ///     群组编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "群组编号")]
        public string GroupId { get; set; }

        /// <summary>
        ///     更改的显示名称。
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        [ApiMember(Description = "更改的显示名称")]
        public string DisplayName { get; set; }
    }

    /// <summary>
    ///     更改群组显示名称的响应。
    /// </summary>
    [DataContract]
    public class GroupChangeDisplayNameResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}