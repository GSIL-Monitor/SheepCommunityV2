using System.Linq;
using ServiceStack;
using Sheep.Model.Content.Entities;

namespace Sheep.ServiceInterface.PostBlocks
{
    /// <summary>
    ///     更改帖子屏蔽的抽象服务。
    /// </summary>
    public abstract class ChangePostBlockService : Service
    {
        /// <summary>
        ///     重置缓存。
        /// </summary>
        /// <param name="postBlock">帖子屏蔽。</param>
        protected void ResetCache(PostBlock postBlock)
        {
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/postblocks/query/bypost?postid={0}", postBlock.PostId)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/postblocks/query/bypost?postid={0}", postBlock.PostId)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/postblocks/query/byblocker?blockerid={0}", postBlock.BlockerId)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/postblocks/query/byblocker?blockerid={0}", postBlock.BlockerId)).ToArray());
        }
    }
}