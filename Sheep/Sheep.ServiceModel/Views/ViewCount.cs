using System;
using System.Runtime.Serialization;
using ServiceStack;

namespace Sheep.ServiceModel.Views
{
    /// <summary>
    ///     根据上级查询并统计一组阅读数量的请求。
    /// </summary>
    [Route("/views/count/byparent", HttpMethods.Get, Summary = "根据上级查询并统计一组阅读数量")]
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
    ///     根据用户查询并统计一组阅读数量的请求。
    /// </summary>
    [Route("/views/count/byuser", HttpMethods.Get, Summary = "根据用户查询并统计一组阅读数量")]
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
    ///     统计一组阅读数量的响应。
    /// </summary>
    [DataContract]
    public class ViewCountResponse : IHasResponseStatus
    {
        /// <summary>
        ///     阅读次数。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "阅读次数")]
        public int ViewsCount { get; set; }

        /// <summary>
        ///     上级对象数量。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "上级对象数量")]
        public int ParentsCount { get; set; }

        /// <summary>
        ///     天数。
        /// </summary>
        [DataMember(Order = 3)]
        [ApiMember(Description = "天数")]
        public int DaysCount { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 4)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}