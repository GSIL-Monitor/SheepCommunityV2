using System;
using System.Runtime.Serialization;

namespace ServiceStack.Quartz.Services.Models.Entities
{
    /// <summary>
    ///     触发器信息。
    /// </summary>
    [DataContract]
    public class TriggerDto
    {
        /// <summary>
        ///     触发器主键。
        /// </summary>
        [DataMember(Order = 1)]
        public TriggerKeyDto Key { get; set; }

        /// <summary>
        ///     作业主键。
        /// </summary>
        [DataMember(Order = 2)]
        public JobKeyDto JobKey { get; set; }

        /// <summary>
        ///     由其创建者 (如果有的话) 给触发器实例的描述。
        /// </summary>
        [DataMember(Order = 3)]
        public string Description { get; set; }

        /// <summary>
        ///     具有此触发器的给定名称的日历。解除日历的关联时使用null。
        /// </summary>
        [DataMember(Order = 4)]
        public string CalendarName { get; set; }

        /// <summary>
        ///     触发器的调度应开始的时间。
        ///     根据触发器的类型和触发器的其他属性的设置，可以也可能不是触发器的第一个实际触发时间，然而第一次实际触发时间不会在这个日期之前。
        /// </summary>
        [DataMember(Order = 5)]
        public DateTimeOffset StartTimeUtc { get; set; }

        /// <summary>
        ///     触发器必须停止触发的日期/时间。
        ///     这定义了触发器触发的最终边界，触发器不会在该日期和时间后触发。如果此值为 null，则不假定结束时间边界，并且触发器可以无限期地继续。
        /// </summary>
        [DataMember(Order = 6)]
        public DateTimeOffset? EndTimeUtc { get; set; }

        /// <summary>
        ///     由调度器使用，以确定是否有可能为这个触发器再次触发。
        ///     如果返回的值为 false，则调度器可以从作业存储中删除此触发器。
        /// </summary>
        [DataMember(Order = 7)]
        public bool MayFireAgain { get; set; }

        /// <summary>
        ///     返回触发器计划触发的下一次时间。如果触发器不会再次触发，则返回 null。
        ///     请注意，如果为触发器计算的时间已经到达，但调度程序还没有能够触发触发器 （这很可能是由于缺少资源，如线程），返回的时间可能在过去。
        /// </summary>
        [DataMember(Order = 8)]
        public DateTimeOffset? NextFireTimeUtc { get; set; }

        /// <summary>
        ///     返回上次触发触发器的时间。如果触发器尚未触发，则返回 null。
        /// </summary>
        [DataMember(Order = 9)]
        public DateTimeOffset? PreviousFireTimeUtc { get; set; }

        /// <summary>
        ///     返回触发器将触发的最后一个 UTC 时间，如果触发器将无限期重复，则返回 null。
        ///     请注意，返回时间可能在过去。
        /// </summary>
        [DataMember(Order = 10)]
        public DateTimeOffset? FinalFireTimeUtc { get; set; }

        /// <summary>
        ///     是否为毫秒级精度。
        /// </summary>
        [DataMember(Order = 11)]
        public bool HasMillisecondPrecision { get; set; }

        /// <summary>
        ///     获取或设置应为处理此触发器的错失触发情况提供计划程序的指令——您使用的具体触发器类型将定义一组可能设置为此属性的附加 MISFIRE_INSTRUCTION_XXX 常量。
        ///     如果未显式设置，则默认值为 InstructionNotSet。
        /// </summary>
        [DataMember(Order = 12)]
        public int MisfireInstruction { get; set; }

        /// <summary>
        ///     触发器的优先级作为一个断路器，这样，如果两个触发器具有相同的定时触发时间，那么Quartz将尽其所能地给具有较高优先级的人第一次访问工作线程。
        /// </summary>
        [DataMember(Order = 13)]
        public int Priority { get; set; }

        /// <summary>
        ///     与触发器关联的状态数据。
        ///     在作业执行过程中对此映射所做的更改不重新保存， 而且实际上通常会导致非法状态。
        /// </summary>
        [DataMember(Order = 14)]
        public JobDataMapDto JobDataMap { get; set; }
    }
}