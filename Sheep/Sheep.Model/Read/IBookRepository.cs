using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sheep.Model.Read.Entities;

namespace Sheep.Model.Read
{
    /// <summary>
    ///     书籍的存储库的接口定义。
    /// </summary>
    public interface IBookRepository
    {
        #region 获取

        /// <summary>
        ///     获取书籍。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <returns>书籍。</returns>
        Book GetBook(string bookId);

        /// <summary>
        ///     异步获取书籍。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <returns>书籍。</returns>
        Task<Book> GetBookAsync(string bookId);

        /// <summary>
        ///     查找书籍。
        /// </summary>
        /// <param name="titleFilter">过滤标题及概要的表达式。</param>
        /// <param name="tag">分类的标签。</param>
        /// <param name="publishedSince">过滤发布日期在指定的时间之后。</param>
        /// <param name="isPublished">是否已发布。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>书籍列表。</returns>
        List<Book> FindBooks(string titleFilter, string tag, DateTime? publishedSince, bool? isPublished, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步查找书籍。
        /// </summary>
        /// <param name="titleFilter">过滤标题及概要的表达式。</param>
        /// <param name="tag">分类的标签。</param>
        /// <param name="publishedSince">过滤发布日期在指定的时间之后。</param>
        /// <param name="isPublished">是否已发布。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>书籍列表。</returns>
        Task<List<Book>> FindBooksAsync(string titleFilter, string tag, DateTime? publishedSince, bool? isPublished, string orderBy, bool? descending, int? skip, int? limit);

        #endregion

        #region 统计

        /// <summary>
        ///     查找获取书籍数量。
        /// </summary>
        /// <param name="titleFilter">过滤标题及概要的表达式。</param>
        /// <param name="tag">分类的标签。</param>
        /// <param name="publishedSince">过滤发布日期在指定的时间之后。</param>
        /// <param name="isPublished">是否已发布。</param>
        /// <returns>书籍数量。</returns>
        int GetBooksCount(string titleFilter, string tag, DateTime? publishedSince, bool? isPublished);

        /// <summary>
        ///     异步获取书籍数量。
        /// </summary>
        /// <param name="titleFilter">过滤标题及概要的表达式。</param>
        /// <param name="tag">分类的标签。</param>
        /// <param name="publishedSince">过滤发布日期在指定的时间之后。</param>
        /// <param name="isPublished">是否已发布。</param>
        /// <returns>书籍数量。</returns>
        Task<int> GetBooksCountAsync(string titleFilter, string tag, DateTime? publishedSince, bool? isPublished);

        #endregion

        #region 写入

        /// <summary>
        ///     创建一个新的书籍。
        /// </summary>
        /// <param name="newBook">新的书籍。</param>
        /// <returns>创建后的书籍。</returns>
        Book CreateBook(Book newBook);

        /// <summary>
        ///     异步创建一个新的书籍。
        /// </summary>
        /// <param name="newBook">新的书籍。</param>
        /// <returns>创建后的书籍。</returns>
        Task<Book> CreateBookAsync(Book newBook);

        /// <summary>
        ///     更新一本书籍。
        /// </summary>
        /// <param name="existingBook">原有的书籍。</param>
        /// <param name="newBook">新的书籍。</param>
        /// <returns>更新后的书籍。</returns>
        Book UpdateBook(Book existingBook, Book newBook);

        /// <summary>
        ///     异步更新一本书籍。
        /// </summary>
        /// <param name="existingBook">原有的书籍。</param>
        /// <param name="newBook">新的书籍。</param>
        /// <returns>更新后的书籍。</returns>
        Task<Book> UpdateBookAsync(Book existingBook, Book newBook);

        /// <summary>
        ///     删除一本书籍。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        void DeleteBook(string bookId);

        /// <summary>
        ///     异步删除一本书籍。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        Task DeleteBookAsync(string bookId);

        /// <summary>
        ///     增加一本书籍的卷数。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="count">增加的数量。</param>
        Book IncrementBookVolumesCount(string bookId, int count);

        /// <summary>
        ///     异步增加一本书籍的卷数。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task<Book> IncrementBookVolumesCountAsync(string bookId, int count);

        /// <summary>
        ///     增加一本书籍的收藏的次数。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="count">增加的数量。</param>
        Book IncrementBookBookmarksCount(string bookId, int count);

        /// <summary>
        ///     异步增加一本书籍的收藏的次数。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task<Book> IncrementBookBookmarksCountAsync(string bookId, int count);

        /// <summary>
        ///     增加一本书籍的评分的次数。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="count">增加的数量。</param>
        Book IncrementBookRatingsCount(string bookId, int count);

        /// <summary>
        ///     异步增加一本书籍的评分的次数。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task<Book> IncrementBookRatingsCountAsync(string bookId, int count);

        /// <summary>
        ///     增加一本书籍的分享的次数。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="count">增加的数量。</param>
        Book IncrementBookSharesCount(string bookId, int count);

        /// <summary>
        ///     异步增加一本书籍的分享的次数。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task<Book> IncrementBookSharesCountAsync(string bookId, int count);

        /// <summary>
        ///     更新一本书籍的评分的平均值。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="value">更新的数值。</param>
        Book UpdateBookRatingsAverageValue(string bookId, float value);

        /// <summary>
        ///     异步更新一本书籍的评分的平均值。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="value">更新的数值。</param>
        Task<Book> UpdateBookRatingsAverageValueAsync(string bookId, float value);

        #endregion
    }
}