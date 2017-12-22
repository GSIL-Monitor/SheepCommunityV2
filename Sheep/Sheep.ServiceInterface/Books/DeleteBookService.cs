using System.Threading.Tasks;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Model.Bookstore;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Books;

namespace Sheep.ServiceInterface.Books
{
    /// <summary>
    ///     删除一本书籍服务接口。
    /// </summary>
    public class DeleteBookService : ChangeBookService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(DeleteBookService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     网易云通信服务客户端。
        /// </summary>
        public INimClient NimClient { get; set; }

        /// <summary>
        ///     获取及设置删除一本书籍的校验器。
        /// </summary>
        public IValidator<BookDelete> BookDeleteValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置书籍的存储库。
        /// </summary>
        public IBookRepository BookRepo { get; set; }

        #endregion

        #region 删除一本书籍

        /// <summary>
        ///     删除一本书籍。
        /// </summary>
        public async Task<object> Delete(BookDelete request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                BookDeleteValidator.ValidateAndThrow(request, ApplyTo.Delete);
            }
            var existingBook = await BookRepo.GetBookAsync(request.BookId);
            if (existingBook == null)
            {
                throw HttpError.NotFound(string.Format(Resources.BookNotFound, request.BookId));
            }
            await BookRepo.DeleteBookAsync(request.BookId);
            ResetCache(existingBook);
            return new BookDeleteResponse();
        }

        #endregion
    }
}