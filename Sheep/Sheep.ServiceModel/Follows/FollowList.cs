using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Follows.Entities;

namespace Sheep.ServiceModel.Follows
{
    /// <summary>
    ///     列举一组被关注者的关注的请求。
    /// </summary>
    [Route("/follows/following", HttpMethods.Get)]
    [DataContract]
    public class FollowListOfFollowingUser : IReturn<FollowListResponse>
    {
        /// <summary>
        ///     关注者的用户编号。
        /// </summary>
        [DataMember(Order = 1, Name = "followerid", IsRequired = true)]
        public int FollowerId { get; set; }

        /// <summary>
        ///     创建日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 2, Name = "createdsince")]
        public DateTime? CreatedSince { get; set; }

        /// <summary>
        ///     修改日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 3, Name = "modifiedsince")]
        public DateTime? ModifiedSince { get; set; }

        /// <summary>
        ///     排序的字段。（可选值：IsBidirectional, CreatedDate, ModifiedDate 默认为 CreatedDate）
        /// </summary>
        [DataMember(Order = 4, Name = "orderby")]
        public string OrderBy { get; set; }

        /// <summary>
        ///     是否按降序排序。
        /// </summary>
        [DataMember(Order = 5, Name = "descending")]
        public bool? Descending { get; set; }

        /// <summary>
        ///     忽略的行数。
        /// </summary>
        [DataMember(Order = 6, Name = "skip")]
        public int? Skip { get; set; }

        /// <summary>
        ///     获取的行数。
        /// </summary>
        [DataMember(Order = 7, Name = "limit")]
        public int? Limit { get; set; }
    }

    /// <summary>
    ///     列举一组关注者的关注的请求。
    /// </summary>
    [Route("/follows/followed", HttpMethods.Get)]
    [DataContract]
    public class FollowListOfFollower : IReturn<FollowListResponse>
    {
        /// <summary>
        ///     被关注者的用户编号。
        /// </summary>
        [DataMember(Order = 1, Name = "followinguserid", IsRequired = true)]
        public int FollowingUserId { get; set; }

        /// <summary>
        ///     创建日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 2, Name = "createdsince")]
        public DateTime? CreatedSince { get; set; }

        /// <summary>
        ///     修改日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 3, Name = "modifiedsince")]
        public DateTime? ModifiedSince { get; set; }

        /// <summary>
        ///     排序的字段。（可选值：IsBidirectional, CreatedDate, ModifiedDate 默认为 CreatedDate）
        /// </summary>
        [DataMember(Order = 4, Name = "orderby")]
        public string OrderBy { get; set; }

        /// <summary>
        ///     是否按降序排序。
        /// </summary>
        [DataMember(Order = 5, Name = "descending")]
        public bool? Descending { get; set; }

        /// <summary>
        ///     忽略的行数。
        /// </summary>
        [DataMember(Order = 6, Name = "skip")]
        public int? Skip { get; set; }

        /// <summary>
        ///     获取的行数。
        /// </summary>
        [DataMember(Order = 7, Name = "limit")]
        public int? Limit { get; set; }
    }

    /// <summary>
    ///     列举一组关注的响应。
    /// </summary>
    [DataContract]
    public class FollowListResponse : IHasResponseStatus
    {
        /// <summary>
        ///     关注信息列表。
        /// </summary>
        [DataMember(Order = 1)]
        public List<FollowDto> Follows { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}