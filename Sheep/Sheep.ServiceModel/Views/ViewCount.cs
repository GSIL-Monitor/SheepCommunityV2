using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Views.Entities;

namespace Sheep.ServiceModel.Views
{
    /// <summary>
    ///     根据上级统计一组阅读数量的请求。
    /// </summary>
    [Route("/views/count/byparent", HttpMethods.Get, Summary = "根据上级统计一组阅读数量")]
    [DataContract]
    public class ViewCountByParent : IReturn<ViewCountResponse>
    {
        /// <summary>
        ///     上级编号。（如帖子编号）
        /// </summary>
        [DataMember(Order = 1, Name = "parentid", IsRequired = true)]
        [ApiMember(Description = "上级编号（如帖子编号）")]
        public string ParentId { get; set; }

        /// <summary>
        ///     是否为我的。
        /// </summary>
        [DataMember(Order = 2, Name = "ismine")]
        [ApiMember(Description = "是否为我的")]
        public bool? IsMine { get; set; }

        /// <summary>
        ///     创建日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 3, Name = "createdsince")]
        [ApiMember(Description = "创建日期在指定的时间之后")]
        public DateTime? CreatedSince { get; set; }
    }

    /// <summary>
    ///     根据用户统计一组阅读数量的请求。
    /// </summary>
    [Route("/views/count/byuser", HttpMethods.Get, Summary = "根据用户统计一组阅读数量")]
    [DataContract]
    public class ViewCountByUser : IReturn<ViewCountResponse>
    {
        /// <summary>
        ///     用户编号。
        /// </summary>
        [DataMember(Order = 1, Name = "userid", IsRequired = true)]
        [ApiMember(Description = "用户编号")]
        public int UserId { get; set; }

        /// <summary>
        ///     上级类型。（可选值：帖子, 章, 节）
        /// </summary>
        [DataMember(Order = 2, Name = "parenttype")]
        [ApiMember(Description = "上级类型（可选值：帖子, 章, 节）")]
        public string ParentType { get; set; }

        /// <summary>
        ///     上级编号的前缀。（如帖子编号）
        /// </summary>
        [DataMember(Order = 3, Name = "parentidprefix")]
        [ApiMember(Description = "上级编号的前缀（如帖子编号）")]
        public string ParentIdPrefix { get; set; }

        /// <summary>
        ///     创建日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 4, Name = "createdsince")]
        [ApiMember(Description = "创建日期在指定的时间之后")]
        public DateTime? CreatedSince { get; set; }
    }

    /// <summary>
    ///     根据用户列表统计一组阅读数量的请求。
    /// </summary>
    [Route("/views/count/byusers", HttpMethods.Get, Summary = "根据用户列表统计一组阅读数量")]
    [DataContract]
    public class ViewCountByUsers : IReturn<ViewCountByUsersResponse>
    {
        /// <summary>
        ///     用户编号列表。
        /// </summary>
        [DataMember(Order = 1, Name = "userids", IsRequired = true)]
        [ApiMember(Description = "用户编号列表")]
        public List<int> UserIds { get; set; }

        /// <summary>
        ///     上级类型。（可选值：帖子, 章, 节）
        /// </summary>
        [DataMember(Order = 2, Name = "parenttype")]
        [ApiMember(Description = "上级类型（可选值：帖子, 章, 节）")]
        public string ParentType { get; set; }

        /// <summary>
        ///     上级编号的前缀。（如帖子编号）
        /// </summary>
        [DataMember(Order = 3, Name = "parentidprefix")]
        [ApiMember(Description = "上级编号的前缀（如帖子编号）")]
        public string ParentIdPrefix { get; set; }

        /// <summary>
        ///     创建日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 4, Name = "createdsince")]
        [ApiMember(Description = "创建日期在指定的时间之后")]
        public DateTime? CreatedSince { get; set; }
    }

    /// <summary>
    ///     统计一组阅读数量的响应。
    /// </summary>
    [DataContract]
    public class ViewCountResponse : IHasResponseStatus
    {
        /// <summary>
        ///     阅读数量。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "阅读数量")]
        public ViewCountsDto Counts { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }

    /// <summary>
    ///     根据用户列表统计一组阅读数量的响应。
    /// </summary>
    [DataContract]
    public class ViewCountByUsersResponse : IHasResponseStatus
    {
        /// <summary>
        ///     一组用户的阅读数量。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "一组用户的阅读数量")]
        public Dictionary<int, ViewCountsDto> UsersCounts { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}