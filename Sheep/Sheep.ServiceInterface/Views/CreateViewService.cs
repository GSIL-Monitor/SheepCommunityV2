using System.Threading.Tasks;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Common.Auth;
using Sheep.Model.Bookstore;
using Sheep.Model.Content;
using Sheep.Model.Content.Entities;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceInterface.Views.Mappers;
using Sheep.ServiceModel.Views;

namespace Sheep.ServiceInterface.Views
{
    /// <summary>
    ///     新建一个查看服务接口。
    /// </summary>
    public class CreateViewService : ChangeViewService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CreateViewService));

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
        ///     获取及设置新建一个查看的校验器。
        /// </summary>
        public IValidator<ViewCreate> ViewCreateValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置查看的存储库。
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

        #region 新建一个查看

        /// <summary>
        ///     新建一个查看。
        /// </summary>
        public async Task<object> Post(ViewCreate request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    ViewCreateValidator.ValidateAndThrow(request, ApplyTo.Post);
            //}
            var currentUserId = GetSession().UserAuthId.ToInt(0);
            var currentUser = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(currentUserId.ToString());
            if (currentUser == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, currentUserId));
            }
            var newView = new View
                          {
                              ParentType = request.ParentType,
                              ParentId = request.ParentId,
                              UserId = currentUserId
                          };
            var view = await ViewRepo.CreateViewAsync(newView);
            ResetCache(view);
            var title = string.Empty;
            switch (view.ParentType)
            {
                case "帖子":
                    title = (await PostRepo.GetPostAsync(view.ParentId))?.Title;
                    await PostRepo.IncrementPostViewsCountAsync(view.ParentId, 1);
                    break;
                case "章":
                    title = (await ChapterRepo.GetChapterAsync(view.ParentId))?.Title;
                    await ChapterRepo.IncrementChapterViewsCountAsync(view.ParentId, 1);
                    break;
                case "节":
                    title = (await ParagraphRepo.GetParagraphAsync(view.ParentId))?.Content;
                    await ParagraphRepo.IncrementParagraphViewsCountAsync(view.ParentId, 1);
                    break;
            }
            //await NimClient.PostAsync(new FriendAddRequest
            //                          {
            //                              AccountId = currentUserId.ToString(),
            //                              FriendAccountId = request.ParentId.ToString(),
            //                              Type = 1
            //                          });
            return new ViewCreateResponse
                   {
                       View = view.MapToViewDto(currentUser, title)
                   };
        }

        #endregion
    }
}