using System;
using System.Linq;
using Quartz;
using RandomNameGeneratorLibrary;

namespace ServiceStack.Quartz
{
    /// <summary>
    ///     Quartz 功能的扩展。
    /// </summary>
    public static class QuartzFeatureExtensions
    {
        #region 属性

        private static readonly IPlaceNameGenerator s_NameGenerator = new PlaceNameGenerator(new Random(Guid.NewGuid().GetHashCode()));

        #endregion

        #region 注册作业

        /// <summary>
        ///     在 Quartz 调度器中注册一个作业。
        /// </summary>
        /// <typeparam name="TJob">作业的类型。</typeparam>
        /// <param name="quartzFeature">Quartz 功能。</param>
        /// <param name="trigger">触发器。</param>
        public static void RegisterJob<TJob>(this QuartzFeature quartzFeature, ITrigger trigger)
            where TJob : IJob
        {
            var jobDetail = JobBuilder.Create<TJob>().WithIdentity(quartzFeature.GetJobIdentity<TJob>()).Build();
            quartzFeature.RegisterJob(trigger, jobDetail);
        }

        /// <summary>
        ///     在 Quartz 调度器中注册一个作业。
        /// </summary>
        /// <typeparam name="TJob">作业的类型。</typeparam>
        /// <param name="quartzFeature">Quartz 功能。</param>
        /// <param name="trigger">触发器。</param>
        /// <param name="jobDetail">作业明细。</param>
        public static void RegisterJob<TJob>(this QuartzFeature quartzFeature, ITrigger trigger, IJobDetail jobDetail)
            where TJob : IJob
        {
            quartzFeature.RegisterJob(trigger, jobDetail);
        }

        /// <summary>
        ///     在 Quartz 调度器中注册一个作业。
        /// </summary>
        /// <typeparam name="TJob">作业的类型。</typeparam>
        /// <param name="quartzFeature">Quartz 功能。</param>
        /// <param name="createTrigger">创建触发器的方法。</param>
        public static void RegisterJob<TJob>(this QuartzFeature quartzFeature, Func<TriggerBuilder, ITrigger> createTrigger)
            where TJob : IJob
        {
            var trigger = createTrigger.Invoke(TriggerBuilder.Create().WithIdentity(quartzFeature.GetTriggerIdentity<TJob>()));
            var jobDetail = JobBuilder.Create<TJob>().WithIdentity(quartzFeature.GetJobIdentity<TJob>()).Build();
            quartzFeature.RegisterJob(trigger, jobDetail);
        }

        /// <summary>
        ///     在 Quartz 调度器中注册一个作业。
        /// </summary>
        /// <typeparam name="TJob">作业的类型。</typeparam>
        /// <param name="quartzFeature">Quartz 功能。</param>
        /// <param name="createTrigger">创建触发器的方法。</param>
        /// <param name="createJobDetail">创建作业明细的方法。</param>
        public static void RegisterJob<TJob>(this QuartzFeature quartzFeature, Func<TriggerBuilder, ITrigger> createTrigger, Func<JobBuilder, IJobDetail> createJobDetail)
            where TJob : IJob
        {
            var trigger = createTrigger.Invoke(TriggerBuilder.Create().WithIdentity(quartzFeature.GetTriggerIdentity<TJob>()));
            var jobDetail = createJobDetail.Invoke(JobBuilder.Create<TJob>().WithIdentity(quartzFeature.GetJobIdentity<TJob>()));
            quartzFeature.RegisterJob(trigger, jobDetail);
        }

        /// <summary>
        ///     在 Quartz 调度器中注册一个作业。
        /// </summary>
        /// <param name="quartzFeature">Quartz 功能。</param>
        /// <param name="trigger">触发器。</param>
        /// <param name="jobDetail">作业明细。</param>
        public static void RegisterJob(this QuartzFeature quartzFeature, ITrigger trigger, IJobDetail jobDetail)
        {
            quartzFeature.AddJob(jobDetail, trigger);
        }

        #endregion

        #region 获取随机编号

        /// <summary>
        ///     为作业生成一个随机键，避免与现有密钥发生冲突。
        /// </summary>
        /// <typeparam name="TJob">作业的类型。</typeparam>
        /// <param name="quartzFeature">Quartz 功能。</param>
        /// <returns>作业键。</returns>
        public static JobKey GetJobIdentity<TJob>(this QuartzFeature quartzFeature)
        {
            var groupName = string.Format("{0}", typeof(TJob).Namespace);
            var jobName = string.Format("{0}", typeof(TJob).Name);
            var jobIdentity = JobKey.Create(string.Format("{0}-{1}", jobName, s_NameGenerator.GenerateRandomPlaceName()), groupName);
            while (quartzFeature.Jobs.ContainsKey(jobIdentity))
            {
                jobIdentity = JobKey.Create(string.Format("{0}-{1}", jobName, s_NameGenerator.GenerateRandomPlaceName()), groupName);
            }
            return jobIdentity;
        }

        /// <summary>
        ///     为触发器生成一个随机键，避免与现有密钥发生冲突。
        /// </summary>
        /// <typeparam name="TJob">作业的类型。</typeparam>
        /// <param name="quartzFeature">Quartz 功能。</param>
        /// <returns>触发器键。</returns>
        public static TriggerKey GetTriggerIdentity<TJob>(this QuartzFeature quartzFeature)
        {
            var groupName = string.Format("{0}", typeof(TJob).Namespace);
            var jobName = string.Format("{0}", typeof(TJob).Name);
            var triggerIdentity = new TriggerKey(string.Format("{0}-{1}", jobName, s_NameGenerator.GenerateRandomPlaceName()), groupName);
            var triggers = quartzFeature.Jobs.Values.SelectMany(jobInstance => jobInstance.Triggers.Select(trigger => trigger.Key)).ToArray();
            while (triggers.Any(triggerKey => triggerKey.Equals(triggerIdentity)))
            {
                triggerIdentity = new TriggerKey(string.Format("{0}-{1}", jobName, s_NameGenerator.GenerateRandomPlaceName()), groupName);
            }
            return triggerIdentity;
        }

        #endregion
    }
}