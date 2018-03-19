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
    }
}