using System;
using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Accounts
{
    /// <summary>
    ///     更改出生日期的请求。
    /// </summary>
    [Route("/account/birthdate", HttpMethods.Put)]
    [DataContract]
    public class AccountChangeBirthDate : IReturn<AccountChangeBirthDateResponse>
    {
        /// <summary>
        ///     更改的出生日期。
        /// </summary>
        [DataMember(Order = 1)]
        public DateTime? BirthDate { get; set; }
    }

    /// <summary>
    ///     更改出生日期的响应。
    /// </summary>
    [DataContract]
    public class AccountChangeBirthDateResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}