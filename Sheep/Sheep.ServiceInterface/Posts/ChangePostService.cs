using System.Linq;
using ServiceStack;
using Sheep.Model.Content.Entities;

namespace Sheep.ServiceInterface.Posts
{
    /// <summary>
    ///     更改帖子的抽象服务。
    /// </summary>
    public abstract class ChangePostService : Service
    {
        /// <summary>
        ///     重置缓存。
        /// </summary>
        /// <param name="post">帖子。</param>
        protected void ResetCache(Post post)
        {
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/posts/{0}", post.Id)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/posts/{0}", post.Id)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/posts/basic/{0}", post.Id)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/posts/basic/{0}", post.Id)).ToArray());
        }
    }
}