using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Groups
{
    /// <summary>
    ///     更改群组简介的请求。
    /// </summary>
    [Route("/groups/{GroupId}/description", HttpMethods.Put, Summary = "更改群组简介")]
    [DataContract]
    public class GroupChangeDescription : IReturn<GroupChangeDescriptionResponse>
    {
        /// <summary>
        ///     群组编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "群组编号")]
        public string GroupId { get; set; }

        /// <summary>
        ///     更改的简介。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "更改的简介")]
        public string Description { get; set; }
    }

    /// <summary>
    ///     更改群组简介的响应。
    /// </summary>
    [DataContract]
    public class GroupChangeDescriptionResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}