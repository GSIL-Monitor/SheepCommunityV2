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
    ///     基于RethinkDb的主题的存储库。
    /// </summary>
    public class RethinkDbSubjectRepository : ISubjectRepository, IClearable
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RethinkDbSubjectRepository));

        /// <summary>
        ///     RethinkDB 查询器.
        /// </summary>
        public static readonly RethinkDB R = RethinkDB.R;

        /// <summary>
        ///     主题的数据表名。
        /// </summary>
        private static readonly string s_SubjectTable = typeof(Subject).Name;

        /// <summary>
        ///     节的数据表名。
        /// </summary>
        private static readonly string s_ParagraphTable = typeof(Paragraph).Name;

        #endregion

        #region 属性

        private readonly IConnection _conn;
        private readonly int _shards;
        private readonly int _replicas;

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="RethinkDbSubjectRepository" />对象。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="shards">数据库分片数。</param>
        /// <param name="replicas">复制份数。</param>
        /// <param name="createMissingTables">是否创建数据表。</param>
        public RethinkDbSubjectRepository(IConnection conn, int shards, int replicas, bool createMissingTables)
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
                throw new InvalidOperationException(string.Format("One of the tables needed by {0} is missing. You can call {0} constructor with the parameter CreateMissingTables set to 'true'  to create the needed tables.", typeof(RethinkDbSubjectRepository).Name));
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
            if (tables.Contains(s_SubjectTable))
            {
                R.TableDrop(s_SubjectTable).RunResult(_conn).AssertNoErrors().AssertTablesDropped(1);
            }
            CreateTables();
        }

        /// <summary>
        ///     重新创建数据表。
        /// </summary>
        public void CreateTables()
        {
            var tables = R.TableList().RunResult<List<string>>(_conn);
            if (!tables.Contains(s_SubjectTable))
            {
                R.TableCreate(s_SubjectTable).OptArg("primary_key", "Id").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
                R.Table(s_SubjectTable).IndexCreate("BookId_VolumeNumber_Number", row => R.Array(row.G("BookId"), row.G("VolumeNumber"), row.G("Number"))).RunResult(_conn).AssertNoErrors();
                R.Table(s_SubjectTable).IndexCreate("VolumeId_Number", row => R.Array(row.G("VolumeId"), row.G("Number"))).RunResult(_conn).AssertNoErrors();
                R.Table(s_SubjectTable).IndexCreate("BookId").RunResult(_conn).AssertNoErrors();
                R.Table(s_SubjectTable).IndexCreate("VolumeId").RunResult(_conn).AssertNoErrors();
                //R.Table(s_SubjectTable).IndexWait().RunResult(_conn).AssertNoErrors();
            }
        }

        /// <summary>
        ///     检测指定的数据表是否存在。
        /// </summary>
        public bool TablesExists()
        {
            var tableNames = new List<string>
                             {
                                 s_SubjectTable
                             };
            var tables = R.TableList().RunResult<List<string>>(_conn);
            return tables.Any(table => tableNames.Contains(table));
        }

        #endregion

        #region 检测主题是否存在

        private void AssertNoExistingSubject(Subject newSubject, Subject exceptForExistingSubject = null)
        {
            var existingSubject = GetSubject(newSubject.VolumeId, newSubject.Number);
            if (existingSubject != null && (exceptForExistingSubject == null || existingSubject.Id != exceptForExistingSubject.Id))
            {
                throw new ArgumentException(string.Format(Resources.VolumeWithNumberAlreadyExists, newSubject.VolumeId, newSubject.Number));
            }
            existingSubject = GetSubject(newSubject.BookId, newSubject.VolumeNumber, newSubject.Number);
            if (existingSubject != null && (exceptForExistingSubject == null || existingSubject.Id != exceptForExistingSubject.Id))
            {
                throw new ArgumentException(string.Format(Resources.BookWithVolumeAndNumberAlreadyExists, newSubject.BookId, newSubject.VolumeNumber, newSubject.Number));
            }
        }

        private async Task AssertNoExistingSubjectAsync(Subject newSubject, Subject exceptForExistingSubject = null)
        {
            var existingSubject = await GetSubjectAsync(newSubject.VolumeId, newSubject.Number);
            if (existingSubject != null && (exceptForExistingSubject == null || existingSubject.Id != exceptForExistingSubject.Id))
            {
                throw new ArgumentException(string.Format(Resources.VolumeWithNumberAlreadyExists, newSubject.VolumeId, newSubject.Number));
            }
            existingSubject = await GetSubjectAsync(newSubject.BookId, newSubject.VolumeNumber, newSubject.Number);
            if (existingSubject != null && (exceptForExistingSubject == null || existingSubject.Id != exceptForExistingSubject.Id))
            {
                throw new ArgumentException(string.Format(Resources.BookWithVolumeAndNumberAlreadyExists, newSubject.BookId, newSubject.VolumeNumber, newSubject.Number));
            }
        }

        #endregion

        #region ISubjectRepository 接口实现

        /// <inheritdoc />
        public Subject GetSubject(string subjectId)
        {
            return R.Table(s_SubjectTable).Get(subjectId).RunResult<Subject>(_conn);
        }

        /// <inheritdoc />
        public Task<Subject> GetSubjectAsync(string subjectId)
        {
            return R.Table(s_SubjectTable).Get(subjectId).RunResultAsync<Subject>(_conn);
        }

        /// <inheritdoc />
        public Subject GetSubject(string volumeId, int number)
        {
            return R.Table(s_SubjectTable).GetAll(R.Array(volumeId, number)).OptArg("index", "VolumeId_Number").Nth(0).Default_(default(Subject)).RunResult<Subject>(_conn);
        }

        /// <inheritdoc />
        public Task<Subject> GetSubjectAsync(string volumeId, int number)
        {
            return R.Table(s_SubjectTable).GetAll(R.Array(volumeId, number)).OptArg("index", "VolumeId_Number").Nth(0).Default_(default(Subject)).RunResultAsync<Subject>(_conn);
        }

        /// <inheritdoc />
        public Subject GetSubject(string bookId, int volumeNumber, int number)
        {
            return R.Table(s_SubjectTable).GetAll(R.Array(bookId, volumeNumber, number)).OptArg("index", "BookId_VolumeNumber_Number").Nth(0).Default_(default(Subject)).RunResult<Subject>(_conn);
        }

        /// <inheritdoc />
        public Task<Subject> GetSubjectAsync(string bookId, int volumeNumber, int number)
        {
            return R.Table(s_SubjectTable).GetAll(R.Array(bookId, volumeNumber, number)).OptArg("index", "BookId_VolumeNumber_Number").Nth(0).Default_(default(Subject)).RunResultAsync<Subject>(_conn);
        }

        /// <inheritdoc />
        public List<Subject> FindSubjects(string bookId, int volumeNumber, string titleFilter, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_SubjectTable).GetAll(bookId).OptArg("index", "BookId").Filter(row => row.G("VolumeNumber").Eq(volumeNumber));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<Subject>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Subject>> FindSubjectsAsync(string bookId, int volumeNumber, string titleFilter, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_SubjectTable).GetAll(bookId).OptArg("index", "BookId").Filter(row => row.G("VolumeNumber").Eq(volumeNumber));
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<Subject>>(_conn);
        }

        /// <inheritdoc />
        public List<Subject> FindSubjectsByVolume(string volumeId, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_SubjectTable).GetAll(volumeId).OptArg("index", "VolumeId").Filter(true);
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("Number")) : query.OrderBy("Number");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResult<List<Subject>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Subject>> FindSubjectsByVolumeAsync(string volumeId, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_SubjectTable).GetAll(volumeId).OptArg("index", "VolumeId").Filter(true);
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("Number")) : query.OrderBy("Number");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 100000).RunResultAsync<List<Subject>>(_conn);
        }

        /// <inheritdoc />
        public int GetSubjectsCount(string bookId, int volumeNumber, string titleFilter)
        {
            var query = R.Table(s_SubjectTable).GetAll(bookId).OptArg("index", "BookId").Filter(row => row.G("VolumeNumber").Eq(volumeNumber));
            if (!titleFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Title").Match(titleFilter));
            }
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetSubjectsCountAsync(string bookId, int volumeNumber, string titleFilter)
        {
            var query = R.Table(s_SubjectTable).GetAll(bookId).OptArg("index", "BookId").Filter(row => row.G("VolumeNumber").Eq(volumeNumber));
            if (!titleFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Title").Match(titleFilter));
            }
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public int GetSubjectsCountByVolume(string volumeId)
        {
            var query = R.Table(s_SubjectTable).GetAll(volumeId).OptArg("index", "VolumeId").Filter(true);
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetSubjectsCountByVolumeAsync(string volumeId)
        {
            var query = R.Table(s_SubjectTable).GetAll(volumeId).OptArg("index", "VolumeId").Filter(true);
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public Subject CreateSubject(Subject newSubject)
        {
            newSubject.ThrowIfNull(nameof(newSubject));
            AssertNoExistingSubject(newSubject);
            newSubject.Id = string.Format("{0}-{1}", newSubject.VolumeId, newSubject.Number);
            var result = R.Table(s_SubjectTable).Get(newSubject.Id).Replace(newSubject).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Subject>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Subject> CreateSubjectAsync(Subject newSubject)
        {
            newSubject.ThrowIfNull(nameof(newSubject));
            await AssertNoExistingSubjectAsync(newSubject);
            newSubject.Id = string.Format("{0}-{1}", newSubject.VolumeId, newSubject.Number);
            var result = (await R.Table(s_SubjectTable).Get(newSubject.Id).Replace(newSubject).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Subject>()[0].NewValue;
        }

        /// <inheritdoc />
        public Subject UpdateSubject(Subject existingSubject, Subject newSubject)
        {
            existingSubject.ThrowIfNull(nameof(existingSubject));
            newSubject.Id = existingSubject.Id;
            newSubject.BookId = existingSubject.BookId;
            newSubject.VolumeId = existingSubject.VolumeId;
            newSubject.VolumeNumber = existingSubject.VolumeNumber;
            newSubject.Number = existingSubject.Number;
            var result = R.Table(s_SubjectTable).Get(newSubject.Id).Replace(newSubject).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Subject>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Subject> UpdateSubjectAsync(Subject existingSubject, Subject newSubject)
        {
            existingSubject.ThrowIfNull(nameof(existingSubject));
            newSubject.Id = existingSubject.Id;
            newSubject.BookId = existingSubject.BookId;
            newSubject.VolumeId = existingSubject.VolumeId;
            newSubject.VolumeNumber = existingSubject.VolumeNumber;
            newSubject.Number = existingSubject.Number;
            var result = (await R.Table(s_SubjectTable).Get(newSubject.Id).Replace(newSubject).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Subject>()[0].NewValue;
        }

        /// <inheritdoc />
        public void DeleteSubject(string subjectId)
        {
            R.Table(s_SubjectTable).Get(subjectId).Delete().RunResult(_conn).AssertNoErrors();
            R.Table(s_ParagraphTable).GetAll(subjectId).OptArg("index", "SubjectId").Update(row => R.HashMap("SubjectId", null)).RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task DeleteSubjectAsync(string subjectId)
        {
            (await R.Table(s_SubjectTable).Get(subjectId).Delete().RunResultAsync(_conn)).AssertNoErrors();
            (await R.Table(s_ParagraphTable).GetAll(subjectId).OptArg("index", "SubjectId").Update(row => R.HashMap("SubjectId", null)).RunResultAsync(_conn)).AssertNoErrors();
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