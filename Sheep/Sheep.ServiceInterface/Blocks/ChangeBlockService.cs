using System.Linq;
using ServiceStack;
using Sheep.Model.Friendship.Entities;

namespace Sheep.ServiceInterface.Blocks
{
    /// <summary>
    ///     更改屏蔽的抽象服务。
    /// </summary>
    public abstract class ChangeBlockService : Service
    {
        /// <summary>
        ///     重置缓存。
        /// </summary>
        /// <param name="block">屏蔽。</param>
        protected void ResetCache(Block block)
        {
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/blocks/blockees?blockerid={0}", block.BlockerId)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/blocks/blockees?blockerid={0}", block.BlockerId)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/blocks/blockers?blockeeid={0}", block.BlockeeId)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/blocks/blockers?blockeeid={0}", block.BlockeeId)).ToArray());
        }
    }
}