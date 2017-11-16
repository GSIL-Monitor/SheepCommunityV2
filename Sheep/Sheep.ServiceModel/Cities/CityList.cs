using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Cities.Entities;

namespace Sheep.ServiceModel.Cities
{
    /// <summary>
    ///     查询并列举一组城市的请求。
    /// </summary>
    [Route("/cities/query", HttpMethods.Get, Summary = "查询并列举一组城市")]
    [DataContract]
    public class CityList : IReturn<CityListResponse>
    {
        /// <summary>
        ///     省份编号。
        /// </summary>
        [DataMember(Order = 1, Name = "stateid", IsRequired = true)]
        [ApiMember(Description = "省份编号")]
        public string StateId { get; set; }

        /// <summary>
        ///     名称过滤。（包括城市名称）
        /// </summary>
        [DataMember(Order = 2, Name = "namefilter")]
        [ApiMember(Description = "名称过滤（包括城市名称）")]
        public string NameFilter { get; set; }
    }

    /// <summary>
    ///     查询并列举一组城市的响应。
    /// </summary>
    [DataContract]
    public class CityListResponse : IHasResponseStatus
    {
        /// <summary>
        ///     城市信息列表。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "城市信息列表")]
        public List<CityDto> Cities { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}