using System.Linq;
using ServiceStack;
using Sheep.Model.Read.Entities;

namespace Sheep.ServiceInterface.Books
{
    /// <summary>
    ///     更改书籍的抽象服务。
    /// </summary>
    public abstract class ChangeBookService : Service
    {
        /// <summary>
        ///     重置缓存。
        /// </summary>
        /// <param name="book">书籍。</param>
        protected void ResetCache(Book book)
        {
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/books/{0}", book.Id)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/books/{0}", book.Id)).ToArray());
        }
    }
}