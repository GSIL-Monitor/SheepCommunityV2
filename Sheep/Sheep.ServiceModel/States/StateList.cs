using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.States.Entities;

namespace Sheep.ServiceModel.States
{
    /// <summary>
    ///     查询并列举一组省份的请求。
    /// </summary>
    [Route("/states/query", HttpMethods.Get, Summary = "查询并列举一组省份")]
    [DataContract]
    public class StateList : IReturn<StateListResponse>
    {
        /// <summary>
        ///     国家编号。
        /// </summary>
        [DataMember(Order = 1, Name = "countryid", IsRequired = true)]
        [ApiMember(Description = "国家编号")]
        public string CountryId { get; set; }

        /// <summary>
        ///     名称过滤。（包括省份名称）
        /// </summary>
        [DataMember(Order = 2, Name = "namefilter")]
        [ApiMember(Description = "名称过滤。（包括省份名称）")]
        public string NameFilter { get; set; }
    }

    /// <summary>
    ///     查询并列举一组省份的响应。
    /// </summary>
    [DataContract]
    public class StateListResponse : IHasResponseStatus
    {
        /// <summary>
        ///     省份信息列表。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "省份信息列表")]
        public List<StateDto> States { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}