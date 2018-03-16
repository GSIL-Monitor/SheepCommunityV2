using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Users.Entities;

namespace Sheep.ServiceModel.Users
{
    /// <summary>
    ///     查询并列举一组用户排行的请求。
    /// </summary>
    [Route("/users/rank", HttpMethods.Get, Summary = "查询并列举一组用户排行")]
    [DataContract]
    public class UserRank : IReturn<UserRankResponse>
    {
        /// <summary>
        ///     排序的字段。（可选值：ViewsCount, ParentsCount, DaysCount 默认为 ViewsCount）
        /// </summary>
        [DataMember(Order = 1, Name = "orderby")]
        [ApiMember(Description = "排序的字段（可选值：ViewsCount, ParentsCount, DaysCount 默认为 ViewsCount）")]
        public string OrderBy { get; set; }

        /// <summary>
        ///     是否按降序排序。
        /// </summary>
        [DataMember(Order = 2, Name = "descending")]
        [ApiMember(Description = "是否按降序排序")]
        public bool? Descending { get; set; }

        /// <summary>
        ///     忽略的行数。
        /// </summary>
        [DataMember(Order = 3, Name = "skip")]
        [ApiMember(Description = "忽略的行数")]
        public int? Skip { get; set; }

        /// <summary>
        ///     获取的行数。
        /// </summary>
        [DataMember(Order = 4, Name = "limit")]
        [ApiMember(Description = "获取的行数")]
        public int? Limit { get; set; }
    }

    /// <summary>
    ///     根据编号列表查询并列举一组用户排行的请求。
    /// </summary>
    [Route("/users/rank/byids", HttpMethods.Get, Summary = "根据编号列表查询并列举一组用户排行")]
    [DataContract]
    public class UserRankByIds : IReturn<UserRankResponse>
    {
        /// <summary>
        ///     用户编号列表。
        /// </summary>
        [DataMember(Order = 1, Name = "userids", IsRequired = true)]
        [ApiMember(Description = "用户编号列表")]
        public List<int> UserIds { get; set; }

        /// <summary>
        ///     排序的字段。（可选值：ViewsCount, ParentsCount, DaysCount 默认为 ViewsCount）
        /// </summary>
        [DataMember(Order = 5, Name = "orderby")]
        [ApiMember(Description = "排序的字段（可选值：ViewsCount, ParentsCount, DaysCount 默认为 ViewsCount）")]
        public string OrderBy { get; set; }

        /// <summary>
        ///     是否按降序排序。
        /// </summary>
        [DataMember(Order = 6, Name = "descending")]
        [ApiMember(Description = "是否按降序排序")]
        public bool? Descending { get; set; }

        /// <summary>
        ///     忽略的行数。
        /// </summary>
        [DataMember(Order = 7, Name = "skip")]
        [ApiMember(Description = "忽略的行数")]
        public int? Skip { get; set; }

        /// <summary>
        ///     获取的行数。
        /// </summary>
        [DataMember(Order = 8, Name = "limit")]
        [ApiMember(Description = "获取的行数")]
        public int? Limit { get; set; }
    }

    /// <summary>
    ///     查询并列举一组用户排行的响应。
    /// </summary>
    [DataContract]
    public class UserRankResponse : IHasResponseStatus
    {
        /// <summary>
        ///     用户排行信息列表。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "用户排行信息列表")]
        public List<KeyValuePair<UserDto, int>> UserRanks { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}