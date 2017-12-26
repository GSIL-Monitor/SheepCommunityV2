using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Groups
{
    /// <summary>
    ///     更改群组真实名称的请求。
    /// </summary>
    [Route("/groups/{GroupId}/fullname", HttpMethods.Put, Summary = "更改群组真实名称")]
    [DataContract]
    public class GroupChangeFullName : IReturn<GroupChangeFullNameResponse>
    {
        /// <summary>
        ///     群组编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "群组编号")]
        public string GroupId { get; set; }

        /// <summary>
        ///     更改的真实名称。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "更改的真实名称")]
        public string FullName { get; set; }

        /// <summary>
        ///     来源执照图片的地址。
        /// </summary>
        [DataMember(Order = 3)]
        [ApiMember(Description = "来源执照图片的地址")]
        public string SourceIdImageUrl { get; set; }
    }

    /// <summary>
    ///     更改群组真实名称的响应。
    /// </summary>
    [DataContract]
    public class GroupChangeFullNameResponse : IHasResponseStatus
    {
        /// <summary>
        ///     执照图片的地址。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "执照图片的地址")]
        public string IdImageUrl { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}