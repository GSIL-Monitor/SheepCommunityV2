using System.Linq;
using ServiceStack;
using Sheep.Model.Content.Entities;

namespace Sheep.ServiceInterface.Views
{
    /// <summary>
    ///     更改查看的抽象服务。
    /// </summary>
    public abstract class ChangeViewService : Service
    {
        /// <summary>
        ///     重置缓存。
        /// </summary>
        /// <param name="view">查看。</param>
        protected void ResetCache(View view)
        {
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/views/query/byparent?parentid={0}", view.ParentId)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/views/query/byparent?parentid={0}", view.ParentId)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/views/query/byuser?userid={0}", view.UserId)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/views/query/byuser?userid={0}", view.UserId)).ToArray());
        }
    }
}