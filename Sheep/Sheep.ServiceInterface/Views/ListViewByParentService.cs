using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Text;
using Sheep.Common.Auth;
using Sheep.Model.Bookstore;
using Sheep.Model.Content;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceInterface.Views.Mappers;
using Sheep.ServiceModel.Views;

namespace Sheep.ServiceInterface.Views
{
    /// <summary>
    ///     根据上级列举一组查看信息服务接口。
    /// </summary>
    [CompressResponse]
    public class ListViewByParentService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListViewByParentService));

        #endregion

        #region 属性

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置列举一组查看的校验器。
        /// </summary>
        public IValidator<ViewListByParent> ViewListByParentValidator { get; set; }

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

        #region 列举一组查看

        /// <summary>
        ///     列举一组查看。
        /// </summary>
        //[CacheResponse(Duration = 3600)]
        public async Task<object> Get(ViewListByParent request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    ViewListByParentValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var currentUserId = GetSession().UserAuthId.ToInt(0);
            var existingViews = await ViewRepo.FindViewsByParentAsync(request.ParentId, request.IsMine.HasValue && request.IsMine.Value ? currentUserId : (int?) null, request.CreatedSince?.FromUnixTime(), request.OrderBy, request.Descending, request.Skip, request.Limit);
            if (existingViews == null)
            {
                throw HttpError.NotFound(string.Format(Resources.ViewsNotFound));
            }
            var postTitlesMap = (await PostRepo.GetPostsAsync(existingViews.Where(view => view.ParentType == "帖子").Select(view => view.ParentId).Distinct().ToList())).ToDictionary(post => post.Id, post => post.Title);
            var chapterTitlesMap = (await ChapterRepo.GetChaptersAsync(existingViews.Where(view => view.ParentType == "章").Select(view => view.ParentId).Distinct().ToList())).ToDictionary(chapter => chapter.Id, chapter => chapter.Title);
            var paragraphTitlesMap = (await ParagraphRepo.GetParagraphsAsync(existingViews.Where(view => view.ParentType == "节").Select(view => view.ParentId).Distinct().ToList())).ToDictionary(paragraph => paragraph.Id, paragraph => paragraph.Content);
            var usersMap = (await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthsAsync(existingViews.Select(view => view.UserId.ToString()).Distinct().ToList())).ToDictionary(userAuth => userAuth.Id, userAuth => userAuth);
            var viewsDto = existingViews.Select(view => view.MapToViewDto(usersMap.GetValueOrDefault(view.UserId), view.ParentType == "帖子" ? postTitlesMap.GetValueOrDefault(view.ParentId) : (view.ParentType == "章" ? chapterTitlesMap.GetValueOrDefault(view.ParentId) : paragraphTitlesMap.GetValueOrDefault(view.ParentId)))).ToList();
            return new ViewListResponse
                   {
                       Views = viewsDto
                   };
        }

        #endregion
    }
}