using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Countries.Entities;

namespace Sheep.ServiceModel.Countries
{
    /// <summary>
    ///     显示一个国家的请求。
    /// </summary>
    [Route("/countries/{CountryId}", HttpMethods.Get, Summary = "显示一个国家")]
    [DataContract]
    public class CountryShow : IReturn<CountryShowResponse>
    {
        /// <summary>
        ///     国家编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "国家编号")]
        public string CountryId { get; set; }
    }

    /// <summary>
    ///     根据国家名称显示一个国家的请求。
    /// </summary>
    [Route("/countries/show/{Name}", HttpMethods.Get, Summary = "根据国家名称显示一个国家")]
    [DataContract]
    public class CountryShowByName : IReturn<CountryShowResponse>
    {
        /// <summary>
        ///     国家名称。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "国家名称")]
        public string Name { get; set; }
    }

    /// <summary>
    ///     显示一个国家的响应。
    /// </summary>
    [DataContract]
    public class CountryShowResponse : IHasResponseStatus
    {
        /// <summary>
        ///     国家信息。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "国家信息")]
        public CountryDto Country { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}