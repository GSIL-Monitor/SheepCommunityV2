using System.Runtime.Serialization;
using ServiceStack.Model;

namespace Sheep.ServiceModel.Countries.Entities
{
    /// <summary>
    ///     国家/地区信息。
    /// </summary>
    [DataContract]
    public class CountryDto : IHasStringId
    {
        /// <summary>
        ///     编号。
        /// </summary>
        [DataMember(Order = 1)]
        public string Id { get; set; }

        /// <summary>
        ///     名称。
        /// </summary>
        [DataMember(Order = 2)]
        public string Name { get; set; }
    }
}