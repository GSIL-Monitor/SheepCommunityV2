using System.Runtime.Serialization;

namespace Sheep.ServiceModel.Views.Entities
{
    /// <summary>
    ///     查看数量信息。
    /// </summary>
    [DataContract]
    public class ViewCountsDto
    {
        /// <summary>
        ///     查看次数。
        /// </summary>
        [DataMember(Order = 1)]
        public int ViewsCount { get; set; }

        /// <summary>
        ///     上级对象数量。
        /// </summary>
        [DataMember(Order = 2)]
        public int ParentsCount { get; set; }

        /// <summary>
        ///     天数。
        /// </summary>
        [DataMember(Order = 3)]
        public int DaysCount { get; set; }
    }
}