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
using Sheep.Model.Read.Entities;

namespace Sheep.Model.Read.Repositories
{
    /// <summary>
    ///     基于RethinkDb的卷注释的存储库。
    /// </summary>
    public class RethinkDbVolumeAnnotationRepository : IVolumeAnnotationRepository, IClearable
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RethinkDbVolumeAnnotationRepository));

        /// <summary>
        ///     RethinkDB 查询器.
        /// </summary>
        public static readonly RethinkDB R = RethinkDB.R;

        /// <summary>
        ///     卷注释的数据表名。
        /// </summary>
        private static readonly string s_VolumeAnnotationTable = typeof(VolumeAnnotation).Name;

        #endregion

        #region 属性

        private readonly IConnection _conn;
        private readonly int _shards;
        private readonly int _replicas;

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="RethinkDbVolumeAnnotationRepository" />对象。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="shards">数据库分片数。</param>
        /// <param name="replicas">复制份数。</param>
        /// <param name="createMissingTables">是否创建数据表。</param>
        public RethinkDbVolumeAnnotationRepository(IConnection conn, int shards, int replicas, bool createMissingTables)
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
                throw new InvalidOperationException(string.Format("One of the tables needed by {0} is missing. You can call {0} constructor with the parameter CreateMissingTables set to 'true'  to create the needed tables.", typeof(RethinkDbVolumeAnnotationRepository).Name));
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
            if (tables.Contains(s_VolumeAnnotationTable))
            {
                R.TableDrop(s_VolumeAnnotationTable).RunResult(_conn).AssertNoErrors().AssertTablesDropped(1);
            }
            CreateTables();
        }

        /// <summary>
        ///     重新创建数据表。
        /// </summary>
        public void CreateTables()
        {
            var tables = R.TableList().RunResult<List<string>>(_conn);
            if (!tables.Contains(s_VolumeAnnotationTable))
            {
                R.TableCreate(s_VolumeAnnotationTable).OptArg("primary_key", "Id").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
                R.Table(s_VolumeAnnotationTable).IndexCreate("VolumeId_Number", row => R.Array(row.G("VolumeId"), row.G("Number"))).RunResult(_conn).AssertNoErrors();
                R.Table(s_VolumeAnnotationTable).IndexCreate("BookId").RunResult(_conn).AssertNoErrors();
                R.Table(s_VolumeAnnotationTable).IndexCreate("VolumeId").RunResult(_conn).AssertNoErrors();
                //R.Table(s_VolumeAnnotationTable).IndexWait().RunResult(_conn).AssertNoErrors();
            }
        }

        /// <summary>
        ///     检测指定的数据表是否存在。
        /// </summary>
        public bool TablesExists()
        {
            var tableNames = new List<string>
                             {
                                 s_VolumeAnnotationTable
                             };
            var tables = R.TableList().RunResult<List<string>>(_conn);
            return tables.Any(table => tableNames.Contains(table));
        }

        #endregion

        #region 检测卷注释是否存在

        private void AssertNoExistingVolumeAnnotation(VolumeAnnotation newVolumeAnnotation, VolumeAnnotation exceptForExistingVolumeAnnotation = null)
        {
            var existingVolumeAnnotation = GetVolumeAnnotation(newVolumeAnnotation.VolumeId, newVolumeAnnotation.Number);
            if (existingVolumeAnnotation != null && (exceptForExistingVolumeAnnotation == null || existingVolumeAnnotation.Id != exceptForExistingVolumeAnnotation.Id))
            {
                throw new ArgumentException(string.Format(Resources.VolumeWithNumberAlreadyExists, newVolumeAnnotation.VolumeId, newVolumeAnnotation.Number));
            }
        }

        private async Task AssertNoExistingVolumeAnnotationAsync(VolumeAnnotation newVolumeAnnotation, VolumeAnnotation exceptForExistingVolumeAnnotation = null)
        {
            var existingVolumeAnnotation = await GetVolumeAnnotationAsync(newVolumeAnnotation.VolumeId, newVolumeAnnotation.Number);
            if (existingVolumeAnnotation != null && (exceptForExistingVolumeAnnotation == null || existingVolumeAnnotation.Id != exceptForExistingVolumeAnnotation.Id))
            {
                throw new ArgumentException(string.Format(Resources.VolumeWithNumberAlreadyExists, newVolumeAnnotation.VolumeId, newVolumeAnnotation.Number));
            }
        }

        #endregion

        #region IVolumeAnnotationRepository 接口实现

        /// <inheritdoc />
        public VolumeAnnotation GetVolumeAnnotation(string volumeAnnotationId)
        {
            return R.Table(s_VolumeAnnotationTable).Get(volumeAnnotationId).RunResult<VolumeAnnotation>(_conn);
        }

        /// <inheritdoc />
        public Task<VolumeAnnotation> GetVolumeAnnotationAsync(string volumeAnnotationId)
        {
            return R.Table(s_VolumeAnnotationTable).Get(volumeAnnotationId).RunResultAsync<VolumeAnnotation>(_conn);
        }

        /// <inheritdoc />
        public VolumeAnnotation GetVolumeAnnotation(string volumeId, int number)
        {
            return R.Table(s_VolumeAnnotationTable).GetAll(R.Array(volumeId, number)).OptArg("index", "VolumeId_Number").Nth(0).Default_(default(VolumeAnnotation)).RunResult<VolumeAnnotation>(_conn);
        }

        /// <inheritdoc />
        public Task<VolumeAnnotation> GetVolumeAnnotationAsync(string volumeId, int number)
        {
            return R.Table(s_VolumeAnnotationTable).GetAll(R.Array(volumeId, number)).OptArg("index", "VolumeId_Number").Nth(0).Default_(default(VolumeAnnotation)).RunResultAsync<VolumeAnnotation>(_conn);
        }

        /// <inheritdoc />
        public List<VolumeAnnotation> FindVolumeAnnotations(string bookId, string annotationFilter, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_VolumeAnnotationTable).GetAll(bookId).OptArg("index", "BookId").Filter(true);
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 500).RunResult<List<VolumeAnnotation>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<VolumeAnnotation>> FindVolumeAnnotationsAsync(string bookId, string annotationFilter, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_VolumeAnnotationTable).GetAll(bookId).OptArg("index", "BookId").Filter(true);
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 500).RunResultAsync<List<VolumeAnnotation>>(_conn);
        }

        /// <inheritdoc />
        public List<VolumeAnnotation> FindVolumeAnnotationsByVolume(string volumeId, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_VolumeAnnotationTable).GetAll(volumeId).OptArg("index", "VolumeId").Filter(true);
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("Number")) : query.OrderBy("Number");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 500).RunResult<List<VolumeAnnotation>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<VolumeAnnotation>> FindVolumeAnnotationsByVolumeAsync(string volumeId, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_VolumeAnnotationTable).GetAll(volumeId).OptArg("index", "VolumeId").Filter(true);
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("Number")) : query.OrderBy("Number");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 500).RunResultAsync<List<VolumeAnnotation>>(_conn);
        }

        /// <inheritdoc />
        public List<VolumeAnnotation> FindVolumeAnnotationsByVolumes(IEnumerable<string> volumeIds, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_VolumeAnnotationTable).GetAll(R.Args(volumeIds.ToArray())).OptArg("index", "VolumeId").Filter(true);
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("Number")) : query.OrderBy("Number");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 500).RunResult<List<VolumeAnnotation>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<VolumeAnnotation>> FindVolumeAnnotationsByVolumesAsync(IEnumerable<string> volumeIds, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_VolumeAnnotationTable).GetAll(R.Args(volumeIds.ToArray())).OptArg("index", "VolumeId").Filter(true);
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("Number")) : query.OrderBy("Number");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 500).RunResultAsync<List<VolumeAnnotation>>(_conn);
        }

        /// <inheritdoc />
        public int GetVolumeAnnotationsCount(string bookId, string annotationFilter)
        {
            var query = R.Table(s_VolumeAnnotationTable).GetAll(bookId).OptArg("index", "BookId").Filter(true);
            if (!annotationFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Annotation").Match(annotationFilter));
            }
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetVolumeAnnotationsCountAsync(string bookId, string annotationFilter)
        {
            var query = R.Table(s_VolumeAnnotationTable).GetAll(bookId).OptArg("index", "BookId").Filter(true);
            if (!annotationFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Annotation").Match(annotationFilter));
            }
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public int GetVolumeAnnotationsCountByVolume(string volumeId)
        {
            var query = R.Table(s_VolumeAnnotationTable).GetAll(volumeId).OptArg("index", "VolumeId").Filter(true);
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetVolumeAnnotationsCountByVolumeAsync(string volumeId)
        {
            var query = R.Table(s_VolumeAnnotationTable).GetAll(volumeId).OptArg("index", "VolumeId").Filter(true);
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public VolumeAnnotation CreateVolumeAnnotation(VolumeAnnotation newVolumeAnnotation)
        {
            newVolumeAnnotation.ThrowIfNull(nameof(newVolumeAnnotation));
            AssertNoExistingVolumeAnnotation(newVolumeAnnotation);
            newVolumeAnnotation.Id = string.Format("{0}-{1}", newVolumeAnnotation.VolumeId, newVolumeAnnotation.Number);
            var result = R.Table(s_VolumeAnnotationTable).Get(newVolumeAnnotation.Id).Replace(newVolumeAnnotation).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<VolumeAnnotation>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<VolumeAnnotation> CreateVolumeAnnotationAsync(VolumeAnnotation newVolumeAnnotation)
        {
            newVolumeAnnotation.ThrowIfNull(nameof(newVolumeAnnotation));
            await AssertNoExistingVolumeAnnotationAsync(newVolumeAnnotation);
            newVolumeAnnotation.Id = string.Format("{0}-{1}", newVolumeAnnotation.VolumeId, newVolumeAnnotation.Number);
            var result = (await R.Table(s_VolumeAnnotationTable).Get(newVolumeAnnotation.Id).Replace(newVolumeAnnotation).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<VolumeAnnotation>()[0].NewValue;
        }

        /// <inheritdoc />
        public VolumeAnnotation UpdateVolumeAnnotation(VolumeAnnotation existingVolumeAnnotation, VolumeAnnotation newVolumeAnnotation)
        {
            existingVolumeAnnotation.ThrowIfNull(nameof(existingVolumeAnnotation));
            newVolumeAnnotation.Id = existingVolumeAnnotation.Id;
            newVolumeAnnotation.BookId = existingVolumeAnnotation.BookId;
            newVolumeAnnotation.VolumeId = existingVolumeAnnotation.VolumeId;
            newVolumeAnnotation.Number = existingVolumeAnnotation.Number;
            var result = R.Table(s_VolumeAnnotationTable).Get(newVolumeAnnotation.Id).Replace(newVolumeAnnotation).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<VolumeAnnotation>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<VolumeAnnotation> UpdateVolumeAnnotationAsync(VolumeAnnotation existingVolumeAnnotation, VolumeAnnotation newVolumeAnnotation)
        {
            existingVolumeAnnotation.ThrowIfNull(nameof(existingVolumeAnnotation));
            newVolumeAnnotation.Id = existingVolumeAnnotation.Id;
            newVolumeAnnotation.BookId = existingVolumeAnnotation.BookId;
            newVolumeAnnotation.VolumeId = existingVolumeAnnotation.VolumeId;
            newVolumeAnnotation.Number = existingVolumeAnnotation.Number;
            var result = (await R.Table(s_VolumeAnnotationTable).Get(newVolumeAnnotation.Id).Replace(newVolumeAnnotation).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<VolumeAnnotation>()[0].NewValue;
        }

        /// <inheritdoc />
        public void DeleteVolumeAnnotation(string volumeAnnotationId)
        {
            R.Table(s_VolumeAnnotationTable).Get(volumeAnnotationId).Delete().RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task DeleteVolumeAnnotationAsync(string volumeAnnotationId)
        {
            (await R.Table(s_VolumeAnnotationTable).Get(volumeAnnotationId).Delete().RunResultAsync(_conn)).AssertNoErrors();
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