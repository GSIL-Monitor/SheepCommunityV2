using System.Text;
using ServiceStack.DataAnnotations;

namespace Sheep.Model.SecurityStamps.Entities
{
    /// <summary>
    ///     安全戳。
    /// </summary>
    public class SecurityStamp
    {
        /// <summary>
        ///     安全戳标识，表示手机号码或电子邮件地址。
        /// </summary>
        [PrimaryKey]
        [StringLength(128)]
        public string Identifier { get; set; }

        /// <summary>
        ///     随机安全戳。
        /// </summary>
        [Required]
        public string Stamp { get; set; }

        /// <summary>
        ///     转换为字节数组。
        /// </summary>
        public byte[] ToSecurityToken()
        {
            return Encoding.Unicode.GetBytes(Stamp);
        }
    }
}