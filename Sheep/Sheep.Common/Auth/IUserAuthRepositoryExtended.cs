using ServiceStack.Auth;

namespace Sheep.Common.Auth
{
    /// <summary>
    ///     扩展用户身份存储库的接口定义。
    /// </summary>
    public interface IUserAuthRepositoryExtended : IUserAuthRepository
    {
        IUserAuth GetUserAuthByDisplayName(string displayName);
        IUserAuthDetails GetUserAuthDetailsByProvider(string provider, string userId);
        void DeleteUserAuthDetailsByProvider(string provider, string userId);
    }
}