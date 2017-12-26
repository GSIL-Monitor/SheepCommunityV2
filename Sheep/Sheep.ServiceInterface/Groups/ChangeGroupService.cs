using System.Linq;
using ServiceStack;
using Sheep.Model.Friendship.Entities;

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
        }
    }
}