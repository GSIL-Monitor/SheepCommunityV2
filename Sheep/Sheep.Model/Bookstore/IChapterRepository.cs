using System.Collections.Generic;
using System.Threading.Tasks;
using Sheep.Model.Bookstore.Entities;

namespace Sheep.Model.Bookstore
{
    /// <summary>
    ///     章的存储库的接口定义。
    /// </summary>
    public interface IChapterRepository
    {
        #region 获取

        /// <summary>
        ///     获取章。
        /// </summary>
        /// <param name="chapterId">章的编号。</param>
        /// <returns>章。</returns>
        Chapter GetChapter(string chapterId);

        /// <summary>
        ///     异步获取章。
        /// </summary>
        /// <param name="chapterId">章的编号。</param>
        /// <returns>章。</returns>
        Task<Chapter> GetChapterAsync(string chapterId);

        /// <summary>
        ///     获取章列表。
        /// </summary>
        /// <param name="chapterIds">章的编号列表。</param>
        /// <returns>章。</returns>
        List<Chapter> GetChapters(List<string> chapterIds);

        /// <summary>
        ///     异步获取章列表。
        /// </summary>
        /// <param name="chapterIds">章的编号列表。</param>
        /// <returns>章。</returns>
        Task<List<Chapter>> GetChaptersAsync(List<string> chapterIds);

        /// <summary>
        ///     根据卷及序号获取章。
        /// </summary>
        /// <param name="volumeId">卷的编号。</param>
        /// <param name="number">序号。</param>
        /// <returns>章。</returns>
        Chapter GetChapter(string volumeId, int number);

        /// <summary>
        ///     异步根据卷及序号获取章。
        /// </summary>
        /// <param name="volumeId">卷的编号。</param>
        /// <param name="number">序号。</param>
        /// <returns>章。</returns>
        Task<Chapter> GetChapterAsync(string volumeId, int number);

        /// <summary>
        ///     根据书籍及卷序号及序号获取章。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="volumeNumber">卷的编号。</param>
        /// <param name="number">序号。</param>
        /// <returns>章。</returns>
        Chapter GetChapter(string bookId, int volumeNumber, int number);

        /// <summary>
        ///     异步根据书籍及卷序号及序号获取章。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="volumeNumber">卷的编号。</param>
        /// <param name="number">序号。</param>
        /// <returns>章。</returns>
        Task<Chapter> GetChapterAsync(string bookId, int volumeNumber, int number);

        /// <summary>
        ///     查找章。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="volumeNumber">卷的编号。</param>
        /// <param name="contentFilter">过滤内容的表达式。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>章列表。</returns>
        List<Chapter> FindChapters(string bookId, int? volumeNumber, string contentFilter, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步查找章。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="volumeNumber">卷的编号。</param>
        /// <param name="contentFilter">过滤内容的表达式。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>章列表。</returns>
        Task<List<Chapter>> FindChaptersAsync(string bookId, int? volumeNumber, string contentFilter, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     根据卷查找章。
        /// </summary>
        /// <param name="volumeId">卷的编号。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>章列表。</returns>
        List<Chapter> FindChaptersByVolume(string volumeId, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据卷查找章。
        /// </summary>
        /// <param name="volumeId">卷的编号。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>章列表。</returns>
        Task<List<Chapter>> FindChaptersByVolumeAsync(string volumeId, string orderBy, bool? descending, int? skip, int? limit);

        #endregion

        #region 统计

        /// <summary>
        ///     查找获取章数量。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="volumeNumber">卷的编号。</param>
        /// <param name="contentFilter">过滤内容的表达式。</param>
        /// <returns>章数量。</returns>
        int GetChaptersCount(string bookId, int? volumeNumber, string contentFilter);

        /// <summary>
        ///     异步获取章数量。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="volumeNumber">卷的编号。</param>
        /// <param name="contentFilter">过滤内容的表达式。</param>
        /// <returns>章数量。</returns>
        Task<int> GetChaptersCountAsync(string bookId, int? volumeNumber, string contentFilter);

        /// <summary>
        ///     根据卷获取章数量。
        /// </summary>
        /// <param name="volumeId">卷的编号。</param>
        /// <returns>章数量。</returns>
        int GetChaptersCountByVolume(string volumeId);

        /// <summary>
        ///     异步根据卷获取章数量。
        /// </summary>
        /// <param name="volumeId">卷的编号。</param>
        /// <returns>章数量。</returns>
        Task<int> GetChaptersCountByVolumeAsync(string volumeId);

        #endregion

        #region 写入

        /// <summary>
        ///     创建一个新的章。
        /// </summary>
        /// <param name="newChapter">新的章。</param>
        /// <returns>创建后的章。</returns>
        Chapter CreateChapter(Chapter newChapter);

        /// <summary>
        ///     异步创建一个新的章。
        /// </summary>
        /// <param name="newChapter">新的章。</param>
        /// <returns>创建后的章。</returns>
        Task<Chapter> CreateChapterAsync(Chapter newChapter);

        /// <summary>
        ///     更新一个章。
        /// </summary>
        /// <param name="existingChapter">原有的章。</param>
        /// <param name="newChapter">新的章。</param>
        /// <returns>更新后的章。</returns>
        Chapter UpdateChapter(Chapter existingChapter, Chapter newChapter);

        /// <summary>
        ///     异步更新一个章。
        /// </summary>
        /// <param name="existingChapter">原有的章。</param>
        /// <param name="newChapter">新的章。</param>
        /// <returns>更新后的章。</returns>
        Task<Chapter> UpdateChapterAsync(Chapter existingChapter, Chapter newChapter);

        /// <summary>
        ///     删除一个章。
        /// </summary>
        /// <param name="chapterId">章的编号。</param>
        void DeleteChapter(string chapterId);

        /// <summary>
        ///     异步删除一个章。
        /// </summary>
        /// <param name="chapterId">章的编号。</param>
        Task DeleteChapterAsync(string chapterId);

        /// <summary>
        ///     增加一个章的节数。
        /// </summary>
        /// <param name="chapterId">章的编号。</param>
        /// <param name="count">增加的数量。</param>
        void IncrementChapterParagraphsCount(string chapterId, int count);

        /// <summary>
        ///     异步增加一个章的节数。
        /// </summary>
        /// <param name="chapterId">章的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task IncrementChapterParagraphsCountAsync(string chapterId, int count);

        /// <summary>
        ///     增加一个章的查看的次数。
        /// </summary>
        /// <param name="chapterId">章的编号。</param>
        /// <param name="count">增加的数量。</param>
        void IncrementChapterViewsCount(string chapterId, int count);

        /// <summary>
        ///     异步增加一个章的查看的次数。
        /// </summary>
        /// <param name="chapterId">章的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task IncrementChapterViewsCountAsync(string chapterId, int count);

        /// <summary>
        ///     增加一个章的收藏的次数。
        /// </summary>
        /// <param name="chapterId">章的编号。</param>
        /// <param name="count">增加的数量。</param>
        void IncrementChapterBookmarksCount(string chapterId, int count);

        /// <summary>
        ///     异步增加一个章的收藏的次数。
        /// </summary>
        /// <param name="chapterId">章的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task IncrementChapterBookmarksCountAsync(string chapterId, int count);

        /// <summary>
        ///     增加一个章的评论的次数。
        /// </summary>
        /// <param name="chapterId">章的编号。</param>
        /// <param name="count">增加的数量。</param>
        void IncrementChapterCommentsCount(string chapterId, int count);

        /// <summary>
        ///     异步增加一个章的评论的次数。
        /// </summary>
        /// <param name="chapterId">章的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task IncrementChapterCommentsCountAsync(string chapterId, int count);

        /// <summary>
        ///     增加一个章的点赞的次数。
        /// </summary>
        /// <param name="chapterId">章的编号。</param>
        /// <param name="count">增加的数量。</param>
        void IncrementChapterLikesCount(string chapterId, int count);

        /// <summary>
        ///     异步增加一个章的点赞的次数。
        /// </summary>
        /// <param name="chapterId">章的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task IncrementChapterLikesCountAsync(string chapterId, int count);

        /// <summary>
        ///     增加一个章的评分的次数。
        /// </summary>
        /// <param name="chapterId">章的编号。</param>
        /// <param name="count">增加的数量。</param>
        void IncrementChapterRatingsCount(string chapterId, int count);

        /// <summary>
        ///     异步增加一个章的评分的次数。
        /// </summary>
        /// <param name="chapterId">章的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task IncrementChapterRatingsCountAsync(string chapterId, int count);

        /// <summary>
        ///     增加一个章的分享的次数。
        /// </summary>
        /// <param name="chapterId">章的编号。</param>
        /// <param name="count">增加的数量。</param>
        void IncrementChapterSharesCount(string chapterId, int count);

        /// <summary>
        ///     异步增加一个章的分享的次数。
        /// </summary>
        /// <param name="chapterId">章的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task IncrementChapterSharesCountAsync(string chapterId, int count);

        /// <summary>
        ///     更新一个章的评分的平均值。
        /// </summary>
        /// <param name="chapterId">章的编号。</param>
        /// <param name="value">更新的数值。</param>
        void UpdateChapterRatingsAverageValue(string chapterId, float value);

        /// <summary>
        ///     异步更新一个章的评分的平均值。
        /// </summary>
        /// <param name="chapterId">章的编号。</param>
        /// <param name="value">更新的数值。</param>
        Task UpdateChapterRatingsAverageValueAsync(string chapterId, float value);

        #endregion
    }
}