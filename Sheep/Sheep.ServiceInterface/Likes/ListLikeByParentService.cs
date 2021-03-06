﻿using System.Linq;
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
using Sheep.ServiceInterface.Likes.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Likes;

namespace Sheep.ServiceInterface.Likes
{
    /// <summary>
    ///     根据上级列举一组点赞信息服务接口。
    /// </summary>
    [CompressResponse]
    public class ListLikeByParentService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListLikeByParentService));

        #endregion

        #region 属性

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置列举一组点赞的校验器。
        /// </summary>
        public IValidator<LikeListByParent> LikeListByParentValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置点赞的存储库。
        /// </summary>
        public ILikeRepository LikeRepo { get; set; }

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

        #region 列举一组点赞

        /// <summary>
        ///     列举一组点赞。
        /// </summary>
        //[CacheResponse(Duration = 3600)]
        public async Task<object> Get(LikeListByParent request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    LikeListByParentValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var existingLikes = await LikeRepo.FindLikesByParentAsync(request.ParentId, request.CreatedSince?.FromUnixTime(), request.OrderBy, request.Descending, request.Skip, request.Limit);
            if (existingLikes == null)
            {
                throw HttpError.NotFound(string.Format(Resources.LikesNotFound));
            }
            var postsMap = (await PostRepo.GetPostsAsync(existingLikes.Where(like => like.ParentType == "帖子").Select(like => like.ParentId).Distinct().ToList())).ToDictionary(post => post.Id, post => post);
            var chaptersMap = (await ChapterRepo.GetChaptersAsync(existingLikes.Where(like => like.ParentType == "章").Select(like => like.ParentId).Distinct().ToList())).ToDictionary(chapter => chapter.Id, chapter => chapter);
            var paragraphsMap = (await ParagraphRepo.GetParagraphsAsync(existingLikes.Where(like => like.ParentType == "节").Select(like => like.ParentId).Distinct().ToList())).ToDictionary(paragraph => paragraph.Id, paragraph => paragraph);
            var usersMap = (await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthsAsync(existingLikes.Select(like => like.UserId.ToString()).Distinct().ToList())).ToDictionary(userAuth => userAuth.Id, userAuth => userAuth);
            var likesDto = existingLikes.Select(like => like.MapToLikeDto(like.ParentType == "帖子" ? postsMap.GetValueOrDefault(like.ParentId)?.Title : (like.ParentType == "章" ? chaptersMap.GetValueOrDefault(like.ParentId)?.Title : paragraphsMap.GetValueOrDefault(like.ParentId)?.Content), like.ParentType == "帖子" ? postsMap.GetValueOrDefault(like.ParentId)?.PictureUrl : null, like.ParentType == "帖子" ? postsMap.GetValueOrDefault(like.ParentId)?.ContentType : null, usersMap.GetValueOrDefault(like.UserId))).ToList();
            return new LikeListResponse
                   {
                       Likes = likesDto
                   };
        }

        #endregion
    }
}