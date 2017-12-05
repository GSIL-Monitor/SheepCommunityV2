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
    ///     基于RethinkDb的章的存储库。
    /// </summary>
    public class RethinkDbChapterRepository : IChapterRepository, IClearable
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RethinkDbChapterRepository));

        /// <summary>
        ///     RethinkDB 查询器.
        /// </summary>
        public static readonly RethinkDB R = RethinkDB.R;

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

        #endregion

        #region 属性

        private readonly IConnection _conn;
        private readonly int _shards;
        private readonly int _replicas;

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="RethinkDbChapterRepository" />对象。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="shards">数据库分片数。</param>
        /// <param name="replicas">复制份数。</param>
        /// <param name="createMissingTables">是否创建数据表。</param>
        public RethinkDbChapterRepository(IConnection conn, int shards, int replicas, bool createMissingTables)
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
                throw new InvalidOperationException(string.Format("One of the tables needed by {0} is missing. You can call {0} constructor with the parameter CreateMissingTables set to 'true'  to create the needed tables.", typeof(RethinkDbChapterRepository).Name));
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
            if (tables.Contains(s_ChapterTable))
            {
                R.TableDrop(s_ChapterTable).RunResult(_conn).AssertNoErrors().AssertTablesDropped(1);
            }
            CreateTables();
        }

        /// <summary>
        ///     重新创建数据表。
        /// </summary>
        public void CreateTables()
        {
            var tables = R.TableList().RunResult<List<string>>(_conn);
            if (!tables.Contains(s_ChapterTable))
            {
                R.TableCreate(s_ChapterTable).OptArg("primary_key", "Id").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
                R.Table(s_ChapterTable).IndexCreate("BookId").RunResult(_conn).AssertNoErrors();
                R.Table(s_ChapterTable).IndexCreate("VolumeId").RunResult(_conn).AssertNoErrors();
                //R.Table(s_ChapterTable).IndexWait().RunResult(_conn).AssertNoErrors();
            }
        }

        /// <summary>
        ///     检测指定的数据表是否存在。
        /// </summary>
        public bool TablesExists()
        {
            var tableNames = new List<string>
                             {
                                 s_ChapterTable
                             };
            var tables = R.TableList().RunResult<List<string>>(_conn);
            return tables.Any(table => tableNames.Contains(table));
        }

        #endregion

        #region 检测章是否存在

        #endregion

        #region IChapterRepository 接口实现

        /// <inheritdoc />
        public Chapter GetChapter(string chapterId)
        {
            return R.Table(s_ChapterTable).Get(chapterId).RunResult<Chapter>(_conn);
        }

        /// <inheritdoc />
        public Task<Chapter> GetChapterAsync(string chapterId)
        {
            return R.Table(s_ChapterTable).Get(chapterId).RunResultAsync<Chapter>(_conn);
        }

        /// <inheritdoc />
        public List<Chapter> FindChapters(string bookId, string contentFilter, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ChapterTable).GetAll(bookId).OptArg("index", "BookId").Filter(true);
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 500).RunResult<List<Chapter>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Chapter>> FindChaptersAsync(string bookId, string contentFilter, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ChapterTable).GetAll(bookId).OptArg("index", "BookId").Filter(true);
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
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 500).RunResultAsync<List<Chapter>>(_conn);
        }

        /// <inheritdoc />
        public List<Chapter> FindChaptersByVolume(string volumeId, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ChapterTable).GetAll(volumeId).OptArg("index", "VolumeId").Filter(true);
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("Number")) : query.OrderBy("Number");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 500).RunResult<List<Chapter>>(_conn);
        }

        /// <inheritdoc />
        public Task<List<Chapter>> FindChaptersByVolumeAsync(string volumeId, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_ChapterTable).GetAll(volumeId).OptArg("index", "VolumeId").Filter(true);
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("Number")) : query.OrderBy("Number");
            }
            return queryOrder.Skip(skip ?? 0).Limit(limit ?? 500).RunResultAsync<List<Chapter>>(_conn);
        }

        /// <inheritdoc />
        public int GetChaptersCount(string bookId, string contentFilter)
        {
            var query = R.Table(s_ChapterTable).GetAll(bookId).OptArg("index", "BookId").Filter(true);
            if (!contentFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Content").Match(contentFilter));
            }
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetChaptersCountAsync(string bookId, string contentFilter)
        {
            var query = R.Table(s_ChapterTable).GetAll(bookId).OptArg("index", "BookId").Filter(true);
            if (!contentFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Content").Match(contentFilter));
            }
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public int GetChaptersCountByVolume(string volumeId)
        {
            var query = R.Table(s_ChapterTable).GetAll(volumeId).OptArg("index", "VolumeId").Filter(true);
            return query.Count().RunResult<int>(_conn);
        }

        /// <inheritdoc />
        public Task<int> GetChaptersCountByVolumeAsync(string volumeId)
        {
            var query = R.Table(s_ChapterTable).GetAll(volumeId).OptArg("index", "VolumeId").Filter(true);
            return query.Count().RunResultAsync<int>(_conn);
        }

        /// <inheritdoc />
        public Chapter CreateChapter(Chapter newChapter)
        {
            newChapter.ThrowIfNull(nameof(newChapter));
            newChapter.Id = newChapter.Id.IsNullOrEmpty() ? new Base36IdGenerator(10, 4, 4).NewId().ToLower() : newChapter.Id;
            newChapter.ViewsCount = 0;
            newChapter.BookmarksCount = 0;
            newChapter.CommentsCount = 0;
            newChapter.LikesCount = 0;
            newChapter.RatingsCount = 0;
            newChapter.RatingsAverageValue = 0;
            newChapter.SharesCount = 0;
            var result = R.Table(s_ChapterTable).Get(newChapter.Id).Replace(newChapter).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Chapter>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Chapter> CreateChapterAsync(Chapter newChapter)
        {
            newChapter.ThrowIfNull(nameof(newChapter));
            newChapter.Id = newChapter.Id.IsNullOrEmpty() ? new Base36IdGenerator(10, 4, 4).NewId().ToLower() : newChapter.Id;
            newChapter.ViewsCount = 0;
            newChapter.BookmarksCount = 0;
            newChapter.CommentsCount = 0;
            newChapter.LikesCount = 0;
            newChapter.RatingsCount = 0;
            newChapter.RatingsAverageValue = 0;
            newChapter.SharesCount = 0;
            var result = (await R.Table(s_ChapterTable).Get(newChapter.Id).Replace(newChapter).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Chapter>()[0].NewValue;
        }

        /// <inheritdoc />
        public Chapter UpdateChapter(Chapter existingChapter, Chapter newChapter)
        {
            existingChapter.ThrowIfNull(nameof(existingChapter));
            newChapter.Id = existingChapter.Id;
            newChapter.VolumeId = existingChapter.VolumeId;
            newChapter.Number = existingChapter.Number;
            var result = R.Table(s_ChapterTable).Get(newChapter.Id).Replace(newChapter).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Chapter>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Chapter> UpdateChapterAsync(Chapter existingChapter, Chapter newChapter)
        {
            existingChapter.ThrowIfNull(nameof(existingChapter));
            newChapter.Id = existingChapter.Id;
            newChapter.VolumeId = existingChapter.VolumeId;
            newChapter.Number = existingChapter.Number;
            var result = (await R.Table(s_ChapterTable).Get(newChapter.Id).Replace(newChapter).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Chapter>()[0].NewValue;
        }

        /// <inheritdoc />
        public void DeleteChapter(string chapterId)
        {
            R.Table(s_ChapterTable).Get(chapterId).Delete().RunResult(_conn).AssertNoErrors();
            R.Table(s_ChapterAnnotationTable).GetAll(chapterId).OptArg("index", "ChapterId").Delete().RunResult(_conn).AssertNoErrors();
            R.Table(s_ParagraphTable).GetAll(chapterId).OptArg("index", "ChapterId").Delete().RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task DeleteChapterAsync(string chapterId)
        {
            (await R.Table(s_ChapterTable).Get(chapterId).Delete().RunResultAsync(_conn)).AssertNoErrors();
            (await R.Table(s_ChapterAnnotationTable).GetAll(chapterId).OptArg("index", "ChapterId").Delete().RunResultAsync(_conn)).AssertNoErrors();
            (await R.Table(s_ParagraphTable).GetAll(chapterId).OptArg("index", "ChapterId").Delete().RunResultAsync(_conn)).AssertNoErrors();
        }

        /// <inheritdoc />
        public Chapter IncrementChapterViewsCount(string chapterId, int count)
        {
            var result = R.Table(s_ChapterTable).Get(chapterId).Update(row => R.HashMap("ViewsCount", row.G("ViewsCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Chapter>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Chapter> IncrementChapterViewsCountAsync(string chapterId, int count)
        {
            var result = (await R.Table(s_ChapterTable).Get(chapterId).Update(row => R.HashMap("ViewsCount", row.G("ViewsCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Chapter>()[0].NewValue;
        }

        /// <inheritdoc />
        public Chapter IncrementChapterBookmarksCount(string chapterId, int count)
        {
            var result = R.Table(s_ChapterTable).Get(chapterId).Update(row => R.HashMap("BookmarksCount", row.G("BookmarksCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Chapter>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Chapter> IncrementChapterBookmarksCountAsync(string chapterId, int count)
        {
            var result = (await R.Table(s_ChapterTable).Get(chapterId).Update(row => R.HashMap("BookmarksCount", row.G("BookmarksCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Chapter>()[0].NewValue;
        }

        /// <inheritdoc />
        public Chapter IncrementChapterCommentsCount(string chapterId, int count)
        {
            var result = R.Table(s_ChapterTable).Get(chapterId).Update(row => R.HashMap("CommentsCount", row.G("CommentsCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Chapter>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Chapter> IncrementChapterCommentsCountAsync(string chapterId, int count)
        {
            var result = (await R.Table(s_ChapterTable).Get(chapterId).Update(row => R.HashMap("CommentsCount", row.G("CommentsCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Chapter>()[0].NewValue;
        }

        /// <inheritdoc />
        public Chapter IncrementChapterLikesCount(string chapterId, int count)
        {
            var result = R.Table(s_ChapterTable).Get(chapterId).Update(row => R.HashMap("LikesCount", row.G("LikesCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Chapter>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Chapter> IncrementChapterLikesCountAsync(string chapterId, int count)
        {
            var result = (await R.Table(s_ChapterTable).Get(chapterId).Update(row => R.HashMap("LikesCount", row.G("LikesCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Chapter>()[0].NewValue;
        }

        /// <inheritdoc />
        public Chapter IncrementChapterRatingsCount(string chapterId, int count)
        {
            var result = R.Table(s_ChapterTable).Get(chapterId).Update(row => R.HashMap("RatingsCount", row.G("RatingsCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Chapter>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Chapter> IncrementChapterRatingsCountAsync(string chapterId, int count)
        {
            var result = (await R.Table(s_ChapterTable).Get(chapterId).Update(row => R.HashMap("RatingsCount", row.G("RatingsCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Chapter>()[0].NewValue;
        }

        /// <inheritdoc />
        public Chapter IncrementChapterSharesCount(string chapterId, int count)
        {
            var result = R.Table(s_ChapterTable).Get(chapterId).Update(row => R.HashMap("SharesCount", row.G("SharesCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Chapter>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Chapter> IncrementChapterSharesCountAsync(string chapterId, int count)
        {
            var result = (await R.Table(s_ChapterTable).Get(chapterId).Update(row => R.HashMap("SharesCount", row.G("SharesCount").Default_(0).Add(count))).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Chapter>()[0].NewValue;
        }

        /// <inheritdoc />
        public Chapter UpdateChapterRatingsAverageValue(string chapterId, float value)
        {
            var result = R.Table(s_ChapterTable).Get(chapterId).Update(row => R.HashMap("RatingsAverageValue", value)).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<Chapter>()[0].NewValue;
        }

        /// <inheritdoc />
        public async Task<Chapter> UpdateChapterRatingsAverageValueAsync(string chapterId, float value)
        {
            var result = (await R.Table(s_ChapterTable).Get(chapterId).Update(row => R.HashMap("RatingsAverageValue", value)).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
            return result.ChangesAs<Chapter>()[0].NewValue;
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