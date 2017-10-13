using System.Threading.Tasks;

namespace Sheep.Model.SecurityTokens
{
    /// <summary>
    ///     安全令牌提供程序的接口定义。
    /// </summary>
    public interface ISecurityTokenProvider
    {
        #region 生成令牌

        /// <summary>
        ///     生成特定用途的令牌。
        /// </summary>
        /// <param name="target">手机号码或电子邮件地址。</param>
        /// <param name="purpose">特定的用途。</param>
        /// <returns>令牌。</returns>
        string GenerateToken(string target, string purpose);

        /// <summary>
        ///     生成特定用途的令牌。
        /// </summary>
        /// <param name="target">手机号码或电子邮件地址。</param>
        /// <param name="purpose">特定的用途。</param>
        /// <returns>令牌。</returns>
        Task<string> GenerateTokenAsync(string target, string purpose);

        #endregion

        #region 校验令牌

        /// <summary>
        ///     校验指定用途的用户令牌。
        /// </summary>
        /// <param name="target">手机号码或电子邮件地址。</param>
        /// <param name="purpose">特定的用途。</param>
        /// <param name="token">令牌。</param>
        /// <returns>true 表示校验成功，否则为 false。</returns>
        bool VerifyToken(string target, string purpose, string token);

        /// <summary>
        ///     校验指定用途的用户令牌。
        /// </summary>
        /// <param name="target">手机号码或电子邮件地址。</param>
        /// <param name="purpose">特定的用途。</param>
        /// <param name="token">令牌。</param>
        /// <returns>true 表示校验成功，否则为 false。</returns>
        Task<bool> VerifyTokenAsync(string target, string purpose, string token);

        #endregion

        #region 发送令牌

        /// <summary>
        ///     通知用户令牌已经生成，可以发送短信或邮件。
        /// </summary>
        /// <param name="target">手机号码或电子邮件地址。</param>
        /// <param name="signature">签名。</param>
        /// <param name="token">令牌。</param>
        /// <param name="template">模板的编号或内容。</param>
        /// <returns>true 表示发送成功，否则为 false。</returns>
        bool SendToken(string target, string signature, string token, string template);

        /// <summary>
        ///     通知用户令牌已经生成，可以发送短信或邮件。
        /// </summary>
        /// <param name="target">手机号码或电子邮件地址。</param>
        /// <param name="signature">签名。</param>
        /// <param name="token">令牌。</param>
        /// <param name="template">模板的编号或内容。</param>
        /// <returns>true 表示发送成功，否则为 false。</returns>
        Task<bool> SendTokenAsync(string target, string signature, string token, string template);

        #endregion
    }
}