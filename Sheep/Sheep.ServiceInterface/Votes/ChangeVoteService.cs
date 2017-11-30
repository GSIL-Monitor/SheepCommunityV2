using System.Linq;
using ServiceStack;
using Sheep.Model.Content.Entities;

namespace Sheep.ServiceInterface.Votes
{
    /// <summary>
    ///     更改投票的抽象服务。
    /// </summary>
    public abstract class ChangeVoteService : Service
    {
        /// <summary>
        ///     重置缓存。
        /// </summary>
        /// <param name="vote">投票。</param>
        protected void ResetCache(Vote vote)
        {
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/votes/query/byparent?parentid={0}", vote.ParentId)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/votes/query/byparent?parentid={0}", vote.ParentId)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/votes/query/byuser?userid={0}", vote.UserId)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/votes/query/byuser?userid={0}", vote.UserId)).ToArray());
        }
    }
}