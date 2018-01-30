using System.Threading.Tasks;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Model.Content;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Recommendations;

namespace Sheep.ServiceInterface.Recommendations
{
    /// <summary>
    ///     删除一个推荐 服务接口。
    /// </summary>
    public class DeleteRecommendationService : ChangeRecommendationService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(DeleteRecommendationService));

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
        ///     获取及设置删除一个推荐的校验器。
        /// </summary>
        public IValidator<RecommendationDelete> RecommendationDeleteValidator { get; set; }

        /// <summary>
        ///     获取及设置推荐 的存储库。
        /// </summary>
        public IRecommendationRepository RecommendationRepo { get; set; }

        /// <summary>
        ///     获取及设置帖子的存储库。
        /// </summary>
        public IPostRepository PostRepo { get; set; }

        #endregion

        #region 删除一个推荐 

        /// <summary>
        ///     删除一个推荐 。
        /// </summary>
        public async Task<object> Delete(RecommendationDelete request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    RecommendationDeleteValidator.ValidateAndThrow(request, ApplyTo.Delete);
            //}
            var existingRecommendation = await RecommendationRepo.GetRecommendationAsync(request.RecommendationId);
            if (existingRecommendation == null)
            {
                throw HttpError.NotFound(string.Format(Resources.RecommendationNotFound, request.RecommendationId));
            }
            await RecommendationRepo.DeleteRecommendationAsync(request.RecommendationId);
            ResetCache(existingRecommendation);
            return new RecommendationDeleteResponse();
        }

        #endregion
    }
}