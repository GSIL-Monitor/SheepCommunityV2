using System.Linq;
using ServiceStack;
using Sheep.Model.Content.Entities;

namespace Sheep.ServiceInterface.Comments
{
    /// <summary>
    ///     更改评论的抽象服务。
    /// </summary>
    public abstract class ChangeCommentService : Service
    {
        /// <summary>
        ///     重置缓存。
        /// </summary>
        /// <param name="comment">评论。</param>
        protected void ResetCache(Comment comment)
        {
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/comments/query/byparent?parentid={0}", comment.ParentId)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/comments/query/byparent?parentid={0}", comment.ParentId)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/comments/query/byuser?userid={0}", comment.ParentId)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/comments/query/byuser?userid={0}", comment.ParentId)).ToArray());
        }
    }
}