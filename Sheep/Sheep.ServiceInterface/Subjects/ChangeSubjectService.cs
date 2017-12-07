using System.Linq;
using ServiceStack;
using Sheep.Model.Read.Entities;

namespace Sheep.ServiceInterface.Subjects
{
    /// <summary>
    ///     更改主题的抽象服务。
    /// </summary>
    public abstract class ChangeSubjectService : Service
    {
        /// <summary>
        ///     重置缓存。
        /// </summary>
        /// <param name="subject">主题。</param>
        protected void ResetCache(Subject subject)
        {
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/books/{0}/volumes/{1}/subjects/{2}", subject.BookId, subject.VolumeNumber, subject.Number)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/books/{0}/volumes/{1}/subjects/{2}", subject.BookId, subject.VolumeNumber, subject.Number)).ToArray());
        }
    }
}