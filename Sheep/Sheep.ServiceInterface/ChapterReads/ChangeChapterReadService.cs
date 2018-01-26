using System.Linq;
using ServiceStack;
using Sheep.Model.Bookstore.Entities;

namespace Sheep.ServiceInterface.ChapterReads
{
    /// <summary>
    ///     更改阅读的抽象服务。
    /// </summary>
    public abstract class ChangeChapterReadService : Service
    {
        /// <summary>
        ///     重置缓存。
        /// </summary>
        /// <param name="chapterRead">阅读。</param>
        protected void ResetCache(ChapterRead chapterRead)
        {
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/chapterreads/query/bychapter?chapterid={0}", chapterRead.ChapterId)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/chapterreads/query/bychapter?chapterid={0}", chapterRead.ChapterId)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/chapterreads/query/byuser?userid={0}", chapterRead.UserId)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/chapterreads/query/byuser?userid={0}", chapterRead.UserId)).ToArray());
        }
    }
}