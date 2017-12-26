using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sheep.Model.Content.Entities;

namespace Sheep.Model.Content
{
    /// <summary>
    ///     收藏的存储库的接口定义。
    /// </summary>
    public interface IBookmarkRepository
    {
        #region 获取

        /// <summary>
        ///     根据上级与用户获取收藏。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="userId">用户的编号。</param>
        /// <returns>收藏。</returns>
        Bookmark GetBookmark(string parentId, int userId);

        /// <summary>
        ///     异步根据上级与用户获取收藏。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="userId">用户的编号。</param>
        /// <returns>收藏。</returns>
        Task<Bookmark> GetBookmarkAsync(string parentId, int userId);

        /// <summary>
        ///     根据上级查找收藏。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>收藏列表。</returns>
        List<Bookmark> FindBookmarksByParent(string parentId, DateTime? createdSince, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据上级查找收藏。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>收藏列表。</returns>
        Task<List<Bookmark>> FindBookmarksByParentAsync(string parentId, DateTime? createdSince, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     根据用户查找收藏。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>收藏列表。</returns>
        List<Bookmark> FindBookmarksByUser(int userId, string parentType, DateTime? createdSince, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据用户查找收藏。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>收藏列表。</returns>
        Task<List<Bookmark>> FindBookmarksByUserAsync(int userId, string parentType, DateTime? createdSince, string orderBy, bool? descending, int? skip, int? limit);

        #endregion

        #region 统计

        /// <summary>
        ///     根据上级获取收藏数量。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <returns>收藏数量。</returns>
        int GetBookmarksCountByParent(string parentId, DateTime? createdSince);

        /// <summary>
        ///     异步根据上级获取收藏数量。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <returns>收藏数量。</returns>
        Task<int> GetBookmarksCountByParentAsync(string parentId, DateTime? createdSince);

        /// <summary>
        ///     根据用户获取收藏数量。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <returns>收藏数量。</returns>
        int GetBookmarksCountByUser(int userId, string parentType, DateTime? createdSince);

        /// <summary>
        ///     异步根据用户获取收藏数量。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <returns>收藏数量。</returns>
        Task<int> GetBookmarksCountByUserAsync(int userId, string parentType, DateTime? createdSince);

        #endregion

        #region 写入

        /// <summary>
        ///     创建一个新的收藏。
        /// </summary>
        /// <param name="newBookmark">新的收藏。</param>
        /// <returns>创建后的收藏。</returns>
        Bookmark CreateBookmark(Bookmark newBookmark);

        /// <summary>
        ///     异步创建一个新的收藏。
        /// </summary>
        /// <param name="newBookmark">新的收藏。</param>
        /// <returns>创建后的收藏。</returns>
        Task<Bookmark> CreateBookmarkAsync(Bookmark newBookmark);

        /// <summary>
        ///     创建一组新的收藏。
        /// </summary>
        /// <param name="newBookmarks">一组新的收藏。</param>
        void CreateBookmarks(List<Bookmark> newBookmarks);

        /// <summary>
        ///     异步创建一组新的收藏。
        /// </summary>
        /// <param name="newBookmarks">一组新的收藏。</param>
        Task CreateBookmarksAsync(List<Bookmark> newBookmarks);

        /// <summary>
        ///     取消一个收藏。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="userId">用户的编号。</param>
        void DeleteBookmark(string parentId, int userId);

        /// <summary>
        ///     异步取消一个收藏。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="userId">用户的编号。</param>
        Task DeleteBookmarkAsync(string parentId, int userId);

        #endregion
    }
}