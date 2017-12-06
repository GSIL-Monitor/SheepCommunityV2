﻿using System;
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
        public List<Paragraph> FindParagraphs(string bookId, string contentFilter, string annotationFilter, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ParagraphTable).GetAll(bookId).OptArg("index", "BookId").Filter(true);
            if (!contentFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Content").Match(contentFilter));
            }
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 10000).RunResult<List<Paragraph>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Paragraph>> FindParagraphsAsync(string bookId, string contentFilter, string annotationFilter, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ParagraphTable).GetAll(bookId).OptArg("index", "BookId").Filter(true);
            if (!contentFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Content").Match(contentFilter));
            }
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 10000).RunResultAsync<List<Paragraph>>(_conn);
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 10000).RunResult<List<Paragraph>>(_conn);
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 10000).RunResultAsync<List<Paragraph>>(_conn);
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 10000).RunResult<List<Paragraph>>(_conn);
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 10000).RunResultAsync<List<Paragraph>>(_conn);
        }

        /// <inheritdoc />
        public int GetParagraphsCount(string bookId, string contentFilter, string annotationFilter)
        {
            var query = R.Table(s_ParagraphTable).GetAll(bookId).OptArg("index", "BookId").Filter(true);
            if (!contentFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Content").Match(contentFilter));
            }
            if (!annotationFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Annotation").Match(annotationFilter));
            }
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetParagraphsCountAsync(string bookId, string contentFilter, string annotationFilter)
        {
            var query = R.Table(s_ParagraphTable).GetAll(bookId).OptArg("index", "BookId").Filter(true);
            if (!contentFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Content").Match(contentFilter));
            }
            if (!annotationFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Annotation").Match(annotationFilter));
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
            newParagraph.Id = newParagraph.Id.IsNullOrEmpty() ? new Base36IdGenerator(11).NewId().ToLower() : newParagraph.Id;
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
            newParagraph.Id = newParagraph.Id.IsNullOrEmpty() ? new Base36IdGenerator(11).NewId().ToLower() : newParagraph.Id;
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
            newParagraph.ChapterId = existingParagraph.ChapterId;
            newParagraph.SubjectId = existingParagraph.SubjectId;
            newParagraph.Number = existingParagraph.Number;
            var result = R.Table(s_ParagraphTable).Get(newParagraph.Id).Replace(newParagraph).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Paragraph>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Paragraph> UpdateParagraphAsync(Paragraph existingParagraph, Paragraph newParagraph)
        {
            existingParagraph.ThrowIfNull(nameof(existingParagraph));
            newParagraph.Id = existingParagraph.Id;
            newParagraph.ChapterId = existingParagraph.ChapterId;
            newParagraph.SubjectId = existingParagraph.SubjectId;
            newParagraph.Number = existingParagraph.Number;
            var result = (await R.Table(s_ParagraphTable).Get(newParagraph.Id).Replace(newParagraph).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Paragraph>()[0].NewValue;
        }

        /// <inheritdoc />
        public void DeleteParagraph(string paragraphId)
        {
            R.Table(s_ParagraphTable).Get(paragraphId).Delete().RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task DeleteParagraphAsync(string paragraphId)
        {
            (await R.Table(s_ParagraphTable).Get(paragraphId).Delete().RunResultAsync(_conn)).AssertNoErrors();
        }

        /// <inheritdoc />
        public Paragraph IncrementParagraphViewsCount(string paragraphId, int count)
        {
            var result = R.Table(s_ParagraphTable).Get(paragraphId).Update(row => R.HashMap("ViewsCount", row.G("ViewsCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Paragraph>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Paragraph> IncrementParagraphViewsCountAsync(string paragraphId, int count)
        {
            var result = (await R.Table(s_ParagraphTable).Get(paragraphId).Update(row => R.HashMap("ViewsCount", row.G("ViewsCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Paragraph>()[0].NewValue;
        }

        /// <inheritdoc />
        public Paragraph IncrementParagraphBookmarksCount(string paragraphId, int count)
        {
            var result = R.Table(s_ParagraphTable).Get(paragraphId).Update(row => R.HashMap("BookmarksCount", row.G("BookmarksCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Paragraph>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Paragraph> IncrementParagraphBookmarksCountAsync(string paragraphId, int count)
        {
            var result = (await R.Table(s_ParagraphTable).Get(paragraphId).Update(row => R.HashMap("BookmarksCount", row.G("BookmarksCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Paragraph>()[0].NewValue;
        }

        /// <inheritdoc />
        public Paragraph IncrementParagraphCommentsCount(string paragraphId, int count)
        {
            var result = R.Table(s_ParagraphTable).Get(paragraphId).Update(row => R.HashMap("CommentsCount", row.G("CommentsCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Paragraph>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Paragraph> IncrementParagraphCommentsCountAsync(string paragraphId, int count)
        {
            var result = (await R.Table(s_ParagraphTable).Get(paragraphId).Update(row => R.HashMap("CommentsCount", row.G("CommentsCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Paragraph>()[0].NewValue;
        }

        /// <inheritdoc />
        public Paragraph IncrementParagraphLikesCount(string paragraphId, int count)
        {
            var result = R.Table(s_ParagraphTable).Get(paragraphId).Update(row => R.HashMap("LikesCount", row.G("LikesCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Paragraph>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Paragraph> IncrementParagraphLikesCountAsync(string paragraphId, int count)
        {
            var result = (await R.Table(s_ParagraphTable).Get(paragraphId).Update(row => R.HashMap("LikesCount", row.G("LikesCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Paragraph>()[0].NewValue;
        }

        /// <inheritdoc />
        public Paragraph IncrementParagraphRatingsCount(string paragraphId, int count)
        {
            var result = R.Table(s_ParagraphTable).Get(paragraphId).Update(row => R.HashMap("RatingsCount", row.G("RatingsCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Paragraph>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Paragraph> IncrementParagraphRatingsCountAsync(string paragraphId, int count)
        {
            var result = (await R.Table(s_ParagraphTable).Get(paragraphId).Update(row => R.HashMap("RatingsCount", row.G("RatingsCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Paragraph>()[0].NewValue;
        }

        /// <inheritdoc />
        public Paragraph IncrementParagraphSharesCount(string paragraphId, int count)
        {
            var result = R.Table(s_ParagraphTable).Get(paragraphId).Update(row => R.HashMap("SharesCount", row.G("SharesCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Paragraph>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Paragraph> IncrementParagraphSharesCountAsync(string paragraphId, int count)
        {
            var result = (await R.Table(s_ParagraphTable).Get(paragraphId).Update(row => R.HashMap("SharesCount", row.G("SharesCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Paragraph>()[0].NewValue;
        }

        /// <inheritdoc />
        public Paragraph UpdateParagraphRatingsAverageValue(string paragraphId, float value)
        {
            var result = R.Table(s_ParagraphTable).Get(paragraphId).Update(row => R.HashMap("RatingsAverageValue", value)).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Paragraph>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Paragraph> UpdateParagraphRatingsAverageValueAsync(string paragraphId, float value)
        {
            var result = (await R.Table(s_ParagraphTable).Get(paragraphId).Update(row => R.HashMap("RatingsAverageValue", value)).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Paragraph>()[0].NewValue;
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