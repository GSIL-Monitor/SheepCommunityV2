using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Model.Bookstore;
using Sheep.ServiceInterface.Books.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Books;

namespace Sheep.ServiceInterface.Books
{
    /// <summary>
    ///     显示一本书籍服务接口。
    /// </summary>
    public class ShowBookService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowBookService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置显示一本书籍的校验器。
        /// </summary>
        public IValidator<BookShow> BookShowValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置书籍的存储库。
        /// </summary>
        public IBookRepository BookRepo { get; set; }

        #endregion

        #region 显示一本书籍

        /// <summary>
        ///     显示一本书籍。
        /// </summary>
        [CacheResponse(Duration = 600)]
        public async Task<object> Get(BookShow request)
        {
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                BookShowValidator.ValidateAndThrow(request, ApplyTo.Get);
            }
            var existingBook = await BookRepo.GetBookAsync(request.BookId);
            if (existingBook == null)
            {
                throw HttpError.NotFound(string.Format(Resources.BookNotFound, request.BookId));
            }
            var bookDto = existingBook.MapToBookDto();
            return new BookShowResponse
                   {
                       Book = bookDto
                   };
        }

        #endregion
    }
}