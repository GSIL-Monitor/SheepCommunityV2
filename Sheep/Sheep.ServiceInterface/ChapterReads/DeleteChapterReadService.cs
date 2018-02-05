using System.Threading.Tasks;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Model.Bookstore;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.ChapterReads;

namespace Sheep.ServiceInterface.ChapterReads
{
    /// <summary>
    ///     删除一个阅读服务接口。
    /// </summary>
    public class DeleteChapterReadService : ChangeChapterReadService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(DeleteChapterReadService));

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
        ///     获取及设置删除一个阅读的校验器。
        /// </summary>
        public IValidator<ChapterReadDelete> ChapterReadDeleteValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置阅读的存储库。
        /// </summary>
        public IChapterReadRepository ChapterReadRepo { get; set; }

        /// <summary>
        ///     获取及设置章的存储库。
        /// </summary>
        public IChapterRepository ChapterRepo { get; set; }

        #endregion

        #region 删除一个阅读

        /// <summary>
        ///     删除一个阅读。
        /// </summary>
        public async Task<object> Delete(ChapterReadDelete request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    ChapterReadDeleteValidator.ValidateAndThrow(request, ApplyTo.Delete);
            //}
            var existingChapterRead = await ChapterReadRepo.GetChapterReadAsync(request.ChapterReadId);
            if (existingChapterRead == null)
            {
                throw HttpError.NotFound(string.Format(Resources.ChapterReadNotFound, request.ChapterReadId));
            }
            //var currentUserId = GetSession().UserAuthId.ToInt(0);
            //if (existingChapterRead.UserId != currentUserId)
            //{
            //    throw HttpError.Unauthorized(Resources.LoginAsAuthorRequired);
            //}
            await ChapterReadRepo.DeleteChapterReadAsync(request.ChapterReadId);
            ResetCache(existingChapterRead);
            return new ChapterReadDeleteResponse();
        }

        #endregion
    }
}