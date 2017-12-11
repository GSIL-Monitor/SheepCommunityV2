using System.Linq;
using ServiceStack;
using Sheep.Model.Read.Entities;

namespace Sheep.ServiceInterface.Paragraphs
{
    /// <summary>
    ///     更改节注释的抽象服务。
    /// </summary>
    public abstract class ChangeParagraphAnnotationService : Service
    {
        /// <summary>
        ///     重置缓存。
        /// </summary>
        /// <param name="paragraphAnnotation">节注释。</param>
        protected void ResetCache(ParagraphAnnotation paragraphAnnotation)
        {
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/books/{0}/volumes/{1}/chapters/{2}/paragraphs/{3}/annotations/{4}", paragraphAnnotation.BookId, paragraphAnnotation.VolumeNumber, paragraphAnnotation.ChapterNumber, paragraphAnnotation.ParagraphNumber, paragraphAnnotation.Number)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/books/{0}/volumes/{1}/chapters/{2}/paragraphs/{3}/annotations/{4}", paragraphAnnotation.BookId, paragraphAnnotation.VolumeNumber, paragraphAnnotation.ChapterNumber, paragraphAnnotation.ParagraphNumber, paragraphAnnotation.Number)).ToArray());
        }
    }
}