using ServiceStack.DataAnnotations;

namespace Sheep.Model.Geo.Entities
{
    /// <summary>
    ///     省份/直辖市/州。
    /// </summary>
    public class State
    {
        /// <summary>
        ///     编号。
        /// </summary>
        [PrimaryKey]
        [StringLength(32)]
        public string Id { get; set; }

        /// <summary>
        ///     国家编号。
        /// </summary>
        [Required]
        [StringLength(32)]
        public string CountryId { get; set; }

        /// <summary>
        ///     名称。
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}