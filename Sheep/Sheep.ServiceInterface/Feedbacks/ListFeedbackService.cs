using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Text;
using Sheep.Common.Auth;
using Sheep.Model.Content;
using Sheep.ServiceInterface.Feedbacks.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Feedbacks;

namespace Sheep.ServiceInterface.Feedbacks
{
    /// <summary>
    ///     根据上级列举一组举报信息服务接口。
    /// </summary>
    public class ListFeedbackService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListFeedbackService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置列举一组举报的校验器。
        /// </summary>
        public IValidator<FeedbackList> FeedbackListValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置举报的存储库。
        /// </summary>
        public IFeedbackRepository FeedbackRepo { get; set; }

        #endregion

        #region 列举一组举报

        /// <summary>
        ///     列举一组举报。
        /// </summary>
        //[CacheResponse(Duration = 3600)]
        public async Task<object> Get(FeedbackList request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    FeedbackListValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var existingFeedbacks = await FeedbackRepo.FindFeedbacksAsync(request.ContentFilter, request.CreatedSince?.FromUnixTime(), request.ModifiedSince?.FromUnixTime(), request.Status, request.OrderBy, request.Descending, request.Skip, request.Limit);
            if (existingFeedbacks == null)
            {
                throw HttpError.NotFound(string.Format(Resources.FeedbacksNotFound));
            }
            var usersMap = (await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthsAsync(existingFeedbacks.Select(feedback => feedback.UserId.ToString()).Distinct().ToList())).ToDictionary(userAuth => userAuth.Id, userAuth => userAuth);
            var feedbacksDto = existingFeedbacks.Select(feedback => feedback.MapToFeedbackDto(usersMap.GetValueOrDefault(feedback.UserId))).ToList();
            return new FeedbackListResponse
                   {
                       Feedbacks = feedbacksDto
                   };
        }

        #endregion
    }
}