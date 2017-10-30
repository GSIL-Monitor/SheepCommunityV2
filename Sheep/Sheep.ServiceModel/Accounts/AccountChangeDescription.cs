using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Accounts
{
    /// <summary>
    ///     更改简介的请求。
    /// </summary>
    [Route("/account/description", HttpMethods.Put)]
    [DataContract]
    public class AccountChangeDescription : IReturn<AccountChangeDescriptionResponse>
    {
        /// <summary>
        ///     更改的简介。
        /// </summary>
        [DataMember(Order = 1)]
        public string Description { get; set; }
    }

    /// <summary>
    ///     更改简介的响应。
    /// </summary>
    [DataContract]
    public class AccountChangeDescriptionResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}