using System.Runtime.Serialization;
using ServiceStack.Model;

namespace Sheep.ServiceModel.States.Entities
{
    /// <summary>
    ///     省份/直辖市/州信息。
    /// </summary>
    [DataContract]
    public class StateDto : IHasStringId
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