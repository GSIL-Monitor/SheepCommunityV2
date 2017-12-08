using System.Linq;
using ServiceStack;
using Sheep.Model.Read.Entities;

namespace Sheep.ServiceInterface.Chapters
{
    /// <summary>
    ///     更改章的抽象服务。
    /// </summary>
    public abstract class ChangeChapterService : Service
    {
        /// <summary>
        ///     重置缓存。
        /// </summary>
        /// <param name="chapter">章。</param>
        protected void ResetCache(Chapter chapter)
        {
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/books/{0}/volumes/{1}/chapters/{2}", chapter.BookId, chapter.VolumeNumber, chapter.Number)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/books/{0}/volumes/{1}/chapters/{2}", chapter.BookId, chapter.VolumeNumber, chapter.Number)).ToArray());
        }
    }
}