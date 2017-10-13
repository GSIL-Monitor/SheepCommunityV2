using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.GroupUsers
{
    /// <summary>
    ///     删除一个群组用户的请求。
    /// </summary>
    [Route("/groups/{GroupId}/users/{UserId}", HttpMethods.Delete)]
    [DataContract]
    public class GroupUserDelete : IReturn<GroupUserDeleteResponse>
    {
        /// <summary>
        ///     群组的编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public string GroupId { get; set; }

        /// <summary>
        ///     要加入群组的用户编号。
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        public int UserId { get; set; }
    }

    /// <summary>
    ///     删除一个群组用户的响应。
    /// </summary>
    [DataContract]
    public class GroupUserDeleteResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}