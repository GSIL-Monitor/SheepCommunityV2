using System.Linq;
using ServiceStack;
using Sheep.Model.Content.Entities;

namespace Sheep.ServiceInterface.Replies
{
    /// <summary>
    ///     更改回复的抽象服务。
    /// </summary>
    public abstract class ChangeReplyService : Service
    {
        /// <summary>
        ///     重置缓存。
        /// </summary>
        /// <param name="reply">回复。</param>
        protected void ResetCache(Reply reply)
        {
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/replies/query/byparent?parentid={0}", reply.ParentId)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/replies/query/byparent?parentid={0}", reply.ParentId)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/replies/query/byuser?userid={0}", reply.UserId)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/replies/query/byuser?userid={0}", reply.UserId)).ToArray());
        }
    }
}