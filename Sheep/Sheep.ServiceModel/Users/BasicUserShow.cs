using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Users.Entities;

namespace Sheep.ServiceModel.Users
{
    /// <summary>
    ///     显示一个用户基本信息的请求。
    /// </summary>
    [Route("/users/basic/{UserId}", HttpMethods.Get, Summary = "显示一个用户基本信息")]
    [DataContract]
    public class BasicUserShow : IReturn<BasicUserShowResponse>
    {
        /// <summary>
        ///     用户编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "用户编号")]
        public int UserId { get; set; }
    }

    /// <summary>
    ///     根据用户名称或电子邮件地址显示一个用户基本信息的请求。
    /// </summary>
    [Route("/users/basic/show/{UserNameOrEmail}", HttpMethods.Get, Summary = "根据用户名称或电子邮件地址显示一个用户基本信息")]
    [DataContract]
    public class BasicUserShowByUserNameOrEmail : IReturn<BasicUserShowResponse>
    {
        /// <summary>
        ///     用户名称或电子邮件地址。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "用户名称或电子邮件地址")]
        public string UserNameOrEmail { get; set; }
    }

    /// <summary>
    ///     根据显示名称显示一个用户基本信息的请求。
    /// </summary>
    [Route("/users/basic/showname/{DisplayName}", HttpMethods.Get, Summary = "根据显示名称显示一个用户基本信息")]
    [DataContract]
    public class BasicUserShowByDisplayName : IReturn<BasicUserShowResponse>
    {
        /// <summary>
        ///     显示名称。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "显示名称")]
        public string DisplayName { get; set; }
    }

    /// <summary>
    ///     显示一个用户基本信息的响应。
    /// </summary>
    [DataContract]
    public class BasicUserShowResponse : IHasResponseStatus
    {
        /// <summary>
        ///     用户基本信息。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "用户基本信息")]
        public BasicUserDto User { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}