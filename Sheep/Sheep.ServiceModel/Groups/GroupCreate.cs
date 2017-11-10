using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Groups.Entities;

namespace Sheep.ServiceModel.Groups
{
    /// <summary>
    ///     新建群组的请求。
    /// </summary>
    [Route("/groups", HttpMethods.Post)]
    [DataContract]
    public class GroupCreate : IReturn<GroupCreateResponse>
    {
        /// <summary>
        ///     显示名称。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public string DisplayName { get; set; }

        /// <summary>
        ///     简介。
        /// </summary>
        [DataMember(Order = 2)]
        public string Description { get; set; }

        /// <summary>
        ///     关联的第三方编号。
        /// </summary>
        [DataMember(Order = 3)]
        public string RefId { get; set; }

        /// <summary>
        ///     群组加入的方式。（可选值：Direct, RequireVerification, Joinless）
        /// </summary>
        [DataMember(Order = 4)]
        public string JoinMode { get; set; }

        /// <summary>
        ///     非群组成员是否可以访问群组内容。
        /// </summary>
        [DataMember(Order = 5)]
        public bool? IsPublic { get; set; }

        /// <summary>
        ///     是否开启群组消息。
        /// </summary>
        [DataMember(Order = 6)]
        public bool? EnableMessages { get; set; }
    }

    /// <summary>
    ///     新建群组的响应。
    /// </summary>
    [DataContract]
    public class GroupCreateResponse : IHasResponseStatus
    {
        /// <summary>
        ///     群组信息。
        /// </summary>
        [DataMember(Order = 1)]
        public GroupDto Group { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}