using Quartz;

namespace ServiceStack.Quartz
{
    /// <summary>
    ///     作业实例。
    /// </summary>
    public class JobInstance
    {
        /// <summary>
        ///     作业明细。
        /// </summary>
        public IJobDetail JobDetail { get; set; }

        /// <summary>
        ///     触发器列表。
        /// </summary>
        public ITrigger[] Triggers { get; set; }
    }
}