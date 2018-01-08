using System.Runtime.Serialization;

namespace ServiceStack.Quartz.Services.Models.Entities
{
    /// <summary>
    ///     作业主键信息。
    /// </summary>
    [DataContract]
    public class JobKeyDto
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