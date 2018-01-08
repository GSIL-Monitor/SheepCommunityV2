using System.Runtime.Serialization;

namespace ServiceStack.Quartz.Services.Models.Entities
{
    /// <summary>
    ///     保存作业实例的状态数据信息。
    /// </summary>
    [DataContract]
    public class JobDataMapDto
    {
        /// <summary>
        ///     状态数据。
        /// </summary>
        [DataMember(Order = 1)]
        public string Data { get; set; }

        /// <summary>
        ///     包含的元素数。
        /// </summary>
        [DataMember(Order = 2)]
        public int Count { get; set; }

        /// <summary>
        ///     获取指示此实例是否为空的值。
        /// </summary>
        [DataMember(Order = 3)]
        public bool IsEmpty { get; set; }

        /// <summary>
        ///     确定 IDictionary 是否已标记为脏。
        /// </summary>
        [DataMember(Order = 4)]
        public bool Dirty { get; set; }
    }
}