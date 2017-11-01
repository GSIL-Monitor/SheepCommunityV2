using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Countries.Entities;

namespace Sheep.ServiceModel.Countries
{
    /// <summary>
    ///     显示一个国家的请求。
    /// </summary>
    [Route("/countries/{CountryId}", HttpMethods.Get)]
    [DataContract]
    public class CountryShow : IReturn<CountryShowResponse>
    {
        /// <summary>
        ///     国家编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public string CountryId { get; set; }
    }

    /// <summary>
    ///     根据国家名称显示一个国家的请求。
    /// </summary>
    [Route("/countries/show/{Name}", HttpMethods.Get)]
    [DataContract]
    public class CountryShowByName : IReturn<CountryShowResponse>
    {
        /// <summary>
        ///     国家名称。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
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
        public CountryDto Country { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}