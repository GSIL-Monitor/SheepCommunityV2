using System.Collections.Generic;
using System.Threading.Tasks;
using Sheep.Model.Bookstore.Entities;

namespace Sheep.Model.Bookstore
{
    /// <summary>
    ///     节的存储库的接口定义。
    /// </summary>
    public interface IParagraphRepository
    {
        #region 获取

        /// <summary>
        ///     获取节。
        /// </summary>
        /// <param name="paragraphId">节的编号。</param>
        /// <returns>节。</returns>
        Paragraph GetParagraph(string paragraphId);

        /// <summary>
        ///     异步获取节。
        /// </summary>
        /// <param name="paragraphId">节的编号。</param>
        /// <returns>节。</returns>
        Task<Paragraph> GetParagraphAsync(string paragraphId);

        /// <summary>
        ///     获取节列表。
        /// </summary>
        /// <param name="paragraphIds">节的编号列表。</param>
        /// <returns>节。</returns>
        List<Paragraph> GetParagraphs(IEnumerable<string> paragraphIds);

        /// <summary>
        ///     异步获取节列表。
        /// </summary>
        /// <param name="paragraphIds">节的编号列表。</param>
        /// <returns>节。</returns>
        Task<List<Paragraph>> GetParagraphsAsync(IEnumerable<string> paragraphIds);

        /// <summary>
        ///     根据章及序号获取节。
        /// </summary>
        /// <param name="chapterId">章的编号。</param>
        /// <param name="number">序号。</param>
        /// <returns>节。</returns>
        Paragraph GetParagraph(string chapterId, int number);

        /// <summary>
        ///     异步根据章及序号获取节。
        /// </summary>
        /// <param name="chapterId">章的编号。</param>
        /// <param name="number">序号。</param>
        /// <returns>节。</returns>
        Task<Paragraph> GetParagraphAsync(string chapterId, int number);

        /// <summary>
        ///     根据书籍及章序号及序号获取节。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="volumeNumber">卷的编号。</param>
        /// <param name="chapterNumber">章的编号。</param>
        /// <param name="number">序号。</param>
        /// <returns>节。</returns>
        Paragraph GetParagraph(string bookId, int volumeNumber, int chapterNumber, int number);

        /// <summary>
        ///     异步根据书籍及章序号及序号获取节。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="volumeNumber">卷的编号。</param>
        /// <param name="chapterNumber">章的编号。</param>
        /// <param name="number">序号。</param>
        /// <returns>节。</returns>
        Task<Paragraph> GetParagraphAsync(string bookId, int volumeNumber, int chapterNumber, int number);

        /// <summary>
        ///     查找节。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="volumeNumber">卷的编号。</param>
        /// <param name="chapterNumber">章的编号。</param>
        /// <param name="contentFilter">过滤内容的表达式。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>节列表。</returns>
        List<Paragraph> FindParagraphs(string bookId, int? volumeNumber, int?chapterNumber, string contentFilter, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步查找节。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="volumeNumber">卷的编号。</param>
        /// <param name="chapterNumber">章的编号。</param>
        /// <param name="contentFilter">过滤内容的表达式。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>节列表。</returns>
        Task<List<Paragraph>> FindParagraphsAsync(string bookId, int? volumeNumber, int? chapterNumber, string contentFilter, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     根据章查找节。
        /// </summary>
        /// <param name="chapterId">章的编号。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>节列表。</returns>
        List<Paragraph> FindParagraphsByChapter(string chapterId, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据章查找节。
        /// </summary>
        /// <param name="chapterId">章的编号。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>节列表。</returns>
        Task<List<Paragraph>> FindParagraphsByChapterAsync(string chapterId, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     根据章列表查找节。
        /// </summary>
        /// <param name="chapterIds">章的编号列表。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>节列表。</returns>
        List<Paragraph> FindParagraphsByChapters(IEnumerable<string> chapterIds, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据章列表查找节。
        /// </summary>
        /// <param name="chapterIds">章的编号列表。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>节列表。</returns>
        Task<List<Paragraph>> FindParagraphsByChaptersAsync(IEnumerable<string> chapterIds, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     根据主题查找节。
        /// </summary>
        /// <param name="subjectId">主题的编号。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>节列表。</returns>
        List<Paragraph> FindParagraphsBySubject(string subjectId, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据主题查找节。
        /// </summary>
        /// <param name="subjectId">主题的编号。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>节列表。</returns>
        Task<List<Paragraph>> FindParagraphsBySubjectAsync(string subjectId, string orderBy, bool? descending, int? skip, int? limit);

        #endregion

        #region 统计

        /// <summary>
        ///     查找获取节数量。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="volumeNumber">卷的编号。</param>
        /// <param name="chapterNumber">章的编号。</param>
        /// <param name="contentFilter">过滤内容的表达式。</param>
        /// <returns>节数量。</returns>
        int GetParagraphsCount(string bookId, int? volumeNumber, int? chapterNumber, string contentFilter);

        /// <summary>
        ///     异步获取节数量。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="volumeNumber">卷的编号。</param>
        /// <param name="chapterNumber">章的编号。</param>
        /// <param name="contentFilter">过滤内容的表达式。</param>
        /// <returns>节数量。</returns>
        Task<int> GetParagraphsCountAsync(string bookId, int? volumeNumber, int? chapterNumber, string contentFilter);

        /// <summary>
        ///     根据章查找节。
        /// </summary>
        /// <param name="chapterId">章的编号。</param>
        /// <returns>节数量。</returns>
        int GetParagraphsCountByChapter(string chapterId);

        /// <summary>
        ///     异步根据章获取节数量。
        /// </summary>
        /// <param name="chapterId">章的编号。</param>
        /// <returns>节数量。</returns>
        Task<int> GetParagraphsCountByChapterAsync(string chapterId);

        /// <summary>
        ///     根据主题获取节数量。
        /// </summary>
        /// <param name="subjectId">主题的编号。</param>
        /// <returns>节数量。</returns>
        int GetParagraphsCountBySubject(string subjectId);

        /// <summary>
        ///     异步根据主题获取节数量。
        /// </summary>
        /// <param name="subjectId">主题的编号。</param>
        /// <returns>节数量。</returns>
        Task<int> GetParagraphsCountBySubjectAsync(string subjectId);

        #endregion

        #region 写入

        /// <summary>
        ///     创建一个新的节。
        /// </summary>
        /// <param name="newParagraph">新的节。</param>
        /// <returns>创建后的节。</returns>
        Paragraph CreateParagraph(Paragraph newParagraph);

        /// <summary>
        ///     异步创建一个新的节。
        /// </summary>
        /// <param name="newParagraph">新的节。</param>
        /// <returns>创建后的节。</returns>
        Task<Paragraph> CreateParagraphAsync(Paragraph newParagraph);

        /// <summary>
        ///     更新一个节。
        /// </summary>
        /// <param name="existingParagraph">原有的节。</param>
        /// <param name="newParagraph">新的节。</param>
        /// <returns>更新后的节。</returns>
        Paragraph UpdateParagraph(Paragraph existingParagraph, Paragraph newParagraph);

        /// <summary>
        ///     异步更新一个节。
        /// </summary>
        /// <param name="existingParagraph">原有的节。</param>
        /// <param name="newParagraph">新的节。</param>
        /// <returns>更新后的节。</returns>
        Task<Paragraph> UpdateParagraphAsync(Paragraph existingParagraph, Paragraph newParagraph);

        /// <summary>
        ///     删除一个节。
        /// </summary>
        /// <param name="paragraphId">节的编号。</param>
        void DeleteParagraph(string paragraphId);

        /// <summary>
        ///     异步删除一个节。
        /// </summary>
        /// <param name="paragraphId">节的编号。</param>
        Task DeleteParagraphAsync(string paragraphId);

        /// <summary>
        ///     增加一个节的查看的次数。
        /// </summary>
        /// <param name="paragraphId">节的编号。</param>
        /// <param name="count">增加的数量。</param>
        Paragraph IncrementParagraphViewsCount(string paragraphId, int count);

        /// <summary>
        ///     异步增加一个节的查看的次数。
        /// </summary>
        /// <param name="paragraphId">节的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task<Paragraph> IncrementParagraphViewsCountAsync(string paragraphId, int count);

        /// <summary>
        ///     增加一个节的收藏的次数。
        /// </summary>
        /// <param name="paragraphId">节的编号。</param>
        /// <param name="count">增加的数量。</param>
        Paragraph IncrementParagraphBookmarksCount(string paragraphId, int count);

        /// <summary>
        ///     异步增加一个节的收藏的次数。
        /// </summary>
        /// <param name="paragraphId">节的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task<Paragraph> IncrementParagraphBookmarksCountAsync(string paragraphId, int count);

        /// <summary>
        ///     增加一个节的评论的次数。
        /// </summary>
        /// <param name="paragraphId">节的编号。</param>
        /// <param name="count">增加的数量。</param>
        Paragraph IncrementParagraphCommentsCount(string paragraphId, int count);

        /// <summary>
        ///     异步增加一个节的评论的次数。
        /// </summary>
        /// <param name="paragraphId">节的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task<Paragraph> IncrementParagraphCommentsCountAsync(string paragraphId, int count);

        /// <summary>
        ///     增加一个节的点赞的次数。
        /// </summary>
        /// <param name="paragraphId">节的编号。</param>
        /// <param name="count">增加的数量。</param>
        Paragraph IncrementParagraphLikesCount(string paragraphId, int count);

        /// <summary>
        ///     异步增加一个节的点赞的次数。
        /// </summary>
        /// <param name="paragraphId">节的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task<Paragraph> IncrementParagraphLikesCountAsync(string paragraphId, int count);

        /// <summary>
        ///     增加一个节的评分的次数。
        /// </summary>
        /// <param name="paragraphId">节的编号。</param>
        /// <param name="count">增加的数量。</param>
        Paragraph IncrementParagraphRatingsCount(string paragraphId, int count);

        /// <summary>
        ///     异步增加一个节的评分的次数。
        /// </summary>
        /// <param name="paragraphId">节的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task<Paragraph> IncrementParagraphRatingsCountAsync(string paragraphId, int count);

        /// <summary>
        ///     增加一个节的分享的次数。
        /// </summary>
        /// <param name="paragraphId">节的编号。</param>
        /// <param name="count">增加的数量。</param>
        Paragraph IncrementParagraphSharesCount(string paragraphId, int count);

        /// <summary>
        ///     异步增加一个节的分享的次数。
        /// </summary>
        /// <param name="paragraphId">节的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task<Paragraph> IncrementParagraphSharesCountAsync(string paragraphId, int count);

        /// <summary>
        ///     更新一个节的评分的平均值。
        /// </summary>
        /// <param name="paragraphId">节的编号。</param>
        /// <param name="value">更新的数值。</param>
        Paragraph UpdateParagraphRatingsAverageValue(string paragraphId, float value);

        /// <summary>
        ///     异步更新一个节的评分的平均值。
        /// </summary>
        /// <param name="paragraphId">节的编号。</param>
        /// <param name="value">更新的数值。</param>
        Task<Paragraph> UpdateParagraphRatingsAverageValueAsync(string paragraphId, float value);

        #endregion
    }
}