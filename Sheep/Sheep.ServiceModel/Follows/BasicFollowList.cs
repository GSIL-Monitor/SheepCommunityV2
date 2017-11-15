using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Follows.Entities;

namespace Sheep.ServiceModel.Follows
{
    /// <summary>
    ///     列举一组被关注者的关注基本信息的请求。
    /// </summary>
    [Route("/follows/basic/following", HttpMethods.Get)]
    [DataContract]
    public class BasicFollowListOfFollowingUser : IReturn<BasicFollowListOfFollowingUserResponse>
    {
        /// <summary>
        ///     关注者的用户编号。
        /// </summary>
        [DataMember(Order = 1, Name = "followerid", IsRequired = true)]
        public int FollowerId { get; set; }

        /// <summary>
        ///     忽略的行数。
        /// </summary>
        [DataMember(Order = 2, Name = "skip")]
        public int? Skip { get; set; }

        /// <summary>
        ///     获取的行数。
        /// </summary>
        [DataMember(Order = 3, Name = "limit")]
        public int? Limit { get; set; }
    }

    /// <summary>
    ///     列举一组被关注者的关注基本信息的响应。
    /// </summary>
    [DataContract]
    public class BasicFollowListOfFollowingUserResponse : IHasResponseStatus
    {
        /// <summary>
        ///     关注基本信息列表。
        /// </summary>
        [DataMember(Order = 1)]
        public List<BasicFollowOfFollowingUserDto> Follows { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }
    }

    /// <summary>
    ///     列举一组关注者的关注基本信息的请求。
    /// </summary>
    [Route("/follows/basic/followed", HttpMethods.Get)]
    [DataContract]
    public class BasicFollowListOfFollower : IReturn<BasicFollowListOfFollowerResponse>
    {
        /// <summary>
        ///     被关注者的用户编号。
        /// </summary>
        [DataMember(Order = 1, Name = "followinguserid", IsRequired = true)]
        public int FollowingUserId { get; set; }

        /// <summary>
        ///     忽略的行数。
        /// </summary>
        [DataMember(Order = 2, Name = "skip")]
        public int? Skip { get; set; }

        /// <summary>
        ///     获取的行数。
        /// </summary>
        [DataMember(Order = 3, Name = "limit")]
        public int? Limit { get; set; }
    }

    /// <summary>
    ///     列举一组关注者的关注基本信息的响应。
    /// </summary>
    [DataContract]
    public class BasicFollowListOfFollowerResponse : IHasResponseStatus
    {
        /// <summary>
        ///     关注基本信息列表。
        /// </summary>
        [DataMember(Order = 1)]
        public List<BasicFollowOfFollowerDto> Follows { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}