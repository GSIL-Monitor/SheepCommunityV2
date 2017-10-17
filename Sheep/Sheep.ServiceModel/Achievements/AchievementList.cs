using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using Sheep.Common.Infrastructure;
using Sheep.ServiceModel.Achievements.Entities;

namespace Sheep.ServiceModel.Achievements
{
    /// <summary>
    ///     列举一组成就的请求。
    /// </summary>
    [Route("/achievements/list", HttpMethods.Get)]
    [DataContract]
    public class AchievementList : IReturn<AchievementListResponse>
    {
        /// <summary>
        ///     是否已开启。
        /// </summary>
        [DataMember(Order = 1)]
        public bool? IsEnabled { get; set; }

        /// <summary>
        ///     排序的字段。（可选值：Title, CreatedDate, 默认为 Title）
        /// </summary>
        [DataMember(Order = 2)]
        public string OrderBy { get; set; }

        /// <summary>
        ///     是否按降序排序。
        /// </summary>
        [DataMember(Order = 3)]
        public bool? OrderDescending { get; set; }

        /// <summary>
        ///     忽略的行数。
        /// </summary>
        [DataMember(Order = 4)]
        public int? Skip { get; set; }

        /// <summary>
        ///     获取的行数。
        /// </summary>
        [DataMember(Order = 5)]
        public int? Take { get; set; }
    }

    /// <summary>
    ///     列举一组成就的响应。
    /// </summary>
    [DataContract]
    public class AchievementListResponse : IHasResponseStatus, IPaged
    {
        /// <summary>
        ///     成就信息列表。
        /// </summary>
        [DataMember(Order = 1)]
        public List<AchievementDto> Achievements { get; set; }

        /// <summary>
        ///     当前分页号。
        /// </summary>
        [DataMember(Order = 2)]
        public int PageNumber { get; set; }

        /// <summary>
        ///     单页行数。
        /// </summary>
        [DataMember(Order = 3)]
        public int PageSize { get; set; }

        /// <summary>
        ///     总行数。
        /// </summary>
        [DataMember(Order = 4)]
        public long TotalCount { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 5)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}