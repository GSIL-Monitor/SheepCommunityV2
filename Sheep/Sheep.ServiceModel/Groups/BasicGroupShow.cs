using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Groups.Entities;

namespace Sheep.ServiceModel.Groups
{
    /// <summary>
    ///     显示一个群组基本信息的请求。
    /// </summary>
    [Route("/groups/basic/{GroupId}", HttpMethods.Get)]
    [DataContract]
    public class BasicGroupShow : IReturn<BasicGroupShowResponse>
    {
        /// <summary>
        ///     群组的编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public string GroupId { get; set; }
    }

    /// <summary>
    ///     根据关联的第三方编号显示一个群组基本信息的请求。
    /// </summary>
    [Route("/groups/basic/show/{RefId}", HttpMethods.Get)]
    [DataContract]
    public class BasicGroupShowByRefId : IReturn<BasicGroupShowResponse>
    {
        /// <summary>
        ///     关联的第三方编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public string RefId { get; set; }
    }

    /// <summary>
    ///     显示一个群组基本信息的响应。
    /// </summary>
    [DataContract]
    public class BasicGroupShowResponse : IHasResponseStatus
    {
        /// <summary>
        ///     群组基本信息。
        /// </summary>
        [DataMember(Order = 1)]
        public BasicGroupDto Group { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}