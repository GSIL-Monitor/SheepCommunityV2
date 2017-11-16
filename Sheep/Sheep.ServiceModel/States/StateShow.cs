using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.States.Entities;

namespace Sheep.ServiceModel.States
{
    /// <summary>
    ///     显示一个省份的请求。
    /// </summary>
    [Route("/states/{StateId}", HttpMethods.Get, Summary = "显示一个省份")]
    [DataContract]
    public class StateShow : IReturn<StateShowResponse>
    {
        /// <summary>
        ///     省份编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "省份编号")]
        public string StateId { get; set; }
    }

    /// <summary>
    ///     根据省份名称显示一个省份的请求。
    /// </summary>
    [Route("/states/show/{CountryId}/{Name}", HttpMethods.Get, Summary = "根据省份名称显示一个省份")]
    [DataContract]
    public class StateShowByName : IReturn<StateShowResponse>
    {
        /// <summary>
        ///     国家编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "国家编号")]
        public string CountryId { get; set; }

        /// <summary>
        ///     省份名称。
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        [ApiMember(Description = "省份名称")]
        public string Name { get; set; }
    }

    /// <summary>
    ///     显示一个省份的响应。
    /// </summary>
    [DataContract]
    public class StateShowResponse : IHasResponseStatus
    {
        /// <summary>
        ///     省份信息。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "省份信息")]
        public StateDto State { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}