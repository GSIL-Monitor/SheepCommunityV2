using System.Runtime.Serialization;
using ServiceStack.Quartz.Services.Models.Entities;

namespace ServiceStack.Quartz.Services.Models
{
    /// <summary>
    ///     显示作业的请求。
    /// </summary>
    [Route("/quartz/jobs/{GroupName}/{JobName}", HttpMethods.Get, Summary = "显示一个作业")]
    [DataContract]
    public class QuartzJobShow : IReturn<QuartzJobShowResponse>
    {
        /// <summary>
        ///     分组的编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "分组的编号")]
        public string GroupName { get; set; }

        /// <summary>
        ///     作业的编号。
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        [ApiMember(Description = "作业的编号")]
        public string JobName { get; set; }
    }

    /// <summary>
    ///     显示作业的响应。
    /// </summary>
    [DataContract]
    public class QuartzJobShowResponse : IHasResponseStatus
    {
        /// <summary>
        ///     作业。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "作业")]
        public JobDto Job { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}