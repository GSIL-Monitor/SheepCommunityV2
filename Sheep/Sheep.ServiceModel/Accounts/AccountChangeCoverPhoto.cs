using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Accounts
{
    /// <summary>
    ///     更改帐户封面图片的请求。
    /// </summary>
    [Route("/account/coverphoto", HttpMethods.Put, Summary = "更改帐户封面图片", Notes = "上传本地封面图片无须输入来源封面图片的地址。")]
    [DataContract]
    public class AccountChangeCoverPhoto : IReturn<AccountChangeCoverPhotoResponse>
    {
        /// <summary>
        ///     来源封面图片的地址。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "来源封面图片的地址")]
        public string SourceCoverPhotoUrl { get; set; }
    }

    /// <summary>
    ///     更改帐户封面图片的响应。
    /// </summary>
    [DataContract]
    public class AccountChangeCoverPhotoResponse : IHasResponseStatus
    {
        /// <summary>
        ///     封面图片的地址。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "封面图片的地址")]
        public string CoverPhotoUrl { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}