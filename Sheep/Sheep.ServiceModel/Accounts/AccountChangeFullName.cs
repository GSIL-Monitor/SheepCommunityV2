using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Accounts
{
    /// <summary>
    ///     更改帐户真实姓名的请求。
    /// </summary>
    [Route("/account/fullname", HttpMethods.Put, Summary = "更改帐户真实姓名")]
    [DataContract]
    public class AccountChangeFullName : IReturn<AccountChangeFullNameResponse>
    {
        /// <summary>
        ///     更改的真实姓名。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "更改的真实姓名")]
        public string FullName { get; set; }

        /// <summary>
        ///     来源身份证图片的地址。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "来源身份证图片的地址")]
        public string SourceIdImageUrl { get; set; }
    }

    /// <summary>
    ///     更改帐户真实姓名的响应。
    /// </summary>
    [DataContract]
    public class AccountChangeFullNameResponse : IHasResponseStatus
    {
        /// <summary>
        ///     身份证图片的地址。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "身份证图片的地址")]
        public string IdImageUrl { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}