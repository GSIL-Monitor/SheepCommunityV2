using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Common.Auth;
using Sheep.Model.Content;
using Sheep.ServiceInterface.Likes.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Likes;

namespace Sheep.ServiceInterface.Likes
{
    /// <summary>
    ///     根据上级列举一组点赞信息服务接口。
    /// </summary>
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

        #endregion

        #region 列举一组用户

        /// <summary>
        ///     列举一组用户。
        /// </summary>
        [CacheResponse(Duration = 600)]
        public async Task<object> Get(LikeListByParent request)
        {
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                LikeListByParentValidator.ValidateAndThrow(request, ApplyTo.Get);
            }
            var existingLikes = await LikeRepo.FindLikesByParentAsync(request.ParentId, request.CreatedSince, request.OrderBy, request.Descending, request.Skip, request.Limit);
            if (existingLikes == null)
            {
                throw HttpError.NotFound(string.Format(Resources.LikesNotFound));
            }
            var usersMap = (await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthsAsync(existingLikes.Select(like => like.UserId.ToString()).Distinct())).ToDictionary(userAuth => userAuth.Id, userAuth => userAuth);
            var likesDto = existingLikes.Select(like => like.MapToLikeDto(usersMap.GetValueOrDefault(like.UserId))).ToList();
            return new LikeListResponse
                   {
                       Likes = likesDto
                   };
        }

        #endregion
    }
}