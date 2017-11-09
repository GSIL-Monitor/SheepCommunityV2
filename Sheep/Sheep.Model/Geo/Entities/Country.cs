using ServiceStack.DataAnnotations;

namespace Sheep.Model.Geo.Entities
{
    /// <summary>
    ///     国家/地区。
    /// </summary>
    public class Country
    {
        /// <summary>
        ///     编号。
        /// </summary>
        [PrimaryKey]
        [StringLength(32)]
        public string Id { get; set; }

        /// <summary>
        ///     名称。
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}