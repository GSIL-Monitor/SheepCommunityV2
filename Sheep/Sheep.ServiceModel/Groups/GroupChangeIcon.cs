using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Groups
{
    /// <summary>
    ///     更改群组图标的请求。
    /// </summary>
    [Route("/groups/{GroupId}/icon", HttpMethods.Put, Summary = "更改群组图标", Notes = "上传本地图标无须输入来源图标的地址。")]
    [DataContract]
    public class GroupChangeIcon : IReturn<GroupChangeIconResponse>
    {
        /// <summary>
        ///     群组编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "群组编号")]
        public string GroupId { get; set; }

        /// <summary>
        ///     来源图标的地址。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "来源图标的地址")]
        public string SourceIconUrl { get; set; }
    }

    /// <summary>
    ///     更改群组图标的响应。
    /// </summary>
    [DataContract]
    public class GroupChangeIconResponse : IHasResponseStatus
    {
        /// <summary>
        ///     图标的地址。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "图标的地址")]
        public string IconUrl { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}