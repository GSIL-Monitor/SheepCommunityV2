using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.States.Entities;

namespace Sheep.ServiceModel.States
{
    /// <summary>
    ///     列举一组省份的请求。
    /// </summary>
    [Route("/states/query", HttpMethods.Get)]
    [DataContract]
    public class StateList : IReturn<StateListResponse>
    {
        /// <summary>
        ///     国家编号。
        /// </summary>
        [DataMember(Order = 1, Name = "countryid", IsRequired = true)]
        public string CountryId { get; set; }

        /// <summary>
        ///     过滤名称。（包括省份名称）
        /// </summary>
        [DataMember(Order = 2, Name = "namefilter")]
        public string NameFilter { get; set; }
    }

    /// <summary>
    ///     列举一组省份的响应。
    /// </summary>
    [DataContract]
    public class StateListResponse : IHasResponseStatus
    {
        /// <summary>
        ///     省份信息列表。
        /// </summary>
        [DataMember(Order = 1)]
        public List<StateDto> States { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}