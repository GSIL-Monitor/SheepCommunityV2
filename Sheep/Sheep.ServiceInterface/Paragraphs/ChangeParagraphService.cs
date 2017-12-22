using System.Linq;
using ServiceStack;
using Sheep.Model.Bookstore.Entities;

namespace Sheep.ServiceInterface.Paragraphs
{
    /// <summary>
    ///     更改节的抽象服务。
    /// </summary>
    public abstract class ChangeParagraphService : Service
    {
        /// <summary>
        ///     重置缓存。
        /// </summary>
        /// <param name="paragraph">节。</param>
        protected void ResetCache(Paragraph paragraph)
        {
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/books/{0}/volumes/{1}/chapters/{2}/paragraphs/{3}", paragraph.BookId, paragraph.VolumeNumber, paragraph.ChapterNumber, paragraph.Number)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/books/{0}/volumes/{1}/chapters/{2}/paragraphs/{3}", paragraph.BookId, paragraph.VolumeNumber, paragraph.ChapterNumber, paragraph.Number)).ToArray());
        }
    }
}