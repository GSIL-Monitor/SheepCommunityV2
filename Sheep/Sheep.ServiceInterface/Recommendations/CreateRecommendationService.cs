using System.Threading.Tasks;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Model.Content;
using Sheep.Model.Content.Entities;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceInterface.Recommendations.Mappers;
using Sheep.ServiceModel.Recommendations;

namespace Sheep.ServiceInterface.Recommendations
{
    /// <summary>
    ///     新建一个推荐 服务接口。
    /// </summary>
    public class CreateRecommendationService : ChangeRecommendationService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CreateRecommendationService));

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
        ///     获取及设置新建一个推荐的校验器。
        /// </summary>
        public IValidator<RecommendationCreate> RecommendationCreateValidator { get; set; }

        /// <summary>
        ///     获取及设置推荐 的存储库。
        /// </summary>
        public IRecommendationRepository RecommendationRepo { get; set; }

        /// <summary>
        ///     获取及设置帖子的存储库。
        /// </summary>
        public IPostRepository PostRepo { get; set; }

        #endregion

        #region 新建一个推荐 

        /// <summary>
        ///     新建一个推荐 。
        /// </summary>
        public async Task<object> Post(RecommendationCreate request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    RecommendationCreateValidator.ValidateAndThrow(request, ApplyTo.Post);
            //}
            var newRecommendation = new Recommendation
                                    {
                                        ContentType = request.ContentType,
                                        ContentId = request.ContentId,
                                        Position = request.Position
                                    };
            var recommendation = await RecommendationRepo.CreateRecommendationAsync(newRecommendation);
            ResetCache(recommendation);
            switch (recommendation.ContentType)
            {
                case "帖子":
                    var post = await PostRepo.GetPostAsync(recommendation.ContentId);
                    if (post == null)
                    {
                    }
                    return new RecommendationCreateResponse
                           {
                               Recommendation = recommendation.MapToRecommendationDto(post)
                           };
            }
            return new RecommendationCreateResponse
                   {
                       Recommendation = recommendation.MapToRecommendationDto(null)
                   };
        }

        #endregion
    }
}