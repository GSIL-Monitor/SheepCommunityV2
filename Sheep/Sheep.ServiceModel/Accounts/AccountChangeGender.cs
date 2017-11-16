using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Accounts
{
    /// <summary>
    ///     更改帐户性别的请求。
    /// </summary>
    [Route("/account/gender", HttpMethods.Put, Summary = "更改帐户性别")]
    [DataContract]
    public class AccountChangeGender : IReturn<AccountChangeGenderResponse>
    {
        /// <summary>
        ///     更改的性别。（可选的值：男, 女）
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "更改的性别（可选的值：男, 女）")]
        public string Gender { get; set; }
    }

    /// <summary>
    ///     更改帐户性别的响应。
    /// </summary>
    [DataContract]
    public class AccountChangeGenderResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}