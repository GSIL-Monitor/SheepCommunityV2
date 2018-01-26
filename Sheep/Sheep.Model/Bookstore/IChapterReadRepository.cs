using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sheep.Model.Bookstore.Entities;

namespace Sheep.Model.Bookstore
{
    /// <summary>
    ///     章阅读的存储库的接口定义。
    /// </summary>
    public interface IChapterReadRepository
    {
        #region 获取

        /// <summary>
        ///     根据编号获取章阅读。
        /// </summary>
        /// <param name="readId">章阅读的编号。</param>
        /// <returns>章阅读。</returns>
        ChapterRead GetChapterRead(string readId);

        /// <summary>
        ///     异步根据编号获取章阅读。
        /// </summary>
        /// <param name="readId">章阅读的编号。</param>
        /// <returns>章阅读。</returns>
        Task<ChapterRead> GetChapterReadAsync(string readId);

        /// <summary>
        ///     根据章查找章阅读。
        /// </summary>
        /// <param name="chapterId">章的编号。</param>
        /// <param name="userId">用户的编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>章阅读列表。</returns>
        List<ChapterRead> FindChapterReadsByChapter(string chapterId, int? userId, DateTime? createdSince, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据章查找章阅读。
        /// </summary>
        /// <param name="chapterId">章的编号。</param>
        /// <param name="userId">用户的编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>章阅读列表。</returns>
        Task<List<ChapterRead>> FindChapterReadsByChapterAsync(string chapterId, int? userId, DateTime? createdSince, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     根据用户查找章阅读。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>章阅读列表。</returns>
        List<ChapterRead> FindChapterReadsByUser(int userId, string bookId, DateTime? createdSince, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据用户查找章阅读。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>章阅读列表。</returns>
        Task<List<ChapterRead>> FindChapterReadsByUserAsync(int userId, string bookId, DateTime? createdSince, string orderBy, bool? descending, int? skip, int? limit);

        #endregion

        #region 统计

        /// <summary>
        ///     根据章获取章阅读次数。
        /// </summary>
        /// <param name="chapterId">上级的编号。（如帖子编号）</param>
        /// <param name="userId">用户的编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <returns>章阅读次数。</returns>
        int GetChapterReadsCountByChapter(string chapterId, int? userId, DateTime? createdSince);

        /// <summary>
        ///     异步根据章获取章阅读次数。
        /// </summary>
        /// <param name="chapterId">上级的编号。（如帖子编号）</param>
        /// <param name="userId">用户的编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <returns>章阅读次数。</returns>
        Task<int> GetChapterReadsCountByChapterAsync(string chapterId, int? userId, DateTime? createdSince);

        /// <summary>
        ///     根据用户获取章阅读次数。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <returns>章阅读次数。</returns>
        int GetChapterReadsCountByUser(int userId, string bookId, DateTime? createdSince);

        /// <summary>
        ///     异步根据用户获取章阅读次数。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <returns>章阅读次数。</returns>
        Task<int> GetChapterReadsCountByUserAsync(int userId, string bookId, DateTime? createdSince);

        #endregion

        #region 写入

        /// <summary>
        ///     创建一个新的章阅读。
        /// </summary>
        /// <param name="newChapterRead">新的章阅读。</param>
        /// <returns>创建后的章阅读。</returns>
        ChapterRead CreateChapterRead(ChapterRead newChapterRead);

        /// <summary>
        ///     异步创建一个新的章阅读。
        /// </summary>
        /// <param name="newChapterRead">新的章阅读。</param>
        /// <returns>创建后的章阅读。</returns>
        Task<ChapterRead> CreateChapterReadAsync(ChapterRead newChapterRead);

        /// <summary>
        ///     取消一个章阅读。
        /// </summary>
        /// <param name="readId">章阅读的编号。</param>
        void DeleteChapterRead(string readId);

        /// <summary>
        ///     异步取消一个章阅读。
        /// </summary>
        /// <param name="readId">章阅读的编号。</param>
        Task DeleteChapterReadAsync(string readId);

        #endregion
    }
}