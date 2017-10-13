using System;
using System.Runtime.Serialization;
using ServiceStack.Model;
using Sheep.ServiceModel.Containers.Entities;

namespace Sheep.ServiceModel.Applications.Entities
{
    /// <summary>
    ///     应用程序信息。
    /// </summary>
    [DataContract]
    public class ApplicationDto : IHasStringId
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
        ///     所属的容器。
        /// </summary>
        [DataMember(Order = 3)]
        public ContainerDto Container { get; set; }

        /// <summary>
        ///     名称。
        /// </summary>
        [DataMember(Order = 4)]
        public string Name { get; set; }

        /// <summary>
        ///     说明。
        /// </summary>
        [DataMember(Order = 5)]
        public string Description { get; set; }

        /// <summary>
        ///     图像网址。
        /// </summary>
        [DataMember(Order = 6)]
        public string AvatarUrl { get; set; }
    }
}