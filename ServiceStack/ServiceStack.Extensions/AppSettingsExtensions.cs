using System.Collections.Generic;
using System.Linq;
using ServiceStack.Configuration;
using ServiceStack.Text;

namespace ServiceStack.Extensions
{
    /// <summary>
    ///     应用程序设置的扩展。
    /// </summary>
    public static class AppSettingsExtensions
    {
        #region 查找

        /// <summary>
        ///     返回以特定字符串开头的所有应用程序设置。
        /// </summary>
        /// <param name="settings">应用程序的设置。</param>
        /// <param name="key">要查找的部分键。</param>
        /// <returns>匹配设置的字典。</returns>
        public static Dictionary<string, string> GetAllKeysStartingWith(this IAppSettings settings, string key)
        {
            return settings.GetAll().Where(x => x.Key.StartsWithIgnoreCase(key)).ToStringDictionary();
        }

        #endregion
    }
}