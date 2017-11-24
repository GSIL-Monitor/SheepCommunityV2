using ServiceStack.DataAnnotations;
using ServiceStack.Model;

namespace Sheep.Model.Geo.Entities
{
    /// <summary>
    ///     城市/区域。
    /// </summary>
    public class City : IHasStringId
    {
        /// <summary>
        ///     编号。
        /// </summary>
        [PrimaryKey]
        [StringLength(32)]
        public string Id { get; set; }

        /// <summary>
        ///     省份编号。
        /// </summary>
        [Required]
        [StringLength(32)]
        public string StateId { get; set; }

        /// <summary>
        ///     名称。
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}