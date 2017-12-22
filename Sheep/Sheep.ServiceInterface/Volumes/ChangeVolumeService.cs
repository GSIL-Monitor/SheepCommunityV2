using System.Linq;
using ServiceStack;
using Sheep.Model.Bookstore.Entities;

namespace Sheep.ServiceInterface.Volumes
{
    /// <summary>
    ///     更改卷的抽象服务。
    /// </summary>
    public abstract class ChangeVolumeService : Service
    {
        /// <summary>
        ///     重置缓存。
        /// </summary>
        /// <param name="volume">卷。</param>
        protected void ResetCache(Volume volume)
        {
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/books/{0}/volumes/{1}", volume.BookId, volume.Number)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/books/{0}/volumes/{1}", volume.BookId, volume.Number)).ToArray());
        }
    }
}