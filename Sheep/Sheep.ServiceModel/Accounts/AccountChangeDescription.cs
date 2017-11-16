using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Accounts
{
    /// <summary>
    ///     更改帐户简介的请求。
    /// </summary>
    [Route("/account/description", HttpMethods.Put, Summary = "更改帐户简介")]
    [DataContract]
    public class AccountChangeDescription : IReturn<AccountChangeDescriptionResponse>
    {
        /// <summary>
        ///     更改的简介。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "更改的简介")]
        public string Description { get; set; }
    }

    /// <summary>
    ///     更改帐户简介的响应。
    /// </summary>
    [DataContract]
    public class AccountChangeDescriptionResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}