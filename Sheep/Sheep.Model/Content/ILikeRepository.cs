using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sheep.Model.Content.Entities;

namespace Sheep.Model.Content
{
    /// <summary>
    ///     点赞的存储库的接口定义。
    /// </summary>
    public interface ILikeRepository
    {
        #region 获取

        /// <summary>
        ///     根据内容与用户获取点赞。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="userId">用户的用户编号。</param>
        /// <returns>点赞。</returns>
        Like GetLike(string parentId, int userId);

        /// <summary>
        ///     异步根据内容与用户获取点赞。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="userId">用户的用户编号。</param>
        /// <returns>点赞。</returns>
        Task<Like> GetLikeAsync(string parentId, int userId);

        /// <summary>
        ///     根据内容查找点赞。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>点赞列表。</returns>
        List<Like> FindLikesByParent(string parentId, DateTime? createdSince, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据内容查找点赞。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>点赞列表。</returns>
        Task<List<Like>> FindLikesByParentAsync(string parentId, DateTime? createdSince, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     根据用户查找点赞。
        /// </summary>
        /// <param name="userId">用户的用户编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>点赞列表。</returns>
        List<Like> FindLikesByUser(int userId, DateTime? createdSince, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据用户查找点赞。
        /// </summary>
        /// <param name="userId">用户的用户编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>点赞列表。</returns>
        Task<List<Like>> FindLikesByUserAsync(int userId, DateTime? createdSince, string orderBy, bool? descending, int? skip, int? limit);

        #endregion

        #region 统计

        /// <summary>
        ///     根据内容获取点赞数量。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <returns>点赞数量。</returns>
        int GetLikesCountByParent(string parentId, DateTime? createdSince);

        /// <summary>
        ///     异步根据内容获取点赞数量。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <returns>点赞数量。</returns>
        Task<int> GetLikesCountByParentAsync(string parentId, DateTime? createdSince);

        /// <summary>
        ///     根据用户获取点赞数量。
        /// </summary>
        /// <param name="userId">用户的用户编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <returns>点赞数量。</returns>
        int GetLikesCountByUser(int userId, DateTime? createdSince);

        /// <summary>
        ///     异步根据用户获取点赞数量。
        /// </summary>
        /// <param name="userId">用户的用户编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <returns>点赞数量。</returns>
        Task<int> GetLikesCountByUserAsync(int userId, DateTime? createdSince);

        #endregion

        #region 写入

        /// <summary>
        ///     创建一个新的点赞。
        /// </summary>
        /// <param name="newLike">新的点赞。</param>
        /// <returns>创建后的点赞。</returns>
        Like CreateLike(Like newLike);

        /// <summary>
        ///     异步创建一个新的点赞。
        /// </summary>
        /// <param name="newLike">新的点赞。</param>
        /// <returns>创建后的点赞。</returns>
        Task<Like> CreateLikeAsync(Like newLike);

        /// <summary>
        ///     取消一个点赞。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="userId">用户的用户编号。</param>
        void DeleteLike(string parentId, int userId);

        /// <summary>
        ///     异步取消一个点赞。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="userId">用户的用户编号。</param>
        Task DeleteLikeAsync(string parentId, int userId);

        #endregion
    }
}