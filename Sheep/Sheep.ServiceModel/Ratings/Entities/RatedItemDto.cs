using System.Runtime.Serialization;

namespace Sheep.ServiceModel.Ratings.Entities
{
    /// <summary>
    ///     被评分对象信息。
    /// </summary>
    [DataContract]
    public class RatedItemDto
    {
        ///// <summary>
        /////     被评分对象相关的内容。
        ///// </summary>
        //[DataMember(Order = 2)]
        //public ContentDto Content { get; set; }

        /// <summary>
        ///     评分的数量。
        /// </summary>
        [DataMember(Order = 3)]
        public int RatingsCount { get; set; }

        /// <summary>
        ///     评分的平均值。
        /// </summary>
        [DataMember(Order = 4)]
        public float? AverageValue { get; set; }
    }
}