using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Accounts
{
    /// <summary>
    ///     更改头像的请求。
    /// </summary>
    [Route("/account/avatar", HttpMethods.Put)]
    [DataContract]
    public class AccountChangeAvatar : IReturn<AccountChangeAvatarResponse>
    {
        /// <summary>
        ///     图像来源的地址。
        /// </summary>
        [DataMember(Order = 1)]
        public string SourceUrl { get; set; }
    }

    /// <summary>
    ///     更改头像的响应。
    /// </summary>
    [DataContract]
    public class AccountChangeAvatarResponse : IHasResponseStatus
    {
        /// <summary>
        ///     头像的地址。
        /// </summary>
        [DataMember(Order = 1)]
        public string AvatarUrl { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}