using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Text;
using Sheep.Model.Content;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceInterface.Recommendations.Mappers;
using Sheep.ServiceModel.Recommendations;

namespace Sheep.ServiceInterface.Recommendations
{
    /// <summary>
    ///     根据用户列举一组推荐 信息服务接口。
    /// </summary>
    [CompressResponse]
    public class ListRecommendationService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListRecommendationService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置列举一组推荐的校验器。
        /// </summary>
        public IValidator<RecommendationList> RecommendationListValidator { get; set; }

        /// <summary>
        ///     获取及设置推荐 的存储库。
        /// </summary>
        public IRecommendationRepository RecommendationRepo { get; set; }

        /// <summary>
        ///     获取及设置帖子的存储库。
        /// </summary>
        public IPostRepository PostRepo { get; set; }

        #endregion

        #region 列举一组推荐 

        /// <summary>
        ///     列举一组推荐 。
        /// </summary>
        //[CacheResponse(Duration = 3600)]
        public async Task<object> Get(RecommendationList request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    RecommendationListValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var existingRecommendations = await RecommendationRepo.FindLatestPositionRecommendationsAsync(request.ContentType, request.CreatedSince?.FromUnixTime());
            if (existingRecommendations == null)
            {
                throw HttpError.NotFound(string.Format(Resources.RecommendationsNotFound));
            }
            var postsMap = (await PostRepo.GetPostsAsync(existingRecommendations.Where(recommendation => recommendation.ContentType == "帖子").Select(recommendation => recommendation.ContentId.ToString()).Distinct().ToList())).ToDictionary(post => post.Id, post => post);
            var recommendationsDto = existingRecommendations.Select(recommendation => recommendation.ContentType == "帖子" ? recommendation.MapToRecommendationDto(postsMap.GetValueOrDefault(recommendation.ContentId)) : null).ToList();
            return new RecommendationListResponse
                   {
                       Recommendations = recommendationsDto
                   };
        }

        #endregion
    }
}