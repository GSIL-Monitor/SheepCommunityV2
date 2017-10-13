using System.Threading.Tasks;
using Sheep.Model.SecurityStamps.Entities;

namespace Sheep.Model.SecurityStamps
{
    /// <summary>
    ///     安全戳存储库的接口定义。
    /// </summary>
    public interface ISecurityStampRepository
    {
        #region 获取或创建

        /// <summary>
        ///     获取或创建一个安全戳。
        /// </summary>
        /// <param name="identifier">安全戳标识，表示手机号码或电子邮件地址。</param>
        /// <returns>安全戳对象。</returns>
        SecurityStamp GetSecurityStampStamp(string identifier);

        /// <summary>
        ///     获取或创建一个安全戳。
        /// </summary>
        /// <param name="identifier">安全戳标识，表示手机号码或电子邮件地址。</param>
        /// <returns>安全戳对象。</returns>
        Task<SecurityStamp> GetSecurityStampAsync(string identifier);

        #endregion
    }
}