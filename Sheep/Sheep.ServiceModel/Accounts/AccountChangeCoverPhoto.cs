using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Accounts
{
    /// <summary>
    ///     更改封面图片的请求。
    /// </summary>
    [Route("/account/coverphoto", HttpMethods.Put)]
    [DataContract]
    public class AccountChangeCoverPhoto : IReturn<AccountChangeCoverPhotoResponse>
    {
        /// <summary>
        ///     原始图像的地址。
        /// </summary>
        [DataMember(Order = 1)]
        public string SourceUrl { get; set; }
    }

    /// <summary>
    ///     更改封面图片的响应。
    /// </summary>
    [DataContract]
    public class AccountChangeCoverPhotoResponse : IHasResponseStatus
    {
        /// <summary>
        ///     封面图片的地址。
        /// </summary>
        [DataMember(Order = 1)]
        public string CoverPhotoUrl { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}