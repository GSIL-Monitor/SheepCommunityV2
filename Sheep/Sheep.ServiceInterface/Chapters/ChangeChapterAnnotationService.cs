using System.Linq;
using ServiceStack;
using Sheep.Model.Read.Entities;

namespace Sheep.ServiceInterface.Chapters
{
    /// <summary>
    ///     更改章注释的抽象服务。
    /// </summary>
    public abstract class ChangeChapterAnnotationService : Service
    {
        /// <summary>
        ///     重置缓存。
        /// </summary>
        /// <param name="chapterAnnotation">章注释。</param>
        protected void ResetCache(ChapterAnnotation chapterAnnotation)
        {
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/books/{0}/volumes/{1}/chapters/{2}/annotations/{3}", chapterAnnotation.BookId, chapterAnnotation.VolumeNumber, chapterAnnotation.ChapterNumber, chapterAnnotation.Number)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/books/{0}/volumes/{1}/chapters/{2}/annotations/{3}", chapterAnnotation.BookId, chapterAnnotation.VolumeNumber, chapterAnnotation.ChapterNumber, chapterAnnotation.Number)).ToArray());
        }
    }
}