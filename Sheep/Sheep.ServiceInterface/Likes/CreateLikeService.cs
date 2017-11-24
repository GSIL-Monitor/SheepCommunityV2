﻿using System.Threading.Tasks;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Common.Auth;
using Sheep.Model.Content;
using Sheep.Model.Content.Entities;
using Sheep.ServiceInterface.Likes.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Likes;

namespace Sheep.ServiceInterface.Likes
{
    /// <summary>
    ///     新建一个点赞服务接口。
    /// </summary>
    public class CreateLikeService : ChangeLikeService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CreateLikeService));

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
        ///     获取及设置新建一个点赞的校验器。
        /// </summary>
        public IValidator<LikeCreate> LikeCreateValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置点赞的存储库。
        /// </summary>
        public ILikeRepository LikeRepo { get; set; }

        #endregion

        #region 新建一个点赞

        /// <summary>
        ///     新建一个点赞。
        /// </summary>
        public async Task<object> Post(LikeCreate request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                LikeCreateValidator.ValidateAndThrow(request, ApplyTo.Post);
            }
            var userId = GetSession().UserAuthId.ToInt(0);
            var user = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(userId.ToString());
            if (user == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, userId));
            }
            var existingLike = await LikeRepo.GetLikeAsync(request.ContentId, userId);
            if (existingLike != null)
            {
                return new LikeCreateResponse
                       {
                           Like = existingLike.MapToLikeDto(user)
                       };
            }
            var newLike = new Like
                          {
                              ContentType = request.ContentType,
                              ContentId = request.ContentId,
                              UserId = userId
                          };
            var like = await LikeRepo.CreateLikeAsync(newLike);
            ResetCache(like);
            //await NimClient.PostAsync(new FriendAddRequest
            //                          {
            //                              AccountId = userId.ToString(),
            //                              FriendAccountId = request.ContentId.ToString(),
            //                              Type = 1
            //                          });
            return new LikeCreateResponse
                   {
                       Like = like.MapToLikeDto(user)
                   };
        }

        #endregion
    }
}