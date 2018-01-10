using System.Runtime.Serialization;
using ServiceStack.Quartz.Services.Models.Entities;

namespace ServiceStack.Quartz.Services.Models
{
    /// <summary>
    ///     显示触发器的请求。
    /// </summary>
    [Route("/quartz/triggers/{GroupName}/{TriggerName}", HttpMethods.Get, Summary = "显示一个触发器")]
    [DataContract]
    public class QuartzTriggerShow : IReturn<QuartzTriggerShowResponse>
    {
        /// <summary>
        ///     分组的名称。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "分组的名称")]
        public string GroupName { get; set; }

        /// <summary>
        ///     触发器的名称。
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        [ApiMember(Description = "触发器的名称")]
        public string TriggerName { get; set; }
    }

    /// <summary>
    ///     显示触发器的响应。
    /// </summary>
    [DataContract]
    public class QuartzTriggerShowResponse : IHasResponseStatus
    {
        /// <summary>
        ///     触发器。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "触发器")]
        public TriggerDto Trigger { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}