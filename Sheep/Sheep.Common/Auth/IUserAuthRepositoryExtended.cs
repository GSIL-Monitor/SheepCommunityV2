using ServiceStack.Auth;

namespace Sheep.Common.Auth
{
    /// <summary>
    ///     扩展用户身份存储库的接口定义。
    /// </summary>
    public interface IUserAuthRepositoryExtended : IUserAuthRepository
    {
        /// <summary>
        ///     根据名称获取用户身份。
        /// </summary>
        /// <param name="displayName">显示名称。</param>
        /// <returns>用户身份。</returns>
        IUserAuth GetUserAuthByDisplayName(string displayName);

        /// <summary>
        ///     根据第三方提供者名称及第三方用户编号获取用户身份提供者明细。
        /// </summary>
        /// <param name="provider">第三方提供者名称。</param>
        /// <param name="userId">第三方用户编号。</param>
        /// <returns>用户身份提供者明细。</returns>
        IUserAuthDetails GetUserAuthDetailsByProvider(string provider, string userId);
    }
}