using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RethinkDb.Driver;
using RethinkDb.Driver.Ast;
using RethinkDb.Driver.Model;
using RethinkDb.Driver.Net;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Logging;
using Sheep.Model.Bookstore.Entities;
using Sheep.Model.Properties;

namespace Sheep.Model.Bookstore.Repositories
{
    /// <summary>
    ///     基于RethinkDb的节的存储库。
    /// </summary>
    public class RethinkDbParagraphRepository : IParagraphRepository, IClearable
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RethinkDbParagraphRepository));

        /// <summary>
        ///     RethinkDB 查询器.
        /// </summary>
        public static readonly RethinkDB R = RethinkDB.R;

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
        ///     初始化一个新的<see cref="RethinkDbParagraphRepository" />对象。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="shards">数据库分片数。</param>
        /// <param name="replicas">复制份数。</param>
        /// <param name="createMissingTables">是否创建数据表。</param>
        public RethinkDbParagraphRepository(IConnection conn, int shards, int replicas, bool createMissingTables)
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
                throw new InvalidOperationException(string.Format("One of the tables needed by {0} is missing. You can call {0} constructor with the parameter CreateMissingTables set to 'true'  to create the needed tables.", typeof(RethinkDbParagraphRepository).Name));
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
            if (tables.Contains(s_ParagraphTable))
            {
                R.TableDrop(s_ParagraphTable).RunResult(_conn).AssertNoErrors().AssertTablesDropped(1);
            }
            CreateTables();
        }

        /// <summary>
        ///     重新创建数据表。
        /// </summary>
        public void CreateTables()
        {
            var tables = R.TableList().RunResult<List<string>>(_conn);
            if (!tables.Contains(s_ParagraphTable))
            {
                R.TableCreate(s_ParagraphTable).OptArg("primary_key", "Id").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
                R.Table(s_ParagraphTable).IndexCreate("BookId_VolumeNumber_ChapterNumber_Number", row => R.Array(row.G("BookId"), row.G("VolumeNumber"), row.G("ChapterNumber"), row.G("Number"))).RunResult(_conn).AssertNoErrors();
                R.Table(s_ParagraphTable).IndexCreate("ChapterId_Number", row => R.Array(row.G("ChapterId"), row.G("Number"))).RunResult(_conn).AssertNoErrors();
                R.Table(s_ParagraphTable).IndexCreate("BookId").RunResult(_conn).AssertNoErrors();
                R.Table(s_ParagraphTable).IndexCreate("VolumeId").RunResult(_conn).AssertNoErrors();
                R.Table(s_ParagraphTable).IndexCreate("ChapterId").RunResult(_conn).AssertNoErrors();
                R.Table(s_ParagraphTable).IndexCreate("SubjectId").RunResult(_conn).AssertNoErrors();
                //R.Table(s_ParagraphTable).IndexWait().RunResult(_conn).AssertNoErrors();
            }
        }

        /// <summary>
        ///     检测指定的数据表是否存在。
        /// </summary>
        public bool TablesExists()
        {
            var tableNames = new List<string>
                             {
                                 s_ParagraphTable
                             };
            var tables = R.TableList().RunResult<List<string>>(_conn);
            return tables.Any(table => tableNames.Contains(table));
        }

        #endregion

        #region 检测节是否存在

        private void AssertNoExistingParagraph(Paragraph newParagraph, Paragraph exceptForExistingParagraph = null)
        {
            var existingParagraph = GetParagraph(newParagraph.ChapterId, newParagraph.Number);
            if (existingParagraph != null && (exceptForExistingParagraph == null || existingParagraph.Id != exceptForExistingParagraph.Id))
            {
                throw new ArgumentException(string.Format(Resources.ChapterWithNumberAlreadyExists, newParagraph.ChapterId, newParagraph.Number));
            }
            existingParagraph = GetParagraph(newParagraph.BookId, newParagraph.VolumeNumber, newParagraph.ChapterNumber, newParagraph.Number);
            if (existingParagraph != null && (exceptForExistingParagraph == null || existingParagraph.Id != exceptForExistingParagraph.Id))
            {
                throw new ArgumentException(string.Format(Resources.BookWithVolumeAndChapterAndNumberAlreadyExists, newParagraph.BookId, newParagraph.VolumeNumber, newParagraph.ChapterNumber, newParagraph.Number));
            }
        }

        private async Task AssertNoExistingParagraphAsync(Paragraph newParagraph, Paragraph exceptForExistingParagraph = null)
        {
            var existingParagraph = await GetParagraphAsync(newParagraph.ChapterId, newParagraph.Number);
            if (existingParagraph != null && (exceptForExistingParagraph == null || existingParagraph.Id != exceptForExistingParagraph.Id))
            {
                throw new ArgumentException(string.Format(Resources.ChapterWithNumberAlreadyExists, newParagraph.ChapterId, newParagraph.Number));
            }
            existingParagraph = await GetParagraphAsync(newParagraph.BookId, newParagraph.VolumeNumber, newParagraph.ChapterNumber, newParagraph.Number);
            if (existingParagraph != null && (exceptForExistingParagraph == null || existingParagraph.Id != exceptForExistingParagraph.Id))
            {
                throw new ArgumentException(string.Format(Resources.BookWithVolumeAndChapterAndNumberAlreadyExists, newParagraph.BookId, newParagraph.VolumeNumber, newParagraph.ChapterNumber, newParagraph.Number));
            }
        }

        #endregion

        #region IParagraphRepository 接口实现

        /// <inheritdoc />
        public Paragraph GetParagraph(string paragraphId)
        {
            return R.Table(s_ParagraphTable).Get(paragraphId).RunResult<Paragraph>(_conn);
        }

        /// <inheritdoc />
        public Task<Paragraph> GetParagraphAsync(string paragraphId)
        {
            return R.Table(s_ParagraphTable).Get(paragraphId).RunResultAsync<Paragraph>(_conn);
        }

        /// <inheritdoc />
        public List<Paragraph> GetParagraphs(List<string> paragraphIds)
        {
            return R.Table(s_ParagraphTable).GetAll(R.Args(paragraphIds.ToArray())).Skip(0).Limit(100000).RunResult<List<Paragraph>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Paragraph>> GetParagraphsAsync(List<string> paragraphIds)
        {
            return R.Table(s_ParagraphTable).GetAll(R.Args(paragraphIds.ToArray())).Skip(0).Limit(100000).RunResultAsync<List<Paragraph>>(_conn);
        }

        /// <inheritdoc />
        public Paragraph GetParagraph(string chapterId, int number)
        {
            return R.Table(s_ParagraphTable).GetAll(R.Array(chapterId, number)).OptArg("index", "ChapterId_Number").Nth(0).Default_(default(Paragraph)).RunResult<Paragraph>(_conn);
        }

        /// <inheritdoc />
        public Task<Paragraph> GetParagraphAsync(string chapterId, int number)
        {
            return R.Table(s_ParagraphTable).GetAll(R.Array(chapterId, number)).OptArg("index", "ChapterId_Number").Nth(0).Default_(default(Paragraph)).RunResultAsync<Paragraph>(_conn);
        }

        /// <inheritdoc />
        public Paragraph GetParagraph(string bookId, int volumeNumber, int chapterNumber, int number)
        {
            return R.Table(s_ParagraphTable).GetAll(R.Array(bookId, volumeNumber, chapterNumber, number)).OptArg("index", "BookId_VolumeNumber_ChapterNumber_Number").Nth(0).Default_(default(Paragraph)).RunResult<Paragraph>(_conn);
        }

        /// <inheritdoc />
        public Task<Paragraph> GetParagraphAsync(string bookId, int volumeNumber, int chapterNumber, int number)
        {
            return R.Table(s_ParagraphTable).GetAll(R.Array(bookId, volumeNumber, chapterNumber, number)).OptArg("index", "BookId_VolumeNumber_ChapterNumber_Number").Nth(0).Default_(default(Paragraph)).RunResultAsync<Paragraph>(_conn);
        }

        /// <inheritdoc />
        public List<Paragraph> FindParagraphs(string bookId, int? volumeNumber, int? chapterNumber, string contentFilter, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ParagraphTable).GetAll(bookId).OptArg("index", "BookId").Filter(true);
            if (volumeNumber.HasValue)
            {
                query = query.Filter(row => row.G("VolumeNumber").Eq(volumeNumber));
            }
            if (chapterNumber.HasValue)
            {
                query = query.Filter(row => row.G("ChapterNumber").Eq(chapterNumber));
            }
            if (!contentFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Content").Match(contentFilter));
            }
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("Number")) : query.OrderBy("Number");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<Paragraph>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Paragraph>> FindParagraphsAsync(string bookId, int? volumeNumber, int? chapterNumber, string contentFilter, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ParagraphTable).GetAll(bookId).OptArg("index", "BookId").Filter(true);
            if (volumeNumber.HasValue)
            {
                query = query.Filter(row => row.G("VolumeNumber").Eq(volumeNumber));
            }
            if (chapterNumber.HasValue)
            {
                query = query.Filter(row => row.G("ChapterNumber").Eq(chapterNumber));
            }
            if (!contentFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Content").Match(contentFilter));
            }
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("Number")) : query.OrderBy("Number");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<Paragraph>>(_conn);
        }

        /// <inheritdoc />
        public List<Paragraph> FindParagraphs(List<string> paragraphIds, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ParagraphTable).GetAll(R.Args(paragraphIds.ToArray())).Filter(true);
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("Number")) : query.OrderBy("Number");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<Paragraph>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Paragraph>> FindParagraphsAsync(List<string> paragraphIds, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ParagraphTable).GetAll(R.Args(paragraphIds.ToArray())).Filter(true);
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("Number")) : query.OrderBy("Number");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<Paragraph>>(_conn);
        }

        /// <inheritdoc />
        public List<Paragraph> FindParagraphsByChapter(string chapterId, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ParagraphTable).GetAll(chapterId).OptArg("index", "ChapterId").Filter(true);
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("Number")) : query.OrderBy("Number");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<Paragraph>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Paragraph>> FindParagraphsByChapterAsync(string chapterId, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ParagraphTable).GetAll(chapterId).OptArg("index", "ChapterId").Filter(true);
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("Number")) : query.OrderBy("Number");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<Paragraph>>(_conn);
        }

        /// <inheritdoc />
        public List<Paragraph> FindParagraphsByChapters(List<string> chapterIds, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ParagraphTable).GetAll(R.Args(chapterIds.ToArray())).OptArg("index", "ChapterId").Filter(true);
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("Number")) : query.OrderBy("Number");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<Paragraph>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Paragraph>> FindParagraphsByChaptersAsync(List<string> chapterIds, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ParagraphTable).GetAll(R.Args(chapterIds.ToArray())).OptArg("index", "ChapterId").Filter(true);
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("Number")) : query.OrderBy("Number");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<Paragraph>>(_conn);
        }

        /// <inheritdoc />
        public List<Paragraph> FindParagraphsBySubject(string subjectId, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ParagraphTable).GetAll(subjectId).OptArg("index", "SubjectId").Filter(true);
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("Number")) : query.OrderBy("Number");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<Paragraph>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Paragraph>> FindParagraphsBySubjectAsync(string subjectId, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ParagraphTable).GetAll(subjectId).OptArg("index", "SubjectId").Filter(true);
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("Number")) : query.OrderBy("Number");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<Paragraph>>(_conn);
        }

        /// <inheritdoc />
        public List<Paragraph> FindParagraphsInRange(string bookId, int volumeNumber, int beginChapterNumber, int beginNumber, int endChapterNumber, int endNumber, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ParagraphTable).Between(R.Array(bookId, volumeNumber, beginChapterNumber, beginNumber), R.Array(bookId, volumeNumber, endChapterNumber, endNumber)).OptArg("index", "BookId_VolumeNumber_ChapterNumber_Number").OptArg("right_bound", Bound.Closed).Filter(true);
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("Number")) : query.OrderBy("Number");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<Paragraph>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Paragraph>> FindParagraphsInRangeAsync(string bookId, int volumeNumber, int beginChapterNumber, int beginNumber, int endChapterNumber, int endNumber, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ParagraphTable).Between(R.Array(bookId, volumeNumber, beginChapterNumber, beginNumber), R.Array(bookId, volumeNumber, endChapterNumber, endNumber)).OptArg("index", "BookId_VolumeNumber_ChapterNumber_Number").OptArg("right_bound", Bound.Closed).Filter(true);
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("Number")) : query.OrderBy("Number");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<Paragraph>>(_conn);
        }

        /// <inheritdoc />
        public int GetParagraphsCount(string bookId, int? volumeNumber, int? chapterNumber, string contentFilter)
        {
            var query = R.Table(s_ParagraphTable).GetAll(bookId).OptArg("index", "BookId").Filter(true);
            if (volumeNumber.HasValue)
            {
                query = query.Filter(row => row.G("VolumeNumber").Eq(volumeNumber));
            }
            if (chapterNumber.HasValue)
            {
                query = query.Filter(row => row.G("ChapterNumber").Eq(chapterNumber));
            }
            if (!contentFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Content").Match(contentFilter));
            }
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetParagraphsCountAsync(string bookId, int? volumeNumber, int? chapterNumber, string contentFilter)
        {
            var query = R.Table(s_ParagraphTable).GetAll(bookId).OptArg("index", "BookId").Filter(true);
            if (volumeNumber.HasValue)
            {
                query = query.Filter(row => row.G("VolumeNumber").Eq(volumeNumber));
            }
            if (chapterNumber.HasValue)
            {
                query = query.Filter(row => row.G("ChapterNumber").Eq(chapterNumber));
            }
            if (!contentFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Content").Match(contentFilter));
            }
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public int GetParagraphsCountByChapter(string chapterId)
        {
            var query = R.Table(s_ParagraphTable).GetAll(chapterId).OptArg("index", "ChapterId").Filter(true);
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetParagraphsCountByChapterAsync(string chapterId)
        {
            var query = R.Table(s_ParagraphTable).GetAll(chapterId).OptArg("index", "ChapterId").Filter(true);
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public int GetParagraphsCountBySubject(string subjectId)
        {
            var query = R.Table(s_ParagraphTable).GetAll(subjectId).OptArg("index", "SubjectId").Filter(true);
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetParagraphsCountBySubjectAsync(string subjectId)
        {
            var query = R.Table(s_ParagraphTable).GetAll(subjectId).OptArg("index", "SubjectId").Filter(true);
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public Paragraph CreateParagraph(Paragraph newParagraph)
        {
            newParagraph.ThrowIfNull(nameof(newParagraph));
            AssertNoExistingParagraph(newParagraph);
            newParagraph.Id = string.Format("{0}-{1}", newParagraph.ChapterId, newParagraph.Number);
            newParagraph.ViewsCount = 0;
            newParagraph.BookmarksCount = 0;
            newParagraph.CommentsCount = 0;
            newParagraph.LikesCount = 0;
            newParagraph.RatingsCount = 0;
            newParagraph.RatingsAverageValue = 0;
            newParagraph.SharesCount = 0;
            var result = R.Table(s_ParagraphTable).Get(newParagraph.Id).Replace(newParagraph).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Paragraph>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Paragraph> CreateParagraphAsync(Paragraph newParagraph)
        {
            newParagraph.ThrowIfNull(nameof(newParagraph));
            await AssertNoExistingParagraphAsync(newParagraph);
            newParagraph.Id = string.Format("{0}-{1}", newParagraph.ChapterId, newParagraph.Number);
            newParagraph.ViewsCount = 0;
            newParagraph.BookmarksCount = 0;
            newParagraph.CommentsCount = 0;
            newParagraph.LikesCount = 0;
            newParagraph.RatingsCount = 0;
            newParagraph.RatingsAverageValue = 0;
            newParagraph.SharesCount = 0;
            var result = (await R.Table(s_ParagraphTable).Get(newParagraph.Id).Replace(newParagraph).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Paragraph>()[0].NewValue;
        }

        /// <inheritdoc />
        public Paragraph UpdateParagraph(Paragraph existingParagraph, Paragraph newParagraph)
        {
            existingParagraph.ThrowIfNull(nameof(existingParagraph));
            newParagraph.Id = existingParagraph.Id;
            newParagraph.BookId = existingParagraph.BookId;
            newParagraph.VolumeId = existingParagraph.VolumeId;
            newParagraph.VolumeNumber = existingParagraph.VolumeNumber;
            newParagraph.ChapterId = existingParagraph.ChapterId;
            newParagraph.ChapterNumber = existingParagraph.ChapterNumber;
            //newParagraph.SubjectId = existingParagraph.SubjectId;
            newParagraph.Number = existingParagraph.Number;
            var result = R.Table(s_ParagraphTable).Get(newParagraph.Id).Replace(newParagraph).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Paragraph>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Paragraph> UpdateParagraphAsync(Paragraph existingParagraph, Paragraph newParagraph)
        {
            existingParagraph.ThrowIfNull(nameof(existingParagraph));
            newParagraph.Id = existingParagraph.Id;
            newParagraph.BookId = existingParagraph.BookId;
            newParagraph.VolumeId = existingParagraph.VolumeId;
            newParagraph.VolumeNumber = existingParagraph.VolumeNumber;
            newParagraph.ChapterId = existingParagraph.ChapterId;
            newParagraph.ChapterNumber = existingParagraph.ChapterNumber;
            //newParagraph.SubjectId = existingParagraph.SubjectId;
            newParagraph.Number = existingParagraph.Number;
            var result = (await R.Table(s_ParagraphTable).Get(newParagraph.Id).Replace(newParagraph).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Paragraph>()[0].NewValue;
        }

        /// <inheritdoc />
        public void DeleteParagraph(string paragraphId)
        {
            R.Table(s_ParagraphTable).Get(paragraphId).Delete().RunResult(_conn).AssertNoErrors();
            R.Table(s_ParagraphAnnotationTable).GetAll(paragraphId).OptArg("index", "ParagraphId").Delete().RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task DeleteParagraphAsync(string paragraphId)
        {
            (await R.Table(s_ParagraphTable).Get(paragraphId).Delete().RunResultAsync(_conn)).AssertNoErrors();
            (await R.Table(s_ParagraphAnnotationTable).GetAll(paragraphId).OptArg("index", "ParagraphId").RunResultAsync(_conn)).AssertNoErrors();
        }

        /// <inheritdoc />
        public void IncrementParagraphViewsCount(string paragraphId, int count)
        {
            R.Table(s_ParagraphTable).Get(paragraphId).Update(row => R.HashMap("ViewsCount", row.G("ViewsCount").Default_(0).Add(count))).RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task IncrementParagraphViewsCountAsync(string paragraphId, int count)
        {
            (await R.Table(s_ParagraphTable).Get(paragraphId).Update(row => R.HashMap("ViewsCount", row.G("ViewsCount").Default_(0).Add(count))).RunResultAsync(_conn)).AssertNoErrors();
        }

        /// <inheritdoc />
        public void IncrementParagraphsViewsCount(List<string> paragraphIds, int count)
        {
            R.Table(s_ParagraphTable).GetAll(R.Args(paragraphIds.ToArray())).Update(row => R.HashMap("ViewsCount", row.G("ViewsCount").Default_(0).Add(count))).RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task IncrementParagraphsViewsCountAsync(List<string> paragraphIds, int count)
        {
            (await R.Table(s_ParagraphTable).GetAll(R.Args(paragraphIds.ToArray())).Update(row => R.HashMap("ViewsCount", row.G("ViewsCount").Default_(0).Add(count))).RunResultAsync(_conn)).AssertNoErrors();
        }

        /// <inheritdoc />
        public void IncrementParagraphBookmarksCount(string paragraphId, int count)
        {
            R.Table(s_ParagraphTable).Get(paragraphId).Update(row => R.HashMap("BookmarksCount", row.G("BookmarksCount").Default_(0).Add(count))).RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task IncrementParagraphBookmarksCountAsync(string paragraphId, int count)
        {
            (await R.Table(s_ParagraphTable).Get(paragraphId).Update(row => R.HashMap("BookmarksCount", row.G("BookmarksCount").Default_(0).Add(count))).RunResultAsync(_conn)).AssertNoErrors();
        }

        /// <inheritdoc />
        public void IncrementParagraphCommentsCount(string paragraphId, int count)
        {
            R.Table(s_ParagraphTable).Get(paragraphId).Update(row => R.HashMap("CommentsCount", row.G("CommentsCount").Default_(0).Add(count))).RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task IncrementParagraphCommentsCountAsync(string paragraphId, int count)
        {
            (await R.Table(s_ParagraphTable).Get(paragraphId).Update(row => R.HashMap("CommentsCount", row.G("CommentsCount").Default_(0).Add(count))).RunResultAsync(_conn)).AssertNoErrors();
        }

        /// <inheritdoc />
        public void IncrementParagraphLikesCount(string paragraphId, int count)
        {
            R.Table(s_ParagraphTable).Get(paragraphId).Update(row => R.HashMap("LikesCount", row.G("LikesCount").Default_(0).Add(count))).RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task IncrementParagraphLikesCountAsync(string paragraphId, int count)
        {
            (await R.Table(s_ParagraphTable).Get(paragraphId).Update(row => R.HashMap("LikesCount", row.G("LikesCount").Default_(0).Add(count))).RunResultAsync(_conn)).AssertNoErrors();
        }

        /// <inheritdoc />
        public void IncrementParagraphRatingsCount(string paragraphId, int count)
        {
            R.Table(s_ParagraphTable).Get(paragraphId).Update(row => R.HashMap("RatingsCount", row.G("RatingsCount").Default_(0).Add(count))).RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task IncrementParagraphRatingsCountAsync(string paragraphId, int count)
        {
            (await R.Table(s_ParagraphTable).Get(paragraphId).Update(row => R.HashMap("RatingsCount", row.G("RatingsCount").Default_(0).Add(count))).RunResultAsync(_conn)).AssertNoErrors();
        }

        /// <inheritdoc />
        public void IncrementParagraphSharesCount(string paragraphId, int count)
        {
            R.Table(s_ParagraphTable).Get(paragraphId).Update(row => R.HashMap("SharesCount", row.G("SharesCount").Default_(0).Add(count))).RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task IncrementParagraphSharesCountAsync(string paragraphId, int count)
        {
            (await R.Table(s_ParagraphTable).Get(paragraphId).Update(row => R.HashMap("SharesCount", row.G("SharesCount").Default_(0).Add(count))).RunResultAsync(_conn)).AssertNoErrors();
        }

        /// <inheritdoc />
        public void UpdateParagraphRatingsAverageValue(string paragraphId, float value)
        {
            R.Table(s_ParagraphTable).Get(paragraphId).Update(row => R.HashMap("RatingsAverageValue", value)).RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task UpdateParagraphRatingsAverageValueAsync(string paragraphId, float value)
        {
            (await R.Table(s_ParagraphTable).Get(paragraphId).Update(row => R.HashMap("RatingsAverageValue", value)).RunResultAsync(_conn)).AssertNoErrors();
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