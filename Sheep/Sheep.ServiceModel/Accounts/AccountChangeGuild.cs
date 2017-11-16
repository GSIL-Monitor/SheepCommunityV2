using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Accounts
{
    /// <summary>
    ///     更改帐户所属教会的请求。
    /// </summary>
    [Route("/account/guild", HttpMethods.Put, Summary = "更改帐户所属教会")]
    [DataContract]
    public class AccountChangeGuild : IReturn<AccountChangeGuildResponse>
    {
        /// <summary>
        ///     更改的所属教会。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "更改的所属教会")]
        public string Guild { get; set; }
    }

    /// <summary>
    ///     更改帐户所属教会的响应。
    /// </summary>
    [DataContract]
    public class AccountChangeGuildResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}