using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Accounts
{
    /// <summary>
    ///     更改显示名称的请求。
    /// </summary>
    [Route("/account/displayname", HttpMethods.Put)]
    [DataContract]
    public class AccountChangeDisplayName : IReturn<AccountChangeDisplayNameResponse>
    {
        /// <summary>
        ///     更改的显示名称。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public string DisplayName { get; set; }
    }

    /// <summary>
    ///     更改显示名称的响应。
    /// </summary>
    [DataContract]
    public class AccountChangeDisplayNameResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}