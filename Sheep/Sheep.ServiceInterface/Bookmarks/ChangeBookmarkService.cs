using System.Linq;
using ServiceStack;
using Sheep.Model.Content.Entities;

namespace Sheep.ServiceInterface.Bookmarks
{
    /// <summary>
    ///     更改收藏的抽象服务。
    /// </summary>
    public abstract class ChangeBookmarkService : Service
    {
        /// <summary>
        ///     重置缓存。
        /// </summary>
        /// <param name="bookmark">收藏。</param>
        protected void ResetCache(Bookmark bookmark)
        {
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/bookmarks/query/byparent?parentid={0}", bookmark.ParentId)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/bookmarks/query/byparent?parentid={0}", bookmark.ParentId)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/bookmarks/query/byuser?userid={0}", bookmark.UserId)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/bookmarks/query/byuser?userid={0}", bookmark.UserId)).ToArray());
        }
    }
}