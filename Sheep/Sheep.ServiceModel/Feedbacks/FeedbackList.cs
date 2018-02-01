using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Feedbacks.Entities;

namespace Sheep.ServiceModel.Feedbacks
{
    /// <summary>
    ///     查询并列举一组反馈的请求。
    /// </summary>
    [Route("/feedbacks/query", HttpMethods.Get, Summary = "查询并列举一组反馈信息")]
    [DataContract]
    public class FeedbackList : IReturn<FeedbackListResponse>
    {
        /// <summary>
        ///     过滤内容。
        /// </summary>
        [DataMember(Order = 1, Name = "contentfilter")]
        [ApiMember(Description = "过滤内容")]
        public string ContentFilter { get; set; }

        /// <summary>
        ///     创建日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 2, Name = "createdsince")]
        [ApiMember(Description = "创建日期在指定的时间之后")]
        public long? CreatedSince { get; set; }

        /// <summary>
        ///     修改日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 3, Name = "modifiedsince")]
        [ApiMember(Description = "修改日期在指定的时间之后")]
        public long? ModifiedSince { get; set; }

        /// <summary>
        ///     状态。（可选值：待处理, 提交技术, 提交产品, 提交运营, 等待删除）
        /// </summary>
        [DataMember(Order = 4, Name = "status")]
        [ApiMember(Description = "状态（可选值：待处理, 提交技术, 提交产品, 提交运营, 等待删除）")]
        public string Status { get; set; }

        /// <summary>
        ///     排序的字段。（可选值：CreatedDate, ModifiedDate 默认为 CreatedDate）
        /// </summary>
        [DataMember(Order = 5, Name = "orderby")]
        [ApiMember(Description = "排序的字段（可选值：CreatedDate, ModifiedDate 默认为 CreatedDate）")]
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
    ///     根据用户查询并列举一组反馈的请求。
    /// </summary>
    [Route("/feedbacks/query/byuser", HttpMethods.Get, Summary = "根据用户查询并列举一组反馈信息")]
    [DataContract]
    public class FeedbackListByUser : IReturn<FeedbackListResponse>
    {
        /// <summary>
        ///     用户编号。
        /// </summary>
        [DataMember(Order = 1, Name = "userid", IsRequired = true)]
        [ApiMember(Description = "用户编号")]
        public int UserId { get; set; }

        /// <summary>
        ///     创建日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 2, Name = "createdsince")]
        [ApiMember(Description = "创建日期在指定的时间之后")]
        public long? CreatedSince { get; set; }

        /// <summary>
        ///     修改日期在指定的时间之后。
        /// </summary>
        [DataMember(Order = 3, Name = "modifiedsince")]
        [ApiMember(Description = "修改日期在指定的时间之后")]
        public long? ModifiedSince { get; set; }

        /// <summary>
        ///     状态。（可选值：待处理, 提交技术, 提交产品, 提交运营, 等待删除）
        /// </summary>
        [DataMember(Order = 4, Name = "status")]
        [ApiMember(Description = "状态（可选值：待处理, 提交技术, 提交产品, 提交运营, 等待删除）")]
        public string Status { get; set; }

        /// <summary>
        ///     排序的字段。（可选值：CreatedDate, ModifiedDate 默认为 CreatedDate）
        /// </summary>
        [DataMember(Order = 5, Name = "orderby")]
        [ApiMember(Description = "排序的字段（可选值：CreatedDate, ModifiedDate 默认为 CreatedDate）")]
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
    ///     查询并列举一组反馈的响应。
    /// </summary>
    [DataContract]
    public class FeedbackListResponse : IHasResponseStatus
    {
        /// <summary>
        ///     反馈信息列表。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "反馈信息列表")]
        public List<FeedbackDto> Feedbacks { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}