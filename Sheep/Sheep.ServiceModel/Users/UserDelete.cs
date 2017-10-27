using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Users
{
    /// <summary>
    ///     删除一个用户的请求。
    /// </summary>
    [Route("/users/{UserId}", HttpMethods.Delete)]
    [DataContract]
    public class UserDelete : IReturn<UserDeleteResponse>
    {
        /// <summary>
        ///     用户编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public int UserId { get; set; }

        /// <summary>
        ///     是否自动删除所有的相关内容。
        /// </summary>
        [DataMember(Order = 2)]
        public bool? DeleteContents { get; set; }

        /// <summary>
        ///     将不删除的相关内容重新分配至新的用户编号下。
        /// </summary>
        [DataMember(Order = 3)]
        public int? ReassignContentsToUserId { get; set; }
    }

    /// <summary>
    ///     删除一个用户的响应。
    /// </summary>
    [DataContract]
    public class UserDeleteResponse : IHasResponseStatus
    {
        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 1)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}