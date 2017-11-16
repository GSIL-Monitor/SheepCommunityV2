using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Accounts
{
    /// <summary>
    ///     更改帐户所在地的请求。
    /// </summary>
    [Route("/account/location", HttpMethods.Put, Summary = "更改帐户所在地")]
    [DataContract]
    public class AccountChangeLocation : IReturn<AccountChangeLocationResponse>
    {
        /// <summary>
        ///     更改的所在地国家/地区。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "更改的所在地国家/地区")]
        public string Country { get; set; }

        /// <summary>
        ///     更改的所在地省份/直辖市/州。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "更改的所在地省份/直辖市/州")]
        public string State { get; set; }

        /// <summary>
        ///     更改的所在地城市/区域。
        /// </summary>
        [DataMember(Order = 3)]
        [ApiMember(Description = "更改的所在地城市/区域")]
        public string City { get; set; }
    }

    /// <summary>
    ///     更改帐户所在地的响应。
    /// </summary>
    [DataContract]
    public class AccountChangeLocationResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}