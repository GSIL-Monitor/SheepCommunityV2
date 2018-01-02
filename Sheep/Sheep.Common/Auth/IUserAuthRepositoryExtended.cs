using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceStack.Auth;

namespace Sheep.Common.Auth
{
    /// <summary>
    ///     扩展用户身份存储库的接口定义。
    /// </summary>
    public interface IUserAuthRepositoryExtended : IUserAuthRepository
    {
        IUserAuth GetUserAuthByDisplayName(string displayName);
        List<IUserAuth> FindUserAuths(string userNameFilter, string nameFilter, DateTime? createdSince, DateTime? modifiedSince, DateTime? lockedSince, string status, string orderBy, bool? descending, int? skip, int? limit);
        IUserAuthDetails GetUserAuthDetailsByProvider(string provider, string userId);
        void DeleteUserAuthDetailsByProvider(string provider, string userId);
        Task<IUserAuth> CreateUserAuthAsync(IUserAuth newUserAuth, string password);
        Task<IUserAuth> UpdateUserAuthAsync(IUserAuth existingUserAuth, IUserAuth newUserAuth);
        Task<IUserAuth> UpdateUserAuthAsync(IUserAuth existingUserAuth, IUserAuth newUserAuth, string password);
        Task<IUserAuth> GetUserAuthAsync(IAuthSession session, IAuthTokens tokens);
        Task<IUserAuth> GetUserAuthAsync(string userAuthId);
        Task<IUserAuth> GetUserAuthByUserNameAsync(string userNameOrEmail);
        Task<IUserAuth> GetUserAuthByDisplayNameAsync(string displayName);
        Task<List<IUserAuth>> GetUserAuthsAsync(List<string> userAuthIds);
        Task<List<IUserAuth>> FindUserAuthsAsync(List<string> userAuthIds, DateTime? createdSince, DateTime? modifiedSince, DateTime? lockedSince, string status, string orderBy, bool? descending, int? skip, int? limit);
        Task<List<IUserAuth>> FindUserAuthsAsync(string userNameFilter, string nameFilter, DateTime? createdSince, DateTime? modifiedSince, DateTime? lockedSince, string status, string orderBy, bool? descending, int? skip, int? limit);
        Task DeleteUserAuthAsync(string userAuthId);
        Task<IUserAuthDetails> GetUserAuthDetailsByProviderAsync(string provider, string userId);
        Task<List<IUserAuthDetails>> GetUserAuthDetailsAsync(string userAuthId);
        Task DeleteUserAuthDetailsByProviderAsync(string provider, string userId);
    }
}