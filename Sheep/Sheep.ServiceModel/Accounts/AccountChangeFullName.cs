using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Accounts
{
    /// <summary>
    ///     更改真实姓名的请求。
    /// </summary>
    [Route("/account/fullname", HttpMethods.Put)]
    [DataContract]
    public class AccountChangeFullName : IReturn<AccountChangeFullNameResponse>
    {
        /// <summary>
        ///     更改的真实姓名。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public string FullName { get; set; }
    }

    /// <summary>
    ///     更改真实姓名的响应。
    /// </summary>
    [DataContract]
    public class AccountChangeFullNameResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}