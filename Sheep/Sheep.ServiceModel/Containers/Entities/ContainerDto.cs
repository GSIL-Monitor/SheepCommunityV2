using System;
using System.Runtime.Serialization;
using ServiceStack.Model;

namespace Sheep.ServiceModel.Containers.Entities
{
    /// <summary>
    ///     容器信息。
    /// </summary>
    [DataContract]
    public class ContainerDto : IHasStringId
    {
        /// <summary>
        ///     编号。
        /// </summary>
        [DataMember(Order = 1)]
        public string Id { get; set; }

        /// <summary>
        ///     类型。
        /// </summary>
        [DataMember(Order = 2)]
        public string Type { get; set; }

        /// <summary>
        ///     名称。
        /// </summary>
        [DataMember(Order = 3)]
        public string Name { get; set; }

        /// <summary>
        ///     图像网址。
        /// </summary>
        [DataMember(Order = 4)]
        public string AvatarUrl { get; set; }
    }
}