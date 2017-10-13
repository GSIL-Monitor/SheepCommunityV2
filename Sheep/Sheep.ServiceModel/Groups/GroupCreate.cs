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
        ///     群组的类型。（可选值：Joinless, PublicOpen, PublicClosed, PrivateUnlisted, PrivateListed）
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public string GroupType { get; set; }

        /// <summary>
        ///     名称。
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        public string Name { get; set; }

        /// <summary>
        ///     上级群组的编号。
        /// </summary>
        [DataMember(Order = 3)]
        public string ParentGroupId { get; set; }

        /// <summary>
        ///     代号。（如果没有指定将自动生成）
        /// </summary>
        [DataMember(Order = 4)]
        public string Code { get; set; }

        /// <summary>
        ///     说明。
        /// </summary>
        [DataMember(Order = 5)]
        public string Description { get; set; }

        /// <summary>
        ///     是否开启群组消息。
        /// </summary>
        [DataMember(Order = 6)]
        public bool? EnableGroupMessages { get; set; }

        /// <summary>
        ///     是否自动创建应用程序。
        /// </summary>
        [DataMember(Order = 7)]
        public bool? CreateApplications { get; set; }
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