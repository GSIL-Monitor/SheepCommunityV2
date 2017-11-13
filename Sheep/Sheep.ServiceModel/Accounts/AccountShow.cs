using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Accounts.Entities;

namespace Sheep.ServiceModel.Accounts
{
    /// <summary>
    ///     显示一个帐户的请求。
    /// </summary>
    [Route("/account", HttpMethods.Get)]
    [DataContract]
    public class AccountShow : IReturn<AccountShowResponse>
    {
    }

    /// <summary>
    ///     显示一个帐户的响应。
    /// </summary>
    [DataContract]
    public class AccountShowResponse : IHasResponseStatus
    {
        /// <summary>
        ///     帐户信息。
        /// </summary>
        [DataMember(Order = 1)]
        public AccountDto Account { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}