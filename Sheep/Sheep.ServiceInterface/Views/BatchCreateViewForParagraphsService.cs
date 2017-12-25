﻿using System.Linq;
using System.Threading.Tasks;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Common.Auth;
using Sheep.Model.Bookstore;
using Sheep.Model.Content;
using Sheep.Model.Content.Entities;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Views;

namespace Sheep.ServiceInterface.Views
{
    /// <summary>
    ///     新建一组节阅读服务接口。
    /// </summary>
    public class BatchCreateViewForParagraphsService : ChangeViewService
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
        ///     获取及设置新建一组节阅读的校验器。
        /// </summary>
        public IValidator<ViewBatchCreateForParagraphs> ViewBatchCreateForParagraphsValidator { get; set; }

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

        #region 新建一组节阅读

        /// <summary>
        ///     新建一组节阅读。
        /// </summary>
        public async Task<object> Post(ViewBatchCreateForParagraphs request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                ViewBatchCreateForParagraphsValidator.ValidateAndThrow(request, ApplyTo.Post);
            }
            var existingParagraphs = await ParagraphRepo.FindParagraphsInRangeAsync(request.BookId, request.VolumeNumber, request.BeginChapterNumber, request.BeginParagraphNumber, request.EndChapterNumber, request.EndParagraphNumber, null, null, null, null);
            if (existingParagraphs == null)
            {
                throw HttpError.NotFound(string.Format(Resources.ParagraphsNotFound));
            }
            var currentUserId = GetSession().UserAuthId.ToInt(0);
            var currentUserAuth = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(currentUserId.ToString());
            if (currentUserAuth == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, currentUserId));
            }
            var newViews = existingParagraphs.Select(paragraph =>
                                                     {
                                                         var newView = new View
                                                                       {
                                                                           ParentType = "节",
                                                                           ParentId = paragraph.Id,
                                                                           UserId = currentUserId
                                                                       };
                                                         ResetCache(newView);
                                                         return newView;
                                                     })
                                             .ToList();
            await ViewRepo.CreateViewsAsync(newViews);
            await ParagraphRepo.IncrementParagraphsViewsCountAsync(existingParagraphs.Select(paragraph => paragraph.Id).ToList(), 1);
            //await NimClient.PostAsync(new FriendAddRequest
            //                          {
            //                              AccountId = currentUserId.ToString(),
            //                              FriendAccountId = request.ParentId.ToString(),
            //                              Type = 1
            //                          });
            return new ViewBatchCreateResponse();
        }

        #endregion
    }
}