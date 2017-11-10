using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Groups.Entities;

namespace Sheep.ServiceModel.Groups
{
    /// <summary>
    ///     更新群组的请求。
    /// </summary>
    [Route("/groups/{GroupId}", HttpMethods.Put)]
    [DataContract]
    public class GroupUpdate : IReturn<GroupUpdateResponse>
    {
        /// <summary>
        ///     群组的编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public string GroupId { get; set; }

        /// <summary>
        ///     显示名称。
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        public string DisplayName { get; set; }

        /// <summary>
        ///     简介。
        /// </summary>
        [DataMember(Order = 3)]
        public string Description { get; set; }

        /// <summary>
        ///     所在国家。
        /// </summary>
        [DataMember(Order = 4)]
        public string Country { get; set; }

        /// <summary>
        ///     所在省份/州。
        /// </summary>
        [DataMember(Order = 5)]
        public string State { get; set; }

        /// <summary>
        ///     所在城市。
        /// </summary>
        [DataMember(Order = 6)]
        public string City { get; set; }

        /// <summary>
        ///     群组加入的方式。（可选值：Direct, RequireVerification, Joinless）
        /// </summary>
        [DataMember(Order = 7)]
        public string JoinMode { get; set; }

        /// <summary>
        ///     非群组成员是否可以访问群组内容。
        /// </summary>
        [DataMember(Order = 8)]
        public bool? IsPublic { get; set; }

        /// <summary>
        ///     是否开启群组消息。
        /// </summary>
        [DataMember(Order = 9)]
        public bool? EnableMessages { get; set; }
    }

    /// <summary>
    ///     更新群组的响应。
    /// </summary>
    [DataContract]
    public class GroupUpdateResponse : IHasResponseStatus
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