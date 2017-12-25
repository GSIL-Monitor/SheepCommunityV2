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
using Sheep.Model.Content.Entities;

namespace Sheep.Model.Content.Repositories
{
    /// <summary>
    ///     基于RethinkDb的帖子的存储库。
    /// </summary>
    public class RethinkDbPostRepository : IPostRepository, IClearable
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RethinkDbPostRepository));

        /// <summary>
        ///     RethinkDB 查询器.
        /// </summary>
        public static readonly RethinkDB R = RethinkDB.R;

        /// <summary>
        ///     帖子的数据表名。
        /// </summary>
        private static readonly string s_PostTable = typeof(Post).Name;

        #endregion

        #region 属性

        private readonly IConnection _conn;
        private readonly int _shards;
        private readonly int _replicas;

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="RethinkDbPostRepository" />对象。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="shards">数据库分片数。</param>
        /// <param name="replicas">复制份数。</param>
        /// <param name="createMissingTables">是否创建数据表。</param>
        public RethinkDbPostRepository(IConnection conn, int shards, int replicas, bool createMissingTables)
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
                throw new InvalidOperationException(string.Format("One of the tables needed by {0} is missing. You can call {0} constructor with the parameter CreateMissingTables set to 'true'  to create the needed tables.", typeof(RethinkDbPostRepository).Name));
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
            if (tables.Contains(s_PostTable))
            {
                R.TableDrop(s_PostTable).RunResult(_conn).AssertNoErrors().AssertTablesDropped(1);
            }
            CreateTables();
        }

        /// <summary>
        ///     重新创建数据表。
        /// </summary>
        public void CreateTables()
        {
            var tables = R.TableList().RunResult<List<string>>(_conn);
            if (!tables.Contains(s_PostTable))
            {
                R.TableCreate(s_PostTable).OptArg("primary_key", "Id").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
                R.Table(s_PostTable).IndexCreate("AuthorId").RunResult(_conn).AssertNoErrors();
                R.Table(s_PostTable).IndexCreate("GroupId").RunResult(_conn).AssertNoErrors();
                R.Table(s_PostTable).IndexCreate("Tags").OptArg("multi", true).RunResult(_conn).AssertNoErrors();
                //R.Table(s_PostTable).IndexWait().RunResult(_conn).AssertNoErrors();
            }
        }

        /// <summary>
        ///     检测指定的数据表是否存在。
        /// </summary>
        public bool TablesExists()
        {
            var tableNames = new List<string>
                             {
                                 s_PostTable
                             };
            var tables = R.TableList().RunResult<List<string>>(_conn);
            return tables.Any(table => tableNames.Contains(table));
        }

        #endregion

        #region 检测帖子是否存在

        #endregion

        #region IPostRepository 接口实现

        /// <inheritdoc />
        public Post GetPost(string postId)
        {
            return R.Table(s_PostTable).Get(postId).RunResult<Post>(_conn);
        }

        /// <inheritdoc />
        public Task<Post> GetPostAsync(string postId)
        {
            return R.Table(s_PostTable).Get(postId).RunResultAsync<Post>(_conn);
        }

        /// <inheritdoc />
        public List<Post> GetPosts(List<string> postIds)
        {
            return R.Table(s_PostTable).GetAll(R.Args(postIds.ToArray())).RunResult<List<Post>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Post>> GetPostsAsync(List<string> postIds)
        {
            return R.Table(s_PostTable).GetAll(R.Args(postIds.ToArray())).RunResultAsync<List<Post>>(_conn);
        }

        /// <inheritdoc />
        public List<Post> FindPosts(string titleFilter, string tag, string contentType, DateTime? createdSince, DateTime? modifiedSince, DateTime? publishedSince, bool? isPublished, bool? isFeatured, string status, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_PostTable).Filter(true);
            if (!tag.IsNullOrEmpty())
            {
                query = R.Table(s_PostTable).GetAll(tag).OptArg("index", "Tags").Filter(true);
            }
            if (!titleFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Title").Match(titleFilter).Or(row.G("Summary").Match(titleFilter)));
            }
            if (!contentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ContentType").Eq(contentType));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Ge(modifiedSince.Value));
            }
            if (publishedSince.HasValue)
            {
                query = query.Filter(row => row.G("PublishedDate").Ge(publishedSince.Value));
            }
            if (isPublished.HasValue)
            {
                query = query.Filter(row => row.G("IsPublished").Eq(isPublished.Value));
            }
            if (isFeatured.HasValue)
            {
                query = query.Filter(row => row.G("IsFeatured").Eq(isFeatured.Value));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
            }
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("CreatedDate")) : query.OrderBy("CreatedDate");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 10000).RunResult<List<Post>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Post>> FindPostsAsync(string titleFilter, string tag, string contentType, DateTime? createdSince, DateTime? modifiedSince, DateTime? publishedSince, bool? isPublished, bool? isFeatured, string status, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_PostTable).Filter(true);
            if (!tag.IsNullOrEmpty())
            {
                query = R.Table(s_PostTable).GetAll(tag).OptArg("index", "Tags").Filter(true);
            }
            if (!titleFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Title").Match(titleFilter).Or(row.G("Summary").Match(titleFilter)));
            }
            if (!contentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ContentType").Eq(contentType));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Ge(modifiedSince.Value));
            }
            if (publishedSince.HasValue)
            {
                query = query.Filter(row => row.G("PublishedDate").Ge(publishedSince.Value));
            }
            if (isPublished.HasValue)
            {
                query = query.Filter(row => row.G("IsPublished").Eq(isPublished.Value));
            }
            if (isFeatured.HasValue)
            {
                query = query.Filter(row => row.G("IsFeatured").Eq(isFeatured.Value));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
            }
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("CreatedDate")) : query.OrderBy("CreatedDate");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 10000).RunResultAsync<List<Post>>(_conn);
        }

        /// <inheritdoc />
        public List<Post> FindPostsByAuthor(int authorId, string tag, string contentType, DateTime? createdSince, DateTime? modifiedSince, DateTime? publishedSince, bool? isPublished, bool? isFeatured, string status, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_PostTable).GetAll(authorId).OptArg("index", "AuthorId").Filter(true);
            if (!tag.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Tags").Contains(tag));
            }
            if (!contentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ContentType").Eq(contentType));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Ge(modifiedSince.Value));
            }
            if (publishedSince.HasValue)
            {
                query = query.Filter(row => row.G("PublishedDate").Ge(publishedSince.Value));
            }
            if (isPublished.HasValue)
            {
                query = query.Filter(row => row.G("IsPublished").Eq(isPublished.Value));
            }
            if (isFeatured.HasValue)
            {
                query = query.Filter(row => row.G("IsFeatured").Eq(isFeatured.Value));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
            }
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("CreatedDate")) : query.OrderBy("CreatedDate");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 10000).RunResult<List<Post>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Post>> FindPostsByAuthorAsync(int authorId, string tag, string contentType, DateTime? createdSince, DateTime? modifiedSince, DateTime? publishedSince, bool? isPublished, bool? isFeatured, string status, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_PostTable).GetAll(authorId).OptArg("index", "AuthorId").Filter(true);
            if (!contentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ContentType").Eq(contentType));
            }
            if (!tag.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Tags").Contains(tag));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Ge(modifiedSince.Value));
            }
            if (publishedSince.HasValue)
            {
                query = query.Filter(row => row.G("PublishedDate").Ge(publishedSince.Value));
            }
            if (isPublished.HasValue)
            {
                query = query.Filter(row => row.G("IsPublished").Eq(isPublished.Value));
            }
            if (isFeatured.HasValue)
            {
                query = query.Filter(row => row.G("IsFeatured").Eq(isFeatured.Value));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
            }
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("CreatedDate")) : query.OrderBy("CreatedDate");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 10000).RunResultAsync<List<Post>>(_conn);
        }

        /// <inheritdoc />
        public List<Post> FindPostsByGroup(string groupId, string tag, string contentType, DateTime? createdSince, DateTime? modifiedSince, DateTime? publishedSince, bool? isPublished, bool? isFeatured, string status, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_PostTable).GetAll(groupId).OptArg("index", "GroupId").Filter(true);
            if (!contentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ContentType").Eq(contentType));
            }
            if (!tag.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Tags").Contains(tag));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Ge(modifiedSince.Value));
            }
            if (publishedSince.HasValue)
            {
                query = query.Filter(row => row.G("PublishedDate").Ge(publishedSince.Value));
            }
            if (isPublished.HasValue)
            {
                query = query.Filter(row => row.G("IsPublished").Eq(isPublished.Value));
            }
            if (isFeatured.HasValue)
            {
                query = query.Filter(row => row.G("IsFeatured").Eq(isFeatured.Value));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
            }
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("CreatedDate")) : query.OrderBy("CreatedDate");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 10000).RunResult<List<Post>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Post>> FindPostsByGroupAsync(string groupId, string tag, string contentType, DateTime? createdSince, DateTime? modifiedSince, DateTime? publishedSince, bool? isPublished, bool? isFeatured, string status, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_PostTable).GetAll(groupId).OptArg("index", "GroupId").Filter(true);
            if (!contentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ContentType").Eq(contentType));
            }
            if (!tag.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Tags").Contains(tag));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Ge(modifiedSince.Value));
            }
            if (publishedSince.HasValue)
            {
                query = query.Filter(row => row.G("PublishedDate").Ge(publishedSince.Value));
            }
            if (isPublished.HasValue)
            {
                query = query.Filter(row => row.G("IsPublished").Eq(isPublished.Value));
            }
            if (isFeatured.HasValue)
            {
                query = query.Filter(row => row.G("IsFeatured").Eq(isFeatured.Value));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
            }
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("CreatedDate")) : query.OrderBy("CreatedDate");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 10000).RunResultAsync<List<Post>>(_conn);
        }

        /// <inheritdoc />
        public int GetPostsCount(string titleFilter, string tag, string contentType, DateTime? createdSince, DateTime? modifiedSince, DateTime? publishedSince, bool? isPublished, bool? isFeatured, string status)
        {
            var query = R.Table(s_PostTable).Filter(true);
            if (!tag.IsNullOrEmpty())
            {
                query = R.Table(s_PostTable).GetAll(tag).OptArg("index", "Tags").Filter(true);
            }
            if (!titleFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Title").Match(titleFilter).Or(row.G("Summary").Match(titleFilter)));
            }
            if (!contentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ContentType").Eq(contentType));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Ge(modifiedSince.Value));
            }
            if (publishedSince.HasValue)
            {
                query = query.Filter(row => row.G("PublishedDate").Ge(publishedSince.Value));
            }
            if (isPublished.HasValue)
            {
                query = query.Filter(row => row.G("IsPublished").Eq(isPublished.Value));
            }
            if (isFeatured.HasValue)
            {
                query = query.Filter(row => row.G("IsFeatured").Eq(isFeatured.Value));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
            }
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetPostsCountAsync(string titleFilter, string tag, string contentType, DateTime? createdSince, DateTime? modifiedSince, DateTime? publishedSince, bool? isPublished, bool? isFeatured, string status)
        {
            var query = R.Table(s_PostTable).Filter(true);
            if (!tag.IsNullOrEmpty())
            {
                query = R.Table(s_PostTable).GetAll(tag).OptArg("index", "Tags").Filter(true);
            }
            if (!titleFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Title").Match(titleFilter).Or(row.G("Summary").Match(titleFilter)));
            }
            if (!contentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ContentType").Eq(contentType));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Ge(modifiedSince.Value));
            }
            if (publishedSince.HasValue)
            {
                query = query.Filter(row => row.G("PublishedDate").Ge(publishedSince.Value));
            }
            if (isPublished.HasValue)
            {
                query = query.Filter(row => row.G("IsPublished").Eq(isPublished.Value));
            }
            if (isFeatured.HasValue)
            {
                query = query.Filter(row => row.G("IsFeatured").Eq(isFeatured.Value));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
            }
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public int GetPostsCountByAuthor(int authorId, string tag, string contentType, DateTime? createdSince, DateTime? modifiedSince, DateTime? publishedSince, bool? isPublished, bool? isFeatured, string status)
        {
            var query = R.Table(s_PostTable).GetAll(authorId).OptArg("index", "AuthorId").Filter(true);
            if (!contentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ContentType").Eq(contentType));
            }
            if (!tag.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Tags").Contains(tag));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Ge(modifiedSince.Value));
            }
            if (publishedSince.HasValue)
            {
                query = query.Filter(row => row.G("PublishedDate").Ge(publishedSince.Value));
            }
            if (isPublished.HasValue)
            {
                query = query.Filter(row => row.G("IsPublished").Eq(isPublished.Value));
            }
            if (isFeatured.HasValue)
            {
                query = query.Filter(row => row.G("IsFeatured").Eq(isFeatured.Value));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
            }
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetPostsCountByAuthorAsync(int authorId, string tag, string contentType, DateTime? createdSince, DateTime? modifiedSince, DateTime? publishedSince, bool? isPublished, bool? isFeatured, string status)
        {
            var query = R.Table(s_PostTable).GetAll(authorId).OptArg("index", "AuthorId").Filter(true);
            if (!contentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ContentType").Eq(contentType));
            }
            if (!tag.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Tags").Contains(tag));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Ge(modifiedSince.Value));
            }
            if (publishedSince.HasValue)
            {
                query = query.Filter(row => row.G("PublishedDate").Ge(publishedSince.Value));
            }
            if (isPublished.HasValue)
            {
                query = query.Filter(row => row.G("IsPublished").Eq(isPublished.Value));
            }
            if (isFeatured.HasValue)
            {
                query = query.Filter(row => row.G("IsFeatured").Eq(isFeatured.Value));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
            }
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public int GetPostsCountByGroup(string groupId, string tag, string contentType, DateTime? createdSince, DateTime? modifiedSince, DateTime? publishedSince, bool? isPublished, bool? isFeatured, string status)
        {
            var query = R.Table(s_PostTable).GetAll(groupId).OptArg("index", "GroupId").Filter(true);
            if (!contentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ContentType").Eq(contentType));
            }
            if (!tag.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Tags").Contains(tag));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Ge(modifiedSince.Value));
            }
            if (publishedSince.HasValue)
            {
                query = query.Filter(row => row.G("PublishedDate").Ge(publishedSince.Value));
            }
            if (isPublished.HasValue)
            {
                query = query.Filter(row => row.G("IsPublished").Eq(isPublished.Value));
            }
            if (isFeatured.HasValue)
            {
                query = query.Filter(row => row.G("IsFeatured").Eq(isFeatured.Value));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
            }
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetPostsCountByGroupAsync(string groupId, string tag, string contentType, DateTime? createdSince, DateTime? modifiedSince, DateTime? publishedSince, bool? isPublished, bool? isFeatured, string status)
        {
            var query = R.Table(s_PostTable).GetAll(groupId).OptArg("index", "GroupId").Filter(true);
            if (!contentType.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("ContentType").Eq(contentType));
            }
            if (!tag.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Tags").Contains(tag));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Ge(createdSince.Value));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Ge(modifiedSince.Value));
            }
            if (publishedSince.HasValue)
            {
                query = query.Filter(row => row.G("PublishedDate").Ge(publishedSince.Value));
            }
            if (isPublished.HasValue)
            {
                query = query.Filter(row => row.G("IsPublished").Eq(isPublished.Value));
            }
            if (isFeatured.HasValue)
            {
                query = query.Filter(row => row.G("IsFeatured").Eq(isFeatured.Value));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Status").Eq(status));
            }
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public Post CreatePost(Post newPost)
        {
            newPost.ThrowIfNull(nameof(newPost));
            newPost.Id = newPost.Id.IsNullOrEmpty() ? new Base36IdGenerator(11).NewId().ToLower() : newPost.Id;
            newPost.Status = "审核通过";
            newPost.CreatedDate = DateTime.UtcNow;
            newPost.ModifiedDate = newPost.CreatedDate;
            newPost.PublishedDate = newPost.IsPublished ? newPost.CreatedDate : (DateTime?) null;
            newPost.IsFeatured = false;
            newPost.ViewsCount = 0;
            newPost.BookmarksCount = 0;
            newPost.CommentsCount = 0;
            newPost.LikesCount = 0;
            newPost.RatingsCount = 0;
            newPost.RatingsAverageValue = 0;
            newPost.SharesCount = 0;
            newPost.AbuseReportsCount = 0;
            newPost.ContentQuality = 0;
            var result = R.Table(s_PostTable).Get(newPost.Id).Replace(newPost).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Post>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Post> CreatePostAsync(Post newPost)
        {
            newPost.ThrowIfNull(nameof(newPost));
            newPost.Id = newPost.Id.IsNullOrEmpty() ? new Base36IdGenerator(11).NewId().ToLower() : newPost.Id;
            newPost.Status = "审核通过";
            newPost.CreatedDate = DateTime.UtcNow;
            newPost.ModifiedDate = newPost.CreatedDate;
            newPost.PublishedDate = newPost.IsPublished ? newPost.CreatedDate : (DateTime?) null;
            newPost.IsFeatured = false;
            newPost.ViewsCount = 0;
            newPost.BookmarksCount = 0;
            newPost.CommentsCount = 0;
            newPost.LikesCount = 0;
            newPost.RatingsCount = 0;
            newPost.RatingsAverageValue = 0;
            newPost.SharesCount = 0;
            newPost.AbuseReportsCount = 0;
            newPost.ContentQuality = 0;
            var result = (await R.Table(s_PostTable).Get(newPost.Id).Replace(newPost).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Post>()[0].NewValue;
        }

        /// <inheritdoc />
        public Post UpdatePost(Post existingPost, Post newPost)
        {
            existingPost.ThrowIfNull(nameof(existingPost));
            newPost.Id = existingPost.Id;
            newPost.CreatedDate = existingPost.CreatedDate;
            newPost.ModifiedDate = DateTime.UtcNow;
            var result = R.Table(s_PostTable).Get(newPost.Id).Replace(newPost).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Post>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Post> UpdatePostAsync(Post existingPost, Post newPost)
        {
            existingPost.ThrowIfNull(nameof(existingPost));
            newPost.Id = existingPost.Id;
            newPost.CreatedDate = existingPost.CreatedDate;
            newPost.ModifiedDate = DateTime.UtcNow;
            var result = (await R.Table(s_PostTable).Get(newPost.Id).Replace(newPost).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Post>()[0].NewValue;
        }

        /// <inheritdoc />
        public void DeletePost(string postId)
        {
            R.Table(s_PostTable).Get(postId).Delete().RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task DeletePostAsync(string postId)
        {
            (await R.Table(s_PostTable).Get(postId).Delete().RunResultAsync(_conn)).AssertNoErrors();
        }

        /// <inheritdoc />
        public Post IncrementPostViewsCount(string postId, int count)
        {
            var result = R.Table(s_PostTable).Get(postId).Update(row => R.HashMap("ViewsCount", row.G("ViewsCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Post>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Post> IncrementPostViewsCountAsync(string postId, int count)
        {
            var result = (await R.Table(s_PostTable).Get(postId).Update(row => R.HashMap("ViewsCount", row.G("ViewsCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Post>()[0].NewValue;
        }

        /// <inheritdoc />
        public Post IncrementPostBookmarksCount(string postId, int count)
        {
            var result = R.Table(s_PostTable).Get(postId).Update(row => R.HashMap("BookmarksCount", row.G("BookmarksCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Post>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Post> IncrementPostBookmarksCountAsync(string postId, int count)
        {
            var result = (await R.Table(s_PostTable).Get(postId).Update(row => R.HashMap("BookmarksCount", row.G("BookmarksCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Post>()[0].NewValue;
        }

        /// <inheritdoc />
        public Post IncrementPostCommentsCount(string postId, int count)
        {
            var result = R.Table(s_PostTable).Get(postId).Update(row => R.HashMap("CommentsCount", row.G("CommentsCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Post>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Post> IncrementPostCommentsCountAsync(string postId, int count)
        {
            var result = (await R.Table(s_PostTable).Get(postId).Update(row => R.HashMap("CommentsCount", row.G("CommentsCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Post>()[0].NewValue;
        }

        /// <inheritdoc />
        public Post IncrementPostLikesCount(string postId, int count)
        {
            var result = R.Table(s_PostTable).Get(postId).Update(row => R.HashMap("LikesCount", row.G("LikesCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Post>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Post> IncrementPostLikesCountAsync(string postId, int count)
        {
            var result = (await R.Table(s_PostTable).Get(postId).Update(row => R.HashMap("LikesCount", row.G("LikesCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Post>()[0].NewValue;
        }

        /// <inheritdoc />
        public Post IncrementPostRatingsCount(string postId, int count)
        {
            var result = R.Table(s_PostTable).Get(postId).Update(row => R.HashMap("RatingsCount", row.G("RatingsCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Post>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Post> IncrementPostRatingsCountAsync(string postId, int count)
        {
            var result = (await R.Table(s_PostTable).Get(postId).Update(row => R.HashMap("RatingsCount", row.G("RatingsCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Post>()[0].NewValue;
        }

        /// <inheritdoc />
        public Post IncrementPostSharesCount(string postId, int count)
        {
            var result = R.Table(s_PostTable).Get(postId).Update(row => R.HashMap("SharesCount", row.G("SharesCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Post>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Post> IncrementPostSharesCountAsync(string postId, int count)
        {
            var result = (await R.Table(s_PostTable).Get(postId).Update(row => R.HashMap("SharesCount", row.G("SharesCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Post>()[0].NewValue;
        }

        /// <inheritdoc />
        public Post IncrementPostAbuseReportsCount(string postId, int count)
        {
            var result = R.Table(s_PostTable).Get(postId).Update(row => R.HashMap("AbuseReportsCount", row.G("AbuseReportsCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Post>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Post> IncrementPostAbuseReportsCountAsync(string postId, int count)
        {
            var result = (await R.Table(s_PostTable).Get(postId).Update(row => R.HashMap("AbuseReportsCount", row.G("AbuseReportsCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Post>()[0].NewValue;
        }

        /// <inheritdoc />
        public Post UpdatePostRatingsAverageValue(string postId, float value)
        {
            var result = R.Table(s_PostTable).Get(postId).Update(row => R.HashMap("RatingsAverageValue", value)).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Post>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Post> UpdatePostRatingsAverageValueAsync(string postId, float value)
        {
            var result = (await R.Table(s_PostTable).Get(postId).Update(row => R.HashMap("RatingsAverageValue", value)).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Post>()[0].NewValue;
        }

        /// <inheritdoc />
        public Post UpdatePostContentQuality(string postId, float value)
        {
            var result = R.Table(s_PostTable).Get(postId).Update(row => R.HashMap("ContentQuality", value)).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Post>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Post> UpdatePostContentQualityAsync(string postId, float value)
        {
            var result = (await R.Table(s_PostTable).Get(postId).Update(row => R.HashMap("ContentQuality", value)).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Post>()[0].NewValue;
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