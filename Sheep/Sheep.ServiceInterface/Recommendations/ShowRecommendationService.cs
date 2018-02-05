using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Model.Content;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceInterface.Recommendations.Mappers;
using Sheep.ServiceModel.Recommendations;

namespace Sheep.ServiceInterface.Recommendations
{
    /// <summary>
    ///     显示一个推荐 服务接口。
    /// </summary>
    [CompressResponse]
    public class ShowRecommendationService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowRecommendationService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置显示一个推荐的校验器。
        /// </summary>
        public IValidator<RecommendationShow> RecommendationShowValidator { get; set; }

        /// <summary>
        ///     获取及设置推荐 的存储库。
        /// </summary>
        public IRecommendationRepository RecommendationRepo { get; set; }

        /// <summary>
        ///     获取及设置帖子的存储库。
        /// </summary>
        public IPostRepository PostRepo { get; set; }

        #endregion

        #region 显示一个推荐 

        /// <summary>
        ///     显示一个推荐 。
        /// </summary>
        public async Task<object> Get(RecommendationShow request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    RecommendationShowValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var existingRecommendation = await RecommendationRepo.GetRecommendationAsync(request.RecommendationId);
            if (existingRecommendation == null)
            {
                throw HttpError.NotFound(string.Format(Resources.RecommendationNotFound, request.RecommendationId));
            }
            switch (existingRecommendation.ContentType)
            {
                case "帖子":
                    var post = await PostRepo.GetPostAsync(existingRecommendation.ContentId);
                    if (post == null)
                    {
                    }
                    return new RecommendationCreateResponse
                           {
                               Recommendation = existingRecommendation.MapToRecommendationDto(post)
                           };
            }
            return new RecommendationCreateResponse
                   {
                       Recommendation = existingRecommendation.MapToRecommendationDto(null)
                   };
        }

        #endregion
    }
}