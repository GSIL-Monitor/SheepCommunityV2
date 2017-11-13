using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Groups
{
    /// <summary>
    ///     更改封面图片的请求。
    /// </summary>
    [Route("/groups/{GroupId}/coverphoto", HttpMethods.Put)]
    [DataContract]
    public class GroupChangeCoverPhoto : IReturn<GroupChangeCoverPhotoResponse>
    {
        /// <summary>
        ///     群组的编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public string GroupId { get; set; }

        /// <summary>
        ///     来源封面图片的地址。
        /// </summary>
        [DataMember(Order = 2)]
        public string SourceCoverPhotoUrl { get; set; }
    }

    /// <summary>
    ///     更改封面图片的响应。
    /// </summary>
    [DataContract]
    public class GroupChangeCoverPhotoResponse : IHasResponseStatus
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