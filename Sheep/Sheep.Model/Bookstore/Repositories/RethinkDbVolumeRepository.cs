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
    ///     基于RethinkDb的卷的存储库。
    /// </summary>
    public class RethinkDbVolumeRepository : IVolumeRepository, IClearable
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RethinkDbVolumeRepository));

        /// <summary>
        ///     RethinkDB 查询器.
        /// </summary>
        public static readonly RethinkDB R = RethinkDB.R;

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
        ///     初始化一个新的<see cref="RethinkDbVolumeRepository" />对象。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="shards">数据库分片数。</param>
        /// <param name="replicas">复制份数。</param>
        /// <param name="createMissingTables">是否创建数据表。</param>
        public RethinkDbVolumeRepository(IConnection conn, int shards, int replicas, bool createMissingTables)
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
                throw new InvalidOperationException(string.Format("One of the tables needed by {0} is missing. You can call {0} constructor with the parameter CreateMissingTables set to 'true'  to create the needed tables.", typeof(RethinkDbVolumeRepository).Name));
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
            if (tables.Contains(s_VolumeTable))
            {
                R.TableDrop(s_VolumeTable).RunResult(_conn).AssertNoErrors().AssertTablesDropped(1);
            }
            CreateTables();
        }

        /// <summary>
        ///     重新创建数据表。
        /// </summary>
        public void CreateTables()
        {
            var tables = R.TableList().RunResult<List<string>>(_conn);
            if (!tables.Contains(s_VolumeTable))
            {
                R.TableCreate(s_VolumeTable).OptArg("primary_key", "Id").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
                R.Table(s_VolumeTable).IndexCreate("BookId_Number", row => R.Array(row.G("BookId"), row.G("Number"))).RunResult(_conn).AssertNoErrors();
                R.Table(s_VolumeTable).IndexCreate("BookId").RunResult(_conn).AssertNoErrors();
                //R.Table(s_VolumeTable).IndexWait().RunResult(_conn).AssertNoErrors();
            }
        }

        /// <summary>
        ///     检测指定的数据表是否存在。
        /// </summary>
        public bool TablesExists()
        {
            var tableNames = new List<string>
                             {
                                 s_VolumeTable
                             };
            var tables = R.TableList().RunResult<List<string>>(_conn);
            return tables.Any(table => tableNames.Contains(table));
        }

        #endregion

        #region 检测卷是否存在

        private void AssertNoExistingVolume(Volume newVolume, Volume exceptForExistingVolume = null)
        {
            var existingVolume = GetVolume(newVolume.BookId, newVolume.Number);
            if (existingVolume != null && (exceptForExistingVolume == null || existingVolume.Id != exceptForExistingVolume.Id))
            {
                throw new ArgumentException(string.Format(Resources.BookWithNumberAlreadyExists, newVolume.BookId, newVolume.Number));
            }
        }

        private async Task AssertNoExistingVolumeAsync(Volume newVolume, Volume exceptForExistingVolume = null)
        {
            var existingVolume = await GetVolumeAsync(newVolume.BookId, newVolume.Number);
            if (existingVolume != null && (exceptForExistingVolume == null || existingVolume.Id != exceptForExistingVolume.Id))
            {
                throw new ArgumentException(string.Format(Resources.BookWithNumberAlreadyExists, newVolume.BookId, newVolume.Number));
            }
        }

        #endregion

        #region IVolumeRepository 接口实现

        /// <inheritdoc />
        public Volume GetVolume(string volumeId)
        {
            return R.Table(s_VolumeTable).Get(volumeId).RunResult<Volume>(_conn);
        }

        /// <inheritdoc />
        public Task<Volume> GetVolumeAsync(string volumeId)
        {
            return R.Table(s_VolumeTable).Get(volumeId).RunResultAsync<Volume>(_conn);
        }

        /// <inheritdoc />
        public Volume GetVolume(string bookId, int number)
        {
            return R.Table(s_VolumeTable).GetAll(R.Array(bookId, number)).OptArg("index", "BookId_Number").Nth(0).Default_(default(Volume)).RunResult<Volume>(_conn);
        }

        /// <inheritdoc />
        public Task<Volume> GetVolumeAsync(string bookId, int number)
        {
            return R.Table(s_VolumeTable).GetAll(R.Array(bookId, number)).OptArg("index", "BookId_Number").Nth(0).Default_(default(Volume)).RunResultAsync<Volume>(_conn);
        }

        /// <inheritdoc />
        public List<Volume> FindVolumes(string bookId, string titleFilter, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_VolumeTable).GetAll(bookId).OptArg("index", "BookId").Filter(true);
            if (!titleFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Title").Match(titleFilter));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<Volume>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Volume>> FindVolumesAsync(string bookId, string titleFilter, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_VolumeTable).GetAll(bookId).OptArg("index", "BookId").Filter(true);
            if (!titleFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Title").Match(titleFilter));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<Volume>>(_conn);
        }

        /// <inheritdoc />
        public List<Volume> FindVolumesByBook(string bookId, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_VolumeTable).GetAll(bookId).OptArg("index", "BookId").Filter(true);
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("Number")) : query.OrderBy("Number");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<Volume>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Volume>> FindVolumesByBookAsync(string bookId, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_VolumeTable).GetAll(bookId).OptArg("index", "BookId").Filter(true);
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("Number")) : query.OrderBy("Number");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<Volume>>(_conn);
        }

        /// <inheritdoc />
        public int GetVolumesCount(string bookId, string titleFilter)
        {
            var query = R.Table(s_VolumeTable).GetAll(bookId).OptArg("index", "BookId").Filter(true);
            if (!titleFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Title").Match(titleFilter));
            }
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetVolumesCountAsync(string bookId, string titleFilter)
        {
            var query = R.Table(s_VolumeTable).GetAll(bookId).OptArg("index", "BookId").Filter(true);
            if (!titleFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Title").Match(titleFilter));
            }
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public int GetVolumesCountByBook(string bookId)
        {
            var query = R.Table(s_VolumeTable).GetAll(bookId).OptArg("index", "BookId").Filter(true);
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetVolumesCountByBookAsync(string bookId)
        {
            var query = R.Table(s_VolumeTable).GetAll(bookId).OptArg("index", "BookId").Filter(true);
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public Volume CreateVolume(Volume newVolume)
        {
            newVolume.ThrowIfNull(nameof(newVolume));
            AssertNoExistingVolume(newVolume);
            newVolume.Id = string.Format("{0}-{1}", newVolume.BookId, newVolume.Number);
            newVolume.ChaptersCount = 0;
            newVolume.SubjectsCount = 0;
            var result = R.Table(s_VolumeTable).Get(newVolume.Id).Replace(newVolume).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Volume>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Volume> CreateVolumeAsync(Volume newVolume)
        {
            newVolume.ThrowIfNull(nameof(newVolume));
            await AssertNoExistingVolumeAsync(newVolume);
            newVolume.Id = string.Format("{0}-{1}", newVolume.BookId, newVolume.Number);
            newVolume.ChaptersCount = 0;
            newVolume.SubjectsCount = 0;
            var result = (await R.Table(s_VolumeTable).Get(newVolume.Id).Replace(newVolume).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Volume>()[0].NewValue;
        }

        /// <inheritdoc />
        public Volume UpdateVolume(Volume existingVolume, Volume newVolume)
        {
            existingVolume.ThrowIfNull(nameof(existingVolume));
            newVolume.Id = existingVolume.Id;
            newVolume.BookId = existingVolume.BookId;
            newVolume.Number = existingVolume.Number;
            var result = R.Table(s_VolumeTable).Get(newVolume.Id).Replace(newVolume).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Volume>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Volume> UpdateVolumeAsync(Volume existingVolume, Volume newVolume)
        {
            existingVolume.ThrowIfNull(nameof(existingVolume));
            newVolume.Id = existingVolume.Id;
            newVolume.BookId = existingVolume.BookId;
            newVolume.Number = existingVolume.Number;
            var result = (await R.Table(s_VolumeTable).Get(newVolume.Id).Replace(newVolume).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Volume>()[0].NewValue;
        }

        /// <inheritdoc />
        public void DeleteVolume(string volumeId)
        {
            R.Table(s_VolumeTable).Get(volumeId).Delete().RunResult(_conn).AssertNoErrors();
            R.Table(s_VolumeAnnotationTable).GetAll(volumeId).OptArg("index", "VolumeId").Delete().RunResult(_conn).AssertNoErrors();
            R.Table(s_SubjectTable).GetAll(volumeId).OptArg("index", "VolumeId").Delete().RunResult(_conn).AssertNoErrors();
            R.Table(s_ChapterTable).GetAll(volumeId).OptArg("index", "VolumeId").Delete().RunResult(_conn).AssertNoErrors();
            R.Table(s_ChapterAnnotationTable).GetAll(volumeId).OptArg("index", "VolumeId").Delete().RunResult(_conn).AssertNoErrors();
            R.Table(s_ParagraphTable).GetAll(volumeId).OptArg("index", "VolumeId").Delete().RunResult(_conn).AssertNoErrors();
            R.Table(s_ParagraphAnnotationTable).GetAll(volumeId).OptArg("index", "VolumeId").Delete().RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task DeleteVolumeAsync(string volumeId)
        {
            (await R.Table(s_VolumeTable).Get(volumeId).Delete().RunResultAsync(_conn)).AssertNoErrors();
            (await R.Table(s_VolumeAnnotationTable).GetAll(volumeId).OptArg("index", "VolumeId").Delete().RunResultAsync(_conn)).AssertNoErrors();
            (await R.Table(s_SubjectTable).GetAll(volumeId).OptArg("index", "VolumeId").Delete().RunResultAsync(_conn)).AssertNoErrors();
            (await R.Table(s_ChapterTable).GetAll(volumeId).OptArg("index", "VolumeId").Delete().RunResultAsync(_conn)).AssertNoErrors();
            (await R.Table(s_ChapterAnnotationTable).GetAll(volumeId).OptArg("index", "VolumeId").Delete().RunResultAsync(_conn)).AssertNoErrors();
            (await R.Table(s_ParagraphTable).GetAll(volumeId).OptArg("index", "VolumeId").Delete().RunResultAsync(_conn)).AssertNoErrors();
            (await R.Table(s_ParagraphAnnotationTable).GetAll(volumeId).OptArg("index", "VolumeId").RunResultAsync(_conn)).AssertNoErrors();
        }

        /// <inheritdoc />
        public Volume IncrementVolumeChaptersCount(string volumeId, int count)
        {
            var result = R.Table(s_VolumeTable).Get(volumeId).Update(row => R.HashMap("ChaptersCount", row.G("ChaptersCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Volume>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Volume> IncrementVolumeChaptersCountAsync(string volumeId, int count)
        {
            var result = (await R.Table(s_VolumeTable).Get(volumeId).Update(row => R.HashMap("ChaptersCount", row.G("ChaptersCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Volume>()[0].NewValue;
        }

        /// <inheritdoc />
        public Volume IncrementVolumeSubjectsCount(string volumeId, int count)
        {
            var result = R.Table(s_VolumeTable).Get(volumeId).Update(row => R.HashMap("SubjectsCount", row.G("SubjectsCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Volume>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Volume> IncrementVolumeSubjectsCountAsync(string volumeId, int count)
        {
            var result = (await R.Table(s_VolumeTable).Get(volumeId).Update(row => R.HashMap("SubjectsCount", row.G("SubjectsCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Volume>()[0].NewValue;
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