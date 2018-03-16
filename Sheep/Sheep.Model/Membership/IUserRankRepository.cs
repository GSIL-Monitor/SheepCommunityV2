using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sheep.Model.Membership.Entities;

namespace Sheep.Model.Membership
{
    /// <summary>
    ///     用户排行的存储库的接口定义。
    /// </summary>
    public interface IUserRankRepository
    {
        #region 获取

        /// <summary>
        ///     获取用户排行。
        /// </summary>
        /// <param name="userId">用户编号。</param>
        /// <returns>用户排行。</returns>
        UserRank GetUserRank(int userId);

        /// <summary>
        ///     异步获取用户排行。
        /// </summary>
        /// <param name="userId">用户编号。</param>
        /// <returns>用户排行。</returns>
        Task<UserRank> GetUserRankAsync(int userId);

        /// <summary>
        ///     查找用户排行。
        /// </summary>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>用户排行列表。</returns>
        List<UserRank> FindUserRanks(DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步查找用户排行。
        /// </summary>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>用户排行列表。</returns>
        Task<List<UserRank>> FindUserRanksAsync(DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit);

        #endregion

        #region 写入

        /// <summary>
        ///     创建一个新的用户排行。
        /// </summary>
        /// <param name="newUserRank">新的用户排行。</param>
        /// <returns>创建后的用户排行。</returns>
        UserRank CreateUserRank(UserRank newUserRank);

        /// <summary>
        ///     异步创建一个新的用户排行。
        /// </summary>
        /// <param name="newUserRank">新的用户排行。</param>
        /// <returns>创建后的用户排行。</returns>
        Task<UserRank> CreateUserRankAsync(UserRank newUserRank);

        /// <summary>
        ///     更新一个用户排行。
        /// </summary>
        /// <param name="existingUserRank">原有的用户排行。</param>
        /// <param name="newUserRank">新的用户排行。</param>
        /// <returns>更新后的用户排行。</returns>
        UserRank UpdateUserRank(UserRank existingUserRank, UserRank newUserRank);

        /// <summary>
        ///     异步更新一个用户排行。
        /// </summary>
        /// <param name="existingUserRank">原有的用户排行。</param>
        /// <param name="newUserRank">新的用户排行。</param>
        /// <returns>更新后的用户排行。</returns>
        Task<UserRank> UpdateUserRankAsync(UserRank existingUserRank, UserRank newUserRank);

        /// <summary>
        ///     删除一个用户排行。
        /// </summary>
        /// <param name="userId">用户编号。</param>
        void DeleteUserRank(int userId);

        /// <summary>
        ///     异步删除一个用户排行。
        /// </summary>
        /// <param name="userId">用户编号。</param>
        Task DeleteUserRankAsync(int userId);

        #endregion
    }
}