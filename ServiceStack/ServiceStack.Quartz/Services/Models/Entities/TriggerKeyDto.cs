using System.Runtime.Serialization;

namespace ServiceStack.Quartz.Services.Models.Entities
{
    /// <summary>
    ///     触发器主键信息。
    /// </summary>
    [DataContract]
    public class TriggerKeyDto
    {
        /// <summary>
        ///     分组。
        /// </summary>
        [DataMember(Order = 1)]
        public string Group { get; set; }

        /// <summary>
        ///     名称。
        /// </summary>
        [DataMember(Order = 2)]
        public string Name { get; set; }
    }
}