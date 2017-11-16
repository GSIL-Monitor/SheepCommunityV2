using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Cities.Entities;

namespace Sheep.ServiceModel.Cities
{
    /// <summary>
    ///     显示一个城市的请求。
    /// </summary>
    [Route("/cities/{CityId}", HttpMethods.Get, Summary = "显示一个城市")]
    [DataContract]
    public class CityShow : IReturn<CityShowResponse>
    {
        /// <summary>
        ///     城市编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "城市编号")]
        public string CityId { get; set; }
    }

    /// <summary>
    ///     根据城市名称显示一个城市的请求。
    /// </summary>
    [Route("/cities/show/{StateId}/{Name}", HttpMethods.Get, Summary = "根据城市名称显示一个城市")]
    [DataContract]
    public class CityShowByName : IReturn<CityShowResponse>
    {
        /// <summary>
        ///     省份编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "省份编号")]
        public string StateId { get; set; }

        /// <summary>
        ///     城市名称。
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        [ApiMember(Description = "城市名称")]
        public string Name { get; set; }
    }

    /// <summary>
    ///     显示一个城市的响应。
    /// </summary>
    [DataContract]
    public class CityShowResponse : IHasResponseStatus
    {
        /// <summary>
        ///     城市信息。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "城市信息")]
        public CityDto City { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}