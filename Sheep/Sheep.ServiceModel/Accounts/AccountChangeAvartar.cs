using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Accounts
{
    /// <summary>
    ///     更改帐户头像的请求。
    /// </summary>
    [Route("/account/avatar", HttpMethods.Put, Summary = "更改帐户头像", Notes = "上传本地头像无须输入来源头像的地址。")]
    [DataContract]
    public class AccountChangeAvatar : IReturn<AccountChangeAvatarResponse>
    {
        /// <summary>
        ///     来源头像的地址。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "来源头像的地址")]
        public string SourceAvatarUrl { get; set; }
    }

    /// <summary>
    ///     更改帐户头像的响应。
    /// </summary>
    [DataContract]
    public class AccountChangeAvatarResponse : IHasResponseStatus
    {
        /// <summary>
        ///     头像的地址。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "头像的地址")]
        public string AvatarUrl { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}