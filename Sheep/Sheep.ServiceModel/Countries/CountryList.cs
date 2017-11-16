using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Countries.Entities;

namespace Sheep.ServiceModel.Countries
{
    /// <summary>
    ///     列举一组国家的请求。
    /// </summary>
    [Route("/countries/query", HttpMethods.Get)]
    [DataContract]
    public class CountryList : IReturn<CountryListResponse>
    {
        /// <summary>
        ///     名称。（包括国家名称）
        /// </summary>
        [DataMember(Order = 1, Name = "namefilter")]
        public string NameFilter { get; set; }
    }

    /// <summary>
    ///     列举一组国家的响应。
    /// </summary>
    [DataContract]
    public class CountryListResponse : IHasResponseStatus
    {
        /// <summary>
        ///     国家信息列表。
        /// </summary>
        [DataMember(Order = 1)]
        public List<CountryDto> Countries { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}