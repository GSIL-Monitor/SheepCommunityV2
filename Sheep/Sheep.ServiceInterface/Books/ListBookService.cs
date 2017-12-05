using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Model.Read;
using Sheep.ServiceInterface.Books.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Books;

namespace Sheep.ServiceInterface.Books
{
    /// <summary>
    ///     列举一组书籍服务接口。
    /// </summary>
    public class ListBookService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListBookService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置列举一组书籍的校验器。
        /// </summary>
        public IValidator<BookList> BookListValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置书籍的存储库。
        /// </summary>
        public IBookRepository BookRepo { get; set; }

        #endregion

        #region 列举一组书籍

        /// <summary>
        ///     列举一组书籍。
        /// </summary>
        [CacheResponse(Duration = 600)]
        public async Task<object> Get(BookList request)
        {
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                BookListValidator.ValidateAndThrow(request, ApplyTo.Get);
            }
            var existingBooks = await BookRepo.FindBooksAsync(request.TitleFilter, request.Tag, request.PublishedSince, request.IsPublished, request.OrderBy, request.Descending, request.Skip, request.Limit);
            if (existingBooks == null)
            {
                throw HttpError.NotFound(string.Format(Resources.BooksNotFound));
            }
            var booksDto = existingBooks.Select(book => book.MapToBookDto()).ToList();
            return new BookListResponse
                   {
                       Books = booksDto
                   };
        }

        #endregion
    }
}