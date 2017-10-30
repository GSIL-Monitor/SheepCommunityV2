using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Accounts
{
    /// <summary>
    ///     更改性别的请求。
    /// </summary>
    [Route("/account/gender", HttpMethods.Put)]
    [DataContract]
    public class AccountChangeGender : IReturn<AccountChangeGenderResponse>
    {
        /// <summary>
        ///     更改的性别。（可选的值：0: 女, 1: 男）
        /// </summary>
        [DataMember(Order = 1)]
        public int? Gender { get; set; }
    }

    /// <summary>
    ///     更改性别的响应。
    /// </summary>
    [DataContract]
    public class AccountChangeGenderResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}