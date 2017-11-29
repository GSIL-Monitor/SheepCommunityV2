using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sheep.Model.Content.Entities;

namespace Sheep.Model.Content
{
    /// <summary>
    ///     投票的存储库的接口定义。
    /// </summary>
    public interface IVoteRepository
    {
        #region 获取

        /// <summary>
        ///     根据上级与用户获取投票。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="userId">用户的用户编号。</param>
        /// <returns>投票。</returns>
        Vote GetVote(string parentId, int userId);

        /// <summary>
        ///     异步根据上级与用户获取投票。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="userId">用户的用户编号。</param>
        /// <returns>投票。</returns>
        Task<Vote> GetVoteAsync(string parentId, int userId);

        /// <summary>
        ///     根据上级查找投票。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>投票列表。</returns>
        List<Vote> FindVotesByParent(string parentId, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据上级查找投票。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>投票列表。</returns>
        Task<List<Vote>> FindVotesByParentAsync(string parentId, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     根据用户查找投票。
        /// </summary>
        /// <param name="userId">用户的用户编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>投票列表。</returns>
        List<Vote> FindVotesByUser(int userId, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据用户查找投票。
        /// </summary>
        /// <param name="userId">用户的用户编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>投票列表。</returns>
        Task<List<Vote>> FindVotesByUserAsync(int userId, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit);

        #endregion

        #region 统计

        /// <summary>
        ///     根据上级获取投票数量。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <returns>投票数量。</returns>
        int GetVotesCountByParent(string parentId, DateTime? createdSince, DateTime? modifiedSince);

        /// <summary>
        ///     异步根据上级获取投票数量。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <returns>投票数量。</returns>
        Task<int> GetVotesCountByParentAsync(string parentId, DateTime? createdSince, DateTime? modifiedSince);

        /// <summary>
        ///     根据用户获取投票数量。
        /// </summary>
        /// <param name="userId">用户的用户编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <returns>投票数量。</returns>
        int GetVotesCountByUser(int userId, DateTime? createdSince, DateTime? modifiedSince);

        /// <summary>
        ///     异步根据用户获取投票数量。
        /// </summary>
        /// <param name="userId">用户的用户编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <returns>投票数量。</returns>
        Task<int> GetVotesCountByUserAsync(int userId, DateTime? createdSince, DateTime? modifiedSince);

        #endregion

        #region 写入

        /// <summary>
        ///     创建一个新的投票。
        /// </summary>
        /// <param name="newVote">新的投票。</param>
        /// <returns>创建后的投票。</returns>
        Vote CreateVote(Vote newVote);

        /// <summary>
        ///     异步创建一个新的投票。
        /// </summary>
        /// <param name="newVote">新的投票。</param>
        /// <returns>创建后的投票。</returns>
        Task<Vote> CreateVoteAsync(Vote newVote);

        /// <summary>
        ///     更新一个投票。
        /// </summary>
        /// <param name="existingVote">原有的投票。</param>
        /// <param name="newVote">新的投票。</param>
        /// <returns>更新后的投票。</returns>
        Vote UpdateVote(Vote existingVote, Vote newVote);

        /// <summary>
        ///     异步更新一个投票。
        /// </summary>
        /// <param name="existingVote">原有的投票。</param>
        /// <param name="newVote">新的投票。</param>
        /// <returns>更新后的投票。</returns>
        Task<Vote> UpdateVoteAsync(Vote existingVote, Vote newVote);

        /// <summary>
        ///     取消一个投票。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="userId">用户的用户编号。</param>
        void DeleteVote(string parentId, int userId);

        /// <summary>
        ///     异步取消一个投票。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="userId">用户的用户编号。</param>
        Task DeleteVoteAsync(string parentId, int userId);

        #endregion
    }
}