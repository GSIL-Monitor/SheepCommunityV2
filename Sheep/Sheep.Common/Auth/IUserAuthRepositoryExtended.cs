using System;
using System.Collections.Generic;
using ServiceStack.Auth;

namespace Sheep.Common.Auth
{
    /// <summary>
    ///     扩展用户身份存储库的接口定义。
    /// </summary>
    public interface IUserAuthRepositoryExtended : IUserAuthRepository
    {
        IUserAuth GetUserAuthByDisplayName(string displayName);
        List<IUserAuth> FindUserAuths(string userNameFilter, string nameFilter, DateTime? createdSince, DateTime? modifiedSince, DateTime? lockedSince, string accountStatus, string orderBy, bool? descending, int? skip, int? limit);
        IUserAuthDetails GetUserAuthDetailsByProvider(string provider, string userId);
        void DeleteUserAuthDetailsByProvider(string provider, string userId);
    }
}