using System.Linq;
using ServiceStack;
using Sheep.Model.Read.Entities;

namespace Sheep.ServiceInterface.Volumes
{
    /// <summary>
    ///     更改卷注释的抽象服务。
    /// </summary>
    public abstract class ChangeVolumeAnnotationService : Service
    {
        /// <summary>
        ///     重置缓存。
        /// </summary>
        /// <param name="volumeAnnotation">卷注释。</param>
        protected void ResetCache(VolumeAnnotation volumeAnnotation)
        {
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/books/{0}/volumes/{1}/annotations/{2}", volumeAnnotation.BookId, volumeAnnotation.VolumeNumber, volumeAnnotation.Number)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/books/{0}/volumes/{1}/annotations/{2}", volumeAnnotation.BookId, volumeAnnotation.VolumeNumber, volumeAnnotation.Number)).ToArray());
        }
    }
}