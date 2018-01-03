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
using Sheep.Model.Properties;
using Sheep.Model.Bookstore.Entities;

namespace Sheep.Model.Bookstore.Repositories
{
    /// <summary>
    ///     基于RethinkDb的节注释的存储库。
    /// </summary>
    public class RethinkDbParagraphAnnotationRepository : IParagraphAnnotationRepository, IClearable
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RethinkDbParagraphAnnotationRepository));

        /// <summary>
        ///     RethinkDB 查询器.
        /// </summary>
        public static readonly RethinkDB R = RethinkDB.R;

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
        ///     初始化一个新的<see cref="RethinkDbParagraphAnnotationRepository" />对象。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="shards">数据库分片数。</param>
        /// <param name="replicas">复制份数。</param>
        /// <param name="createMissingTables">是否创建数据表。</param>
        public RethinkDbParagraphAnnotationRepository(IConnection conn, int shards, int replicas, bool createMissingTables)
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
                throw new InvalidOperationException(string.Format("One of the tables needed by {0} is missing. You can call {0} constructor with the parameter CreateMissingTables set to 'true'  to create the needed tables.", typeof(RethinkDbParagraphAnnotationRepository).Name));
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
            if (tables.Contains(s_ParagraphAnnotationTable))
            {
                R.TableDrop(s_ParagraphAnnotationTable).RunResult(_conn).AssertNoErrors().AssertTablesDropped(1);
            }
            CreateTables();
        }

        /// <summary>
        ///     重新创建数据表。
        /// </summary>
        public void CreateTables()
        {
            var tables = R.TableList().RunResult<List<string>>(_conn);
            if (!tables.Contains(s_ParagraphAnnotationTable))
            {
                R.TableCreate(s_ParagraphAnnotationTable).OptArg("primary_key", "Id").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
                R.Table(s_ParagraphAnnotationTable).IndexCreate("BookId_VolumeNumber_ChapterNumber_ParagraphNumber_Number", row => R.Array(row.G("BookId"), row.G("VolumeNumber"), row.G("ChapterNumber"), row.G("ParagraphNumber"), row.G("Number"))).RunResult(_conn).AssertNoErrors();
                R.Table(s_ParagraphAnnotationTable).IndexCreate("ParagraphId_Number", row => R.Array(row.G("ParagraphId"), row.G("Number"))).RunResult(_conn).AssertNoErrors();
                R.Table(s_ParagraphAnnotationTable).IndexCreate("BookId").RunResult(_conn).AssertNoErrors();
                R.Table(s_ParagraphAnnotationTable).IndexCreate("VolumeId").RunResult(_conn).AssertNoErrors();
                R.Table(s_ParagraphAnnotationTable).IndexCreate("ChapterId").RunResult(_conn).AssertNoErrors();
                R.Table(s_ParagraphAnnotationTable).IndexCreate("ParagraphId").RunResult(_conn).AssertNoErrors();
                //R.Table(s_ParagraphAnnotationTable).IndexWait().RunResult(_conn).AssertNoErrors();
            }
        }

        /// <summary>
        ///     检测指定的数据表是否存在。
        /// </summary>
        public bool TablesExists()
        {
            var tableNames = new List<string>
                             {
                                 s_ParagraphAnnotationTable
                             };
            var tables = R.TableList().RunResult<List<string>>(_conn);
            return tables.Any(table => tableNames.Contains(table));
        }

        #endregion

        #region 检测节注释是否存在

        private void AssertNoExistingParagraphAnnotation(ParagraphAnnotation newParagraphAnnotation, ParagraphAnnotation exceptForExistingParagraphAnnotation = null)
        {
            var existingParagraphAnnotation = GetParagraphAnnotation(newParagraphAnnotation.ParagraphId, newParagraphAnnotation.Number);
            if (existingParagraphAnnotation != null && (exceptForExistingParagraphAnnotation == null || existingParagraphAnnotation.Id != exceptForExistingParagraphAnnotation.Id))
            {
                throw new ArgumentException(string.Format(Resources.ParagraphWithNumberAlreadyExists, newParagraphAnnotation.ParagraphId, newParagraphAnnotation.Number));
            }
            existingParagraphAnnotation = GetParagraphAnnotation(newParagraphAnnotation.BookId, newParagraphAnnotation.VolumeNumber, newParagraphAnnotation.ChapterNumber, newParagraphAnnotation.ParagraphNumber, newParagraphAnnotation.Number);
            if (existingParagraphAnnotation != null && (exceptForExistingParagraphAnnotation == null || existingParagraphAnnotation.Id != exceptForExistingParagraphAnnotation.Id))
            {
                throw new ArgumentException(string.Format(Resources.BookWithVolumeAndChapterAndParagraphAndNumberAlreadyExists, newParagraphAnnotation.BookId, newParagraphAnnotation.VolumeNumber, newParagraphAnnotation.ChapterNumber, newParagraphAnnotation.ParagraphNumber, newParagraphAnnotation.Number));
            }
        }

        private async Task AssertNoExistingParagraphAnnotationAsync(ParagraphAnnotation newParagraphAnnotation, ParagraphAnnotation exceptForExistingParagraphAnnotation = null)
        {
            var existingParagraphAnnotation = await GetParagraphAnnotationAsync(newParagraphAnnotation.ParagraphId, newParagraphAnnotation.Number);
            if (existingParagraphAnnotation != null && (exceptForExistingParagraphAnnotation == null || existingParagraphAnnotation.Id != exceptForExistingParagraphAnnotation.Id))
            {
                throw new ArgumentException(string.Format(Resources.ParagraphWithNumberAlreadyExists, newParagraphAnnotation.ParagraphId, newParagraphAnnotation.Number));
            }
            existingParagraphAnnotation = await GetParagraphAnnotationAsync(newParagraphAnnotation.BookId, newParagraphAnnotation.VolumeNumber, newParagraphAnnotation.ChapterNumber, newParagraphAnnotation.ParagraphNumber, newParagraphAnnotation.Number);
            if (existingParagraphAnnotation != null && (exceptForExistingParagraphAnnotation == null || existingParagraphAnnotation.Id != exceptForExistingParagraphAnnotation.Id))
            {
                throw new ArgumentException(string.Format(Resources.BookWithVolumeAndChapterAndParagraphAndNumberAlreadyExists, newParagraphAnnotation.BookId, newParagraphAnnotation.VolumeNumber, newParagraphAnnotation.ChapterNumber, newParagraphAnnotation.ParagraphNumber, newParagraphAnnotation.Number));
            }
        }

        #endregion

        #region IParagraphAnnotationRepository 接口实现

        /// <inheritdoc />
        public ParagraphAnnotation GetParagraphAnnotation(string paragraphAnnotationId)
        {
            return R.Table(s_ParagraphAnnotationTable).Get(paragraphAnnotationId).RunResult<ParagraphAnnotation>(_conn);
        }

        /// <inheritdoc />
        public Task<ParagraphAnnotation> GetParagraphAnnotationAsync(string paragraphAnnotationId)
        {
            return R.Table(s_ParagraphAnnotationTable).Get(paragraphAnnotationId).RunResultAsync<ParagraphAnnotation>(_conn);
        }

        /// <inheritdoc />
        public ParagraphAnnotation GetParagraphAnnotation(string paragraphId, int number)
        {
            return R.Table(s_ParagraphAnnotationTable).GetAll(R.Array(paragraphId, number)).OptArg("index", "ParagraphId_Number").Nth(0).Default_(default(ParagraphAnnotation)).RunResult<ParagraphAnnotation>(_conn);
        }

        /// <inheritdoc />
        public Task<ParagraphAnnotation> GetParagraphAnnotationAsync(string paragraphId, int number)
        {
            return R.Table(s_ParagraphAnnotationTable).GetAll(R.Array(paragraphId, number)).OptArg("index", "ParagraphId_Number").Nth(0).Default_(default(ParagraphAnnotation)).RunResultAsync<ParagraphAnnotation>(_conn);
        }

        /// <inheritdoc />
        public ParagraphAnnotation GetParagraphAnnotation(string bookId, int volumeNumber, int chapterNumber, int paragraphNumber, int number)
        {
            return R.Table(s_ParagraphAnnotationTable).GetAll(R.Array(bookId, volumeNumber, chapterNumber, paragraphNumber, number)).OptArg("index", "BookId_VolumeNumber_ChapterNumber_ParagraphNumber_Number").Nth(0).Default_(default(ParagraphAnnotation)).RunResult<ParagraphAnnotation>(_conn);
        }

        /// <inheritdoc />
        public Task<ParagraphAnnotation> GetParagraphAnnotationAsync(string bookId, int volumeNumber, int chapterNumber, int paragraphNumber, int number)
        {
            return R.Table(s_ParagraphAnnotationTable).GetAll(R.Array(bookId, volumeNumber, chapterNumber, paragraphNumber, number)).OptArg("index", "BookId_VolumeNumber_ChapterNumber_ParagraphNumber_Number").Nth(0).Default_(default(ParagraphAnnotation)).RunResultAsync<ParagraphAnnotation>(_conn);
        }

        /// <inheritdoc />
        public List<ParagraphAnnotation> FindParagraphAnnotations(string bookId, int volumeNumber, int chapterNumber, int paragraphNumber, string annotationFilter, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ParagraphAnnotationTable).GetAll(bookId).OptArg("index", "BookId").Filter(row => row.G("VolumeNumber").Eq(volumeNumber).And(row.G("ChapterNumber").Eq(chapterNumber)).And(row.G("ParagraphNumber").Eq(paragraphNumber)));
            if (!annotationFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Annotation").Match(annotationFilter));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<ParagraphAnnotation>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<ParagraphAnnotation>> FindParagraphAnnotationsAsync(string bookId, int volumeNumber, int chapterNumber, int paragraphNumber, string annotationFilter, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ParagraphAnnotationTable).GetAll(bookId).OptArg("index", "BookId").Filter(row => row.G("VolumeNumber").Eq(volumeNumber).And(row.G("ChapterNumber").Eq(chapterNumber)).And(row.G("ParagraphNumber").Eq(paragraphNumber)));
            if (!annotationFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Annotation").Match(annotationFilter));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<ParagraphAnnotation>>(_conn);
        }

        /// <inheritdoc />
        public List<ParagraphAnnotation> FindParagraphAnnotationsByParagraph(string paragraphId, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ParagraphAnnotationTable).GetAll(paragraphId).OptArg("index", "ParagraphId").Filter(true);
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("Number")) : query.OrderBy("Number");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<ParagraphAnnotation>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<ParagraphAnnotation>> FindParagraphAnnotationsByParagraphAsync(string paragraphId, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ParagraphAnnotationTable).GetAll(paragraphId).OptArg("index", "ParagraphId").Filter(true);
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("Number")) : query.OrderBy("Number");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<ParagraphAnnotation>>(_conn);
        }

        /// <inheritdoc />
        public List<ParagraphAnnotation> FindParagraphAnnotationsByParagraphs(List<string> paragraphIds, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ParagraphAnnotationTable).GetAll(R.Args(paragraphIds.ToArray())).OptArg("index", "ParagraphId").Filter(true);
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("Number")) : query.OrderBy("Number");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<ParagraphAnnotation>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<ParagraphAnnotation>> FindParagraphAnnotationsByParagraphsAsync(List<string> paragraphIds, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ParagraphAnnotationTable).GetAll(R.Args(paragraphIds.ToArray())).OptArg("index", "ParagraphId").Filter(true);
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("Number")) : query.OrderBy("Number");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<ParagraphAnnotation>>(_conn);
        }

        /// <inheritdoc />
        public int GetParagraphAnnotationsCount(string bookId, int volumeNumber, int chapterNumber, int paragraphNumber, string annotationFilter)
        {
            var query = R.Table(s_ParagraphAnnotationTable).GetAll(bookId).OptArg("index", "BookId").Filter(row => row.G("VolumeNumber").Eq(volumeNumber).And(row.G("ChapterNumber").Eq(chapterNumber)).And(row.G("ParagraphNumber").Eq(paragraphNumber)));
            if (!annotationFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Annotation").Match(annotationFilter));
            }
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetParagraphAnnotationsCountAsync(string bookId, int volumeNumber, int chapterNumber, int paragraphNumber, string annotationFilter)
        {
            var query = R.Table(s_ParagraphAnnotationTable).GetAll(bookId).OptArg("index", "BookId").Filter(row => row.G("VolumeNumber").Eq(volumeNumber).And(row.G("ChapterNumber").Eq(chapterNumber)).And(row.G("ParagraphNumber").Eq(paragraphNumber)));
            if (!annotationFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Annotation").Match(annotationFilter));
            }
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public int GetParagraphAnnotationsCountByParagraph(string paragraphId)
        {
            var query = R.Table(s_ParagraphAnnotationTable).GetAll(paragraphId).OptArg("index", "ParagraphId").Filter(true);
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetParagraphAnnotationsCountByParagraphAsync(string paragraphId)
        {
            var query = R.Table(s_ParagraphAnnotationTable).GetAll(paragraphId).OptArg("index", "ParagraphId").Filter(true);
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public ParagraphAnnotation CreateParagraphAnnotation(ParagraphAnnotation newParagraphAnnotation)
        {
            newParagraphAnnotation.ThrowIfNull(nameof(newParagraphAnnotation));
            AssertNoExistingParagraphAnnotation(newParagraphAnnotation);
            newParagraphAnnotation.Id = string.Format("{0}-{1}", newParagraphAnnotation.ParagraphId, newParagraphAnnotation.Number);
            var result = R.Table(s_ParagraphAnnotationTable).Get(newParagraphAnnotation.Id).Replace(newParagraphAnnotation).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<ParagraphAnnotation>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<ParagraphAnnotation> CreateParagraphAnnotationAsync(ParagraphAnnotation newParagraphAnnotation)
        {
            newParagraphAnnotation.ThrowIfNull(nameof(newParagraphAnnotation));
            await AssertNoExistingParagraphAnnotationAsync(newParagraphAnnotation);
            newParagraphAnnotation.Id = string.Format("{0}-{1}", newParagraphAnnotation.ParagraphId, newParagraphAnnotation.Number);
            var result = (await R.Table(s_ParagraphAnnotationTable).Get(newParagraphAnnotation.Id).Replace(newParagraphAnnotation).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<ParagraphAnnotation>()[0].NewValue;
        }

        /// <inheritdoc />
        public ParagraphAnnotation UpdateParagraphAnnotation(ParagraphAnnotation existingParagraphAnnotation, ParagraphAnnotation newParagraphAnnotation)
        {
            existingParagraphAnnotation.ThrowIfNull(nameof(existingParagraphAnnotation));
            newParagraphAnnotation.Id = existingParagraphAnnotation.Id;
            newParagraphAnnotation.BookId = existingParagraphAnnotation.BookId;
            newParagraphAnnotation.VolumeId = existingParagraphAnnotation.VolumeId;
            newParagraphAnnotation.VolumeNumber = existingParagraphAnnotation.VolumeNumber;
            newParagraphAnnotation.ChapterId = existingParagraphAnnotation.ChapterId;
            newParagraphAnnotation.ChapterNumber = existingParagraphAnnotation.ChapterNumber;
            newParagraphAnnotation.ParagraphId = existingParagraphAnnotation.ParagraphId;
            newParagraphAnnotation.ParagraphNumber = existingParagraphAnnotation.ParagraphNumber;
            newParagraphAnnotation.Number = existingParagraphAnnotation.Number;
            var result = R.Table(s_ParagraphAnnotationTable).Get(newParagraphAnnotation.Id).Replace(newParagraphAnnotation).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<ParagraphAnnotation>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<ParagraphAnnotation> UpdateParagraphAnnotationAsync(ParagraphAnnotation existingParagraphAnnotation, ParagraphAnnotation newParagraphAnnotation)
        {
            existingParagraphAnnotation.ThrowIfNull(nameof(existingParagraphAnnotation));
            newParagraphAnnotation.Id = existingParagraphAnnotation.Id;
            newParagraphAnnotation.BookId = existingParagraphAnnotation.BookId;
            newParagraphAnnotation.VolumeId = existingParagraphAnnotation.VolumeId;
            newParagraphAnnotation.VolumeNumber = existingParagraphAnnotation.VolumeNumber;
            newParagraphAnnotation.ChapterId = existingParagraphAnnotation.ChapterId;
            newParagraphAnnotation.ChapterNumber = existingParagraphAnnotation.ChapterNumber;
            newParagraphAnnotation.ParagraphId = existingParagraphAnnotation.ParagraphId;
            newParagraphAnnotation.ParagraphNumber = existingParagraphAnnotation.ParagraphNumber;
            newParagraphAnnotation.Number = existingParagraphAnnotation.Number;
            var result = (await R.Table(s_ParagraphAnnotationTable).Get(newParagraphAnnotation.Id).Replace(newParagraphAnnotation).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<ParagraphAnnotation>()[0].NewValue;
        }

        /// <inheritdoc />
        public void DeleteParagraphAnnotation(string paragraphAnnotationId)
        {
            R.Table(s_ParagraphAnnotationTable).Get(paragraphAnnotationId).Delete().RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task DeleteParagraphAnnotationAsync(string paragraphAnnotationId)
        {
            (await R.Table(s_ParagraphAnnotationTable).Get(paragraphAnnotationId).Delete().RunResultAsync(_conn)).AssertNoErrors();
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