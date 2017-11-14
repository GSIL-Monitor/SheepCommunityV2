using System.Linq;
using ServiceStack;
using Sheep.Model.Corp.Entities;

namespace Sheep.ServiceInterface.Groups
{
    /// <summary>
    ///     更改群组属性的抽象服务。
    /// </summary>
    public abstract class ChangeGroupService : Service
    {
        /// <summary>
        ///     重置缓存。
        /// </summary>
        /// <param name="group">群组。</param>
        protected void ResetCache(Group group)
        {
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/groups/{0}", group.Id)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/groups/{0}", group.Id)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/groups/basic/{0}", group.Id)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/groups/basic/{0}", group.Id)).ToArray());
            if (!group.RefId.IsNullOrEmpty())
            {
                Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/groups/show/{0}", group.RefId)).ToArray());
                Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/groups/show/{0}", group.RefId)).ToArray());
                Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/groups/basic/show/{0}", group.RefId)).ToArray());
                Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/groups/basic/show/{0}", group.RefId)).ToArray());
            }
        }
    }
}