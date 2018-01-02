using System.Threading.Tasks;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Model.Bookstore;
using Sheep.Model.Content;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Views;

namespace Sheep.ServiceInterface.Views
{
    /// <summary>
    ///     删除一个阅读服务接口。
    /// </summary>
    public class DeleteViewService : ChangeViewService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(DeleteViewService));

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
        public IValidator<ViewDelete> ViewDeleteValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置阅读的存储库。
        /// </summary>
        public IViewRepository ViewRepo { get; set; }

        /// <summary>
        ///     获取及设置帖子的存储库。
        /// </summary>
        public IPostRepository PostRepo { get; set; }

        /// <summary>
        ///     获取及设置章的存储库。
        /// </summary>
        public IChapterRepository ChapterRepo { get; set; }

        /// <summary>
        ///     获取及设置节的存储库。
        /// </summary>
        public IParagraphRepository ParagraphRepo { get; set; }

        #endregion

        #region 删除一个阅读

        /// <summary>
        ///     删除一个阅读。
        /// </summary>
        public async Task<object> Delete(ViewDelete request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    ViewDeleteValidator.ValidateAndThrow(request, ApplyTo.Delete);
            //}
            var existingView = await ViewRepo.GetViewAsync(request.ViewId);
            if (existingView == null)
            {
                throw HttpError.NotFound(string.Format(Resources.ViewNotFound, request.ViewId));
            }
            var currentUserId = GetSession().UserAuthId.ToInt(0);
            if (existingView.UserId != currentUserId)
            {
                throw HttpError.Unauthorized(Resources.LoginAsAuthorRequired);
            }
            await ViewRepo.DeleteViewAsync(request.ViewId);
            ResetCache(existingView);
            switch (existingView.ParentType)
            {
                case "帖子":
                    await PostRepo.IncrementPostViewsCountAsync(existingView.ParentId, -1);
                    break;
                case "章":
                    await ChapterRepo.IncrementChapterViewsCountAsync(existingView.ParentId, -1);
                    break;
                case "节":
                    await ParagraphRepo.IncrementParagraphViewsCountAsync(existingView.ParentId, -1);
                    break;
            }
            return new ViewDeleteResponse();
        }

        #endregion
    }
}