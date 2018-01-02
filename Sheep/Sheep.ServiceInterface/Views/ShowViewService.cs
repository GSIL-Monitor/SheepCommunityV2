using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Common.Auth;
using Sheep.Model.Bookstore;
using Sheep.Model.Content;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceInterface.Views.Mappers;
using Sheep.ServiceModel.Views;

namespace Sheep.ServiceInterface.Views
{
    /// <summary>
    ///     显示一个阅读服务接口。
    /// </summary>
    public class ShowViewService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowViewService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置显示一个阅读的校验器。
        /// </summary>
        public IValidator<ViewShow> ViewShowValidator { get; set; }

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

        #region 显示一个阅读

        /// <summary>
        ///     显示一个阅读。
        /// </summary>
        public async Task<object> Get(ViewShow request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    ViewShowValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var existingView = await ViewRepo.GetViewAsync(request.ViewId);
            if (existingView == null)
            {
                throw HttpError.NotFound(string.Format(Resources.ViewNotFound, request.ViewId));
            }
            var user = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(existingView.UserId.ToString());
            if (user == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, existingView.UserId));
            }
            var title = string.Empty;
            switch (existingView.ParentType)
            {
                case "帖子":
                    title = (await PostRepo.GetPostAsync(existingView.ParentId))?.Title;
                    break;
                case "章":
                    title = (await ChapterRepo.GetChapterAsync(existingView.ParentId))?.Title;
                    break;
                case "节":
                    title = (await ParagraphRepo.GetParagraphAsync(existingView.ParentId))?.Content;
                    break;
            }
            var viewDto = existingView.MapToViewDto(user, title);
            return new ViewShowResponse
                   {
                       View = viewDto
                   };
        }

        #endregion
    }
}