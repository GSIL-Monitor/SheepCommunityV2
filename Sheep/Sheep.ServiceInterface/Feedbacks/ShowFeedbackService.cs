using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Common.Auth;
using Sheep.Model.Content;
using Sheep.ServiceInterface.Feedbacks.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Feedbacks;

namespace Sheep.ServiceInterface.Feedbacks
{
    /// <summary>
    ///     显示一个举报服务接口。
    /// </summary>
    [CompressResponse]
    public class ShowFeedbackService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowFeedbackService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置显示一个举报的校验器。
        /// </summary>
        public IValidator<FeedbackShow> FeedbackShowValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置举报的存储库。
        /// </summary>
        public IFeedbackRepository FeedbackRepo { get; set; }

        #endregion

        #region 显示一个举报

        /// <summary>
        ///     显示一个举报。
        /// </summary>
        public async Task<object> Get(FeedbackShow request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    FeedbackShowValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var existingFeedback = await FeedbackRepo.GetFeedbackAsync(request.FeedbackId);
            if (existingFeedback == null)
            {
                throw HttpError.NotFound(string.Format(Resources.FeedbackNotFound, request.FeedbackId));
            }
            var user = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(existingFeedback.UserId.ToString());
            if (user == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, existingFeedback.UserId));
            }
            var feedbackDto = existingFeedback.MapToFeedbackDto(user);
            return new FeedbackShowResponse
                   {
                       Feedback = feedbackDto
                   };
        }

        #endregion
    }
}