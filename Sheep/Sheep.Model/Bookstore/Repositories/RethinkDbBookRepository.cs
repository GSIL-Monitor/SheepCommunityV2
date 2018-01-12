using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funcular.IdGenerators.Base36;
using RethinkDb.Driver;
using RethinkDb.Driver.Ast;
using RethinkDb.Driver.Model;
using RethinkDb.Driver.Net;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Logging;
using Sheep.Model.Bookstore.Entities;

namespace Sheep.Model.Bookstore.Repositories
{
    /// <summary>
    ///     基于RethinkDb的书籍的存储库。
    /// </summary>
    public class RethinkDbBookRepository : IBookRepository, IClearable
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RethinkDbBookRepository));

        /// <summary>
        ///     RethinkDB 查询器.
        /// </summary>
        public static readonly RethinkDB R = RethinkDB.R;

        /// <summary>
        ///     书籍的数据表名。
        /// </summary>
        private static readonly string s_BookTable = typeof(Book).Name;

        /// <summary>
        ///     卷的数据表名。
        /// </summary>
        private static readonly string s_VolumeTable = typeof(Volume).Name;

        /// <summary>
        ///     卷注释的数据表名。
        /// </summary>
        private static readonly string s_VolumeAnnotationTable = typeof(VolumeAnnotation).Name;

        /// <summary>
        ///     主题的数据表名。
        /// </summary>
        private static readonly string s_SubjectTable = typeof(Subject).Name;

        /// <summary>
        ///     章的数据表名。
        /// </summary>
        private static readonly string s_ChapterTable = typeof(Chapter).Name;

        /// <summary>
        ///     章注释的数据表名。
        /// </summary>
        private static readonly string s_ChapterAnnotationTable = typeof(ChapterAnnotation).Name;

        /// <summary>
        ///     节的数据表名。
        /// </summary>
        private static readonly string s_ParagraphTable = typeof(Paragraph).Name;

        /// <summary>
        ///     节注释的数据表名。
        /// </summary>
        private static readonly string s_ParagraphAnnotationTable = typeof(ParagraphAnnotation).Name;

        #endregion

        #region 属性

        private readonly IConnection _conn;
        private readonly int _shards;
        private readonly int _replicas;

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="RethinkDbBookRepository" />对象。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="shards">数据库分片数。</param>
        /// <param name="replicas">复制份数。</param>
        /// <param name="createMissingTables">是否创建数据表。</param>
        public RethinkDbBookRepository(IConnection conn, int shards, int replicas, bool createMissingTables)
        {
            _conn = conn;
            _shards = shards;
            _replicas = replicas;
            // 创建数据表。
            if (createMissingTables)
            {
                CreateTables();
            }
            // 检测指定的数据表是否存在。
            if (!TablesExists())
            {
                throw new InvalidOperationException(string.Format("One of the tables needed by {0} is missing. You can call {0} constructor with the parameter CreateMissingTables set to 'true'  to create the needed tables.", typeof(RethinkDbBookRepository).Name));
            }
        }

        #endregion

        #region 数据表检测及创建

        /// <summary>
        ///     删除并重新创建数据表。
        /// </summary>
        public void DropAndReCreateTables()
        {
            var tables = R.TableList().RunResult<List<string>>(_conn);
            if (tables.Contains(s_BookTable))
            {
                R.TableDrop(s_BookTable).RunResult(_conn).AssertNoErrors().AssertTablesDropped(1);
            }
            CreateTables();
        }

        /// <summary>
        ///     重新创建数据表。
        /// </summary>
        public void CreateTables()
        {
            var tables = R.TableList().RunResult<List<string>>(_conn);
            if (!tables.Contains(s_BookTable))
            {
                R.TableCreate(s_BookTable).OptArg("primary_key", "Id").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
                R.Table(s_BookTable).IndexCreate("Tags").OptArg("multi", true).RunResult(_conn).AssertNoErrors();
                //R.Table(s_BookTable).IndexWait().RunResult(_conn).AssertNoErrors();
            }
        }

        /// <summary>
        ///     检测指定的数据表是否存在。
        /// </summary>
        public bool TablesExists()
        {
            var tableNames = new List<string>
                             {
                                 s_BookTable
                             };
            var tables = R.TableList().RunResult<List<string>>(_conn);
            return tables.Any(table => tableNames.Contains(table));
        }

        #endregion

        #region 检测书籍是否存在

        #endregion

        #region IBookRepository 接口实现

        /// <inheritdoc />
        public Book GetBook(string bookId)
        {
            return R.Table(s_BookTable).Get(bookId).RunResult<Book>(_conn);
        }

        /// <inheritdoc />
        public Task<Book> GetBookAsync(string bookId)
        {
            return R.Table(s_BookTable).Get(bookId).RunResultAsync<Book>(_conn);
        }

        /// <inheritdoc />
        public List<Book> FindBooks(string titleFilter, string tag, DateTime? publishedSince, bool? isPublished, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_BookTable).Filter(true);
            if (!tag.IsNullOrEmpty())
            {
                query = R.Table(s_BookTable).GetAll(tag).OptArg("index", "Tags").Filter(true);
            }
            if (!titleFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Title").Match(titleFilter).Or(row.G("Summary").Match(titleFilter)));
            }
            if (publishedSince.HasValue)
            {
                query = query.Filter(row => row.G("PublishedDate").Ge(publishedSince.Value));
            }
            if (isPublished.HasValue)
            {
                query = query.Filter(row => row.G("IsPublished").Eq(isPublished.Value));
            }
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("PublishedDate")) : query.OrderBy("PublishedDate");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<Book>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Book>> FindBooksAsync(string titleFilter, string tag, DateTime? publishedSince, bool? isPublished, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_BookTable).Filter(true);
            if (!tag.IsNullOrEmpty())
            {
                query = R.Table(s_BookTable).GetAll(tag).OptArg("index", "Tags").Filter(true);
            }
            if (!titleFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Title").Match(titleFilter).Or(row.G("Summary").Match(titleFilter)));
            }
            if (publishedSince.HasValue)
            {
                query = query.Filter(row => row.G("PublishedDate").Ge(publishedSince.Value));
            }
            if (isPublished.HasValue)
            {
                query = query.Filter(row => row.G("IsPublished").Eq(isPublished.Value));
            }
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("PublishedDate")) : query.OrderBy("PublishedDate");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<Book>>(_conn);
        }

        /// <inheritdoc />
        public int GetBooksCount(string titleFilter, string tag, DateTime? publishedSince, bool? isPublished)
        {
            var query = R.Table(s_BookTable).Filter(true);
            if (!tag.IsNullOrEmpty())
            {
                query = R.Table(s_BookTable).GetAll(tag).OptArg("index", "Tags").Filter(true);
            }
            if (!titleFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Title").Match(titleFilter).Or(row.G("Summary").Match(titleFilter)));
            }
            if (publishedSince.HasValue)
            {
                query = query.Filter(row => row.G("PublishedDate").Ge(publishedSince.Value));
            }
            if (isPublished.HasValue)
            {
                query = query.Filter(row => row.G("IsPublished").Eq(isPublished.Value));
            }
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetBooksCountAsync(string titleFilter, string tag, DateTime? publishedSince, bool? isPublished)
        {
            var query = R.Table(s_BookTable).Filter(true);
            if (!tag.IsNullOrEmpty())
            {
                query = R.Table(s_BookTable).GetAll(tag).OptArg("index", "Tags").Filter(true);
            }
            if (!titleFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Title").Match(titleFilter).Or(row.G("Summary").Match(titleFilter)));
            }
            if (publishedSince.HasValue)
            {
                query = query.Filter(row => row.G("PublishedDate").Ge(publishedSince.Value));
            }
            if (isPublished.HasValue)
            {
                query = query.Filter(row => row.G("IsPublished").Eq(isPublished.Value));
            }
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public Book CreateBook(Book newBook)
        {
            newBook.ThrowIfNull(nameof(newBook));
            newBook.Id = newBook.Id.IsNullOrEmpty() ? new Base36IdGenerator(8, 4, 4).NewId().ToLower() : newBook.Id;
            newBook.PublishedDate = newBook.IsPublished ? DateTime.UtcNow : (DateTime?) null;
            newBook.BookmarksCount = 0;
            newBook.RatingsCount = 0;
            newBook.RatingsAverageValue = 0;
            newBook.SharesCount = 0;
            var result = R.Table(s_BookTable).Get(newBook.Id).Replace(newBook).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Book>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Book> CreateBookAsync(Book newBook)
        {
            newBook.ThrowIfNull(nameof(newBook));
            newBook.Id = newBook.Id.IsNullOrEmpty() ? new Base36IdGenerator(8, 4, 4).NewId().ToLower() : newBook.Id;
            newBook.PublishedDate = newBook.IsPublished ? DateTime.UtcNow : (DateTime?) null;
            newBook.BookmarksCount = 0;
            newBook.RatingsCount = 0;
            newBook.RatingsAverageValue = 0;
            newBook.SharesCount = 0;
            var result = (await R.Table(s_BookTable).Get(newBook.Id).Replace(newBook).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Book>()[0].NewValue;
        }

        /// <inheritdoc />
        public Book UpdateBook(Book existingBook, Book newBook)
        {
            existingBook.ThrowIfNull(nameof(existingBook));
            newBook.Id = existingBook.Id;
            var result = R.Table(s_BookTable).Get(newBook.Id).Replace(newBook).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Book>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Book> UpdateBookAsync(Book existingBook, Book newBook)
        {
            existingBook.ThrowIfNull(nameof(existingBook));
            newBook.Id = existingBook.Id;
            var result = (await R.Table(s_BookTable).Get(newBook.Id).Replace(newBook).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Book>()[0].NewValue;
        }

        /// <inheritdoc />
        public void DeleteBook(string bookId)
        {
            R.Table(s_BookTable).Get(bookId).Delete().RunResult(_conn).AssertNoErrors();
            R.Table(s_VolumeTable).GetAll(bookId).OptArg("index", "BookId").Delete().RunResult(_conn).AssertNoErrors();
            R.Table(s_VolumeAnnotationTable).GetAll(bookId).OptArg("index", "BookId").Delete().RunResult(_conn).AssertNoErrors();
            R.Table(s_SubjectTable).GetAll(bookId).OptArg("index", "BookId").Delete().RunResult(_conn).AssertNoErrors();
            R.Table(s_ChapterTable).GetAll(bookId).OptArg("index", "BookId").Delete().RunResult(_conn).AssertNoErrors();
            R.Table(s_ChapterAnnotationTable).GetAll(bookId).OptArg("index", "BookId").Delete().RunResult(_conn).AssertNoErrors();
            R.Table(s_ParagraphTable).GetAll(bookId).OptArg("index", "BookId").Delete().RunResult(_conn).AssertNoErrors();
            R.Table(s_ParagraphAnnotationTable).GetAll(bookId).OptArg("index", "BookId").Delete().RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task DeleteBookAsync(string bookId)
        {
            (await R.Table(s_BookTable).Get(bookId).Delete().RunResultAsync(_conn)).AssertNoErrors();
            (await R.Table(s_VolumeTable).GetAll(bookId).OptArg("index", "BookId").Delete().RunResultAsync(_conn)).AssertNoErrors();
            (await R.Table(s_VolumeAnnotationTable).GetAll(bookId).OptArg("index", "BookId").Delete().RunResultAsync(_conn)).AssertNoErrors();
            (await R.Table(s_SubjectTable).GetAll(bookId).OptArg("index", "BookId").Delete().RunResultAsync(_conn)).AssertNoErrors();
            (await R.Table(s_ChapterTable).GetAll(bookId).OptArg("index", "BookId").Delete().RunResultAsync(_conn)).AssertNoErrors();
            (await R.Table(s_ChapterAnnotationTable).GetAll(bookId).OptArg("index", "BookId").Delete().RunResultAsync(_conn)).AssertNoErrors();
            (await R.Table(s_ParagraphTable).GetAll(bookId).OptArg("index", "BookId").Delete().RunResultAsync(_conn)).AssertNoErrors();
            (await R.Table(s_ParagraphAnnotationTable).GetAll(bookId).OptArg("index", "BookId").Delete().RunResultAsync(_conn)).AssertNoErrors();
        }

        /// <inheritdoc />
        public void IncrementBookVolumesCount(string bookId, int count)
        {
            R.Table(s_BookTable).Get(bookId).Update(row => R.HashMap("VolumesCount", row.G("VolumesCount").Default_(0).Add(count))).RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task IncrementBookVolumesCountAsync(string bookId, int count)
        {
            (await R.Table(s_BookTable).Get(bookId).Update(row => R.HashMap("VolumesCount", row.G("VolumesCount").Default_(0).Add(count))).RunResultAsync(_conn)).AssertNoErrors();
        }

        /// <inheritdoc />
        public void IncrementBookBookmarksCount(string bookId, int count)
        {
            R.Table(s_BookTable).Get(bookId).Update(row => R.HashMap("BookmarksCount", row.G("BookmarksCount").Default_(0).Add(count))).RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task IncrementBookBookmarksCountAsync(string bookId, int count)
        {
            (await R.Table(s_BookTable).Get(bookId).Update(row => R.HashMap("BookmarksCount", row.G("BookmarksCount").Default_(0).Add(count))).RunResultAsync(_conn)).AssertNoErrors();
        }

        /// <inheritdoc />
        public void IncrementBookRatingsCount(string bookId, int count)
        {
            R.Table(s_BookTable).Get(bookId).Update(row => R.HashMap("RatingsCount", row.G("RatingsCount").Default_(0).Add(count))).RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task IncrementBookRatingsCountAsync(string bookId, int count)
        {
            (await R.Table(s_BookTable).Get(bookId).Update(row => R.HashMap("RatingsCount", row.G("RatingsCount").Default_(0).Add(count))).RunResultAsync(_conn)).AssertNoErrors();
        }

        /// <inheritdoc />
        public void IncrementBookSharesCount(string bookId, int count)
        {
            R.Table(s_BookTable).Get(bookId).Update(row => R.HashMap("SharesCount", row.G("SharesCount").Default_(0).Add(count))).RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task IncrementBookSharesCountAsync(string bookId, int count)
        {
            (await R.Table(s_BookTable).Get(bookId).Update(row => R.HashMap("SharesCount", row.G("SharesCount").Default_(0).Add(count))).RunResultAsync(_conn)).AssertNoErrors();
        }

        /// <inheritdoc />
        public void UpdateBookRatingsAverageValue(string bookId, float value)
        {
            R.Table(s_BookTable).Get(bookId).Update(row => R.HashMap("RatingsAverageValue", value)).RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task UpdateBookRatingsAverageValueAsync(string bookId, float value)
        {
            (await R.Table(s_BookTable).Get(bookId).Update(row => R.HashMap("RatingsAverageValue", value)).RunResultAsync(_conn)).AssertNoErrors();
        }

        #endregion

        #region IClearable 接口实现

        /// <inheritdoc />
        public void Clear()
        {
            DropAndReCreateTables();
        }

        #endregion
    }
}