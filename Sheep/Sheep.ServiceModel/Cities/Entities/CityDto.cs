using System.Runtime.Serialization;
using ServiceStack.Model;

namespace Sheep.ServiceModel.Cities.Entities
{
    /// <summary>
    ///     城市/区域信息。
    /// </summary>
    [DataContract]
    public class CityDto : IHasStringId
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