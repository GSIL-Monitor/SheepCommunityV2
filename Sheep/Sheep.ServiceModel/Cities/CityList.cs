using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Cities.Entities;

namespace Sheep.ServiceModel.Cities
{
    /// <summary>
    ///     列举一组城市的请求。
    /// </summary>
    [Route("/cities/query", HttpMethods.Get)]
    [DataContract]
    public class CityList : IReturn<CityListResponse>
    {
        /// <summary>
        ///     省份编号。
        /// </summary>
        [DataMember(Order = 1, Name = "stateid", IsRequired = true)]
        public string StateId { get; set; }

        /// <summary>
        ///     过滤名称。（包括城市名称）
        /// </summary>
        [DataMember(Order = 2, Name = "namefilter")]
        public string NameFilter { get; set; }
    }

    /// <summary>
    ///     列举一组城市的响应。
    /// </summary>
    [DataContract]
    public class CityListResponse : IHasResponseStatus
    {
        /// <summary>
        ///     城市信息列表。
        /// </summary>
        [DataMember(Order = 1)]
        public List<CityDto> Cities { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}