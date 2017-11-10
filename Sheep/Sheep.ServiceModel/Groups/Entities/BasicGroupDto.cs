using System.Runtime.Serialization;
using ServiceStack.Model;

namespace Sheep.ServiceModel.Groups.Entities
{
    /// <summary>
    ///     基本群组信息。
    /// </summary>
    [DataContract]
    public class BasicGroupDto : IHasStringId
    {
        /// <summary>
        ///     编号。
        /// </summary>
        [DataMember(Order = 1)]
        public string Id { get; set; }

        /// <summary>
        ///     显示名称。
        /// </summary>
        [DataMember(Order = 2)]
        public string DisplayName { get; set; }

        /// <summary>
        ///     图像网址。
        /// </summary>
        [DataMember(Order = 3)]
        public string IconUrl { get; set; }

        /// <summary>
        ///     关联的第三方编号。
        /// </summary>
        [DataMember(Order = 4)]
        public string RefId { get; set; }

        /// <summary>
        ///     加入群组的方式。（可选值：Direct, RequireVerification, Joinless）
        /// </summary>
        [DataMember(Order = 5)]
        public string JoinMode { get; set; }
    }
}