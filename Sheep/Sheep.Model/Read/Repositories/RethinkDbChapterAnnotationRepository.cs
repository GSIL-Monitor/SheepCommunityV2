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
using Sheep.Model.Read.Entities;

namespace Sheep.Model.Read.Repositories
{
    /// <summary>
    ///     基于RethinkDb的章注释的存储库。
    /// </summary>
    public class RethinkDbChapterAnnotationRepository : IChapterAnnotationRepository, IClearable
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RethinkDbChapterAnnotationRepository));

        /// <summary>
        ///     RethinkDB 查询器.
        /// </summary>
        public static readonly RethinkDB R = RethinkDB.R;

        /// <summary>
        ///     章注释的数据表名。
        /// </summary>
        private static readonly string s_ChapterAnnotationTable = typeof(ChapterAnnotation).Name;

        #endregion

        #region 属性

        private readonly IConnection _conn;
        private readonly int _shards;
        private readonly int _replicas;

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="RethinkDbChapterAnnotationRepository" />对象。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="shards">数据库分片数。</param>
        /// <param name="replicas">复制份数。</param>
        /// <param name="createMissingTables">是否创建数据表。</param>
        public RethinkDbChapterAnnotationRepository(IConnection conn, int shards, int replicas, bool createMissingTables)
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
                throw new InvalidOperationException(string.Format("One of the tables needed by {0} is missing. You can call {0} constructor with the parameter CreateMissingTables set to 'true'  to create the needed tables.", typeof(RethinkDbChapterAnnotationRepository).Name));
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
            if (tables.Contains(s_ChapterAnnotationTable))
            {
                R.TableDrop(s_ChapterAnnotationTable).RunResult(_conn).AssertNoErrors().AssertTablesDropped(1);
            }
            CreateTables();
        }

        /// <summary>
        ///     重新创建数据表。
        /// </summary>
        public void CreateTables()
        {
            var tables = R.TableList().RunResult<List<string>>(_conn);
            if (!tables.Contains(s_ChapterAnnotationTable))
            {
                R.TableCreate(s_ChapterAnnotationTable).OptArg("primary_key", "Id").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
                R.Table(s_ChapterAnnotationTable).IndexCreate("ChapterId").RunResult(_conn).AssertNoErrors();
                //R.Table(s_ChapterAnnotationTable).IndexWait().RunResult(_conn).AssertNoErrors();
            }
        }

        /// <summary>
        ///     检测指定的数据表是否存在。
        /// </summary>
        public bool TablesExists()
        {
            var tableNames = new List<string>
                             {
                                 s_ChapterAnnotationTable
                             };
            var tables = R.TableList().RunResult<List<string>>(_conn);
            return tables.Any(table => tableNames.Contains(table));
        }

        #endregion

        #region 检测章注释是否存在

        #endregion

        #region IChapterAnnotationRepository 接口实现

        /// <inheritdoc />
        public ChapterAnnotation GetChapterAnnotation(string chapterAnnotationId)
        {
            return R.Table(s_ChapterAnnotationTable).Get(chapterAnnotationId).RunResult<ChapterAnnotation>(_conn);
        }

        /// <inheritdoc />
        public Task<ChapterAnnotation> GetChapterAnnotationAsync(string chapterAnnotationId)
        {
            return R.Table(s_ChapterAnnotationTable).Get(chapterAnnotationId).RunResultAsync<ChapterAnnotation>(_conn);
        }

        /// <inheritdoc />
        public List<ChapterAnnotation> FindChapterAnnotations(string annotationFilter, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ChapterAnnotationTable).Filter(true);
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 500).RunResult<List<ChapterAnnotation>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<ChapterAnnotation>> FindChapterAnnotationsAsync(string annotationFilter, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ChapterAnnotationTable).Filter(true);
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 500).RunResultAsync<List<ChapterAnnotation>>(_conn);
        }

        /// <inheritdoc />
        public List<ChapterAnnotation> FindChapterAnnotationsByChapter(string chapterId, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ChapterAnnotationTable).GetAll(chapterId).OptArg("index", "ChapterId").Filter(true);
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("Number")) : query.OrderBy("Number");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 500).RunResult<List<ChapterAnnotation>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<ChapterAnnotation>> FindChapterAnnotationsByChapterAsync(string chapterId, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ChapterAnnotationTable).GetAll(chapterId).OptArg("index", "ChapterId").Filter(true);
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("Number")) : query.OrderBy("Number");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 500).RunResultAsync<List<ChapterAnnotation>>(_conn);
        }

        /// <inheritdoc />
        public int GetChapterAnnotationsCount(string annotationFilter)
        {
            var query = R.Table(s_ChapterAnnotationTable).Filter(true);
            if (!annotationFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Annotation").Match(annotationFilter));
            }
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetChapterAnnotationsCountAsync(string annotationFilter)
        {
            var query = R.Table(s_ChapterAnnotationTable).Filter(true);
            if (!annotationFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Annotation").Match(annotationFilter));
            }
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public int GetChapterAnnotationsCountByChapter(string chapterId)
        {
            var query = R.Table(s_ChapterAnnotationTable).GetAll(chapterId).OptArg("index", "ChapterId").Filter(true);
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetChapterAnnotationsCountByChapterAsync(string chapterId)
        {
            var query = R.Table(s_ChapterAnnotationTable).GetAll(chapterId).OptArg("index", "ChapterId").Filter(true);
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public ChapterAnnotation CreateChapterAnnotation(ChapterAnnotation newChapterAnnotation)
        {
            newChapterAnnotation.ThrowIfNull(nameof(newChapterAnnotation));
            newChapterAnnotation.Id = newChapterAnnotation.Id.IsNullOrEmpty() ? new Base36IdGenerator(10, 4, 4).NewId().ToLower() : newChapterAnnotation.Id;
            var result = R.Table(s_ChapterAnnotationTable).Get(newChapterAnnotation.Id).Replace(newChapterAnnotation).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<ChapterAnnotation>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<ChapterAnnotation> CreateChapterAnnotationAsync(ChapterAnnotation newChapterAnnotation)
        {
            newChapterAnnotation.ThrowIfNull(nameof(newChapterAnnotation));
            newChapterAnnotation.Id = newChapterAnnotation.Id.IsNullOrEmpty() ? new Base36IdGenerator(10, 4, 4).NewId().ToLower() : newChapterAnnotation.Id;
            var result = (await R.Table(s_ChapterAnnotationTable).Get(newChapterAnnotation.Id).Replace(newChapterAnnotation).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<ChapterAnnotation>()[0].NewValue;
        }

        /// <inheritdoc />
        public ChapterAnnotation UpdateChapterAnnotation(ChapterAnnotation existingChapterAnnotation, ChapterAnnotation newChapterAnnotation)
        {
            existingChapterAnnotation.ThrowIfNull(nameof(existingChapterAnnotation));
            newChapterAnnotation.Id = existingChapterAnnotation.Id;
            newChapterAnnotation.ChapterId = existingChapterAnnotation.ChapterId;
            newChapterAnnotation.Number = existingChapterAnnotation.Number;
            var result = R.Table(s_ChapterAnnotationTable).Get(newChapterAnnotation.Id).Replace(newChapterAnnotation).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<ChapterAnnotation>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<ChapterAnnotation> UpdateChapterAnnotationAsync(ChapterAnnotation existingChapterAnnotation, ChapterAnnotation newChapterAnnotation)
        {
            existingChapterAnnotation.ThrowIfNull(nameof(existingChapterAnnotation));
            newChapterAnnotation.Id = existingChapterAnnotation.Id;
            newChapterAnnotation.ChapterId = existingChapterAnnotation.ChapterId;
            newChapterAnnotation.Number = existingChapterAnnotation.Number;
            var result = (await R.Table(s_ChapterAnnotationTable).Get(newChapterAnnotation.Id).Replace(newChapterAnnotation).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<ChapterAnnotation>()[0].NewValue;
        }

        /// <inheritdoc />
        public void DeleteChapterAnnotation(string chapterAnnotationId)
        {
            R.Table(s_ChapterAnnotationTable).Get(chapterAnnotationId).Delete().RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task DeleteChapterAnnotationAsync(string chapterAnnotationId)
        {
            (await R.Table(s_ChapterAnnotationTable).Get(chapterAnnotationId).Delete().RunResultAsync(_conn)).AssertNoErrors();
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