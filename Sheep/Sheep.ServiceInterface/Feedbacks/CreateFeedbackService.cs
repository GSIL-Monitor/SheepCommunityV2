using System.Threading.Tasks;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Common.Auth;
using Sheep.Model.Content;
using Sheep.Model.Content.Entities;
using Sheep.ServiceInterface.Feedbacks.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Feedbacks;

namespace Sheep.ServiceInterface.Feedbacks
{
    /// <summary>
    ///     新建一个举报服务接口。
    /// </summary>
    public class CreateFeedbackService : ChangeFeedbackService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CreateFeedbackService));

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
        ///     获取及设置新建一个举报的校验器。
        /// </summary>
        public IValidator<FeedbackCreate> FeedbackCreateValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置举报的存储库。
        /// </summary>
        public IFeedbackRepository FeedbackRepo { get; set; }

        #endregion

        #region 新建一个举报

        /// <summary>
        ///     新建一个举报。
        /// </summary>
        public async Task<object> Post(FeedbackCreate request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    FeedbackCreateValidator.ValidateAndThrow(request, ApplyTo.Post);
            //}
            var currentUserId = GetSession().UserAuthId.ToInt(0);
            var currentUserAuth = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(currentUserId.ToString());
            if (currentUserAuth == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, currentUserId));
            }
            var newFeedback = new Feedback
                              {
                                  UserId = currentUserId,
                                  Content = request.Content?.Replace("\"", "'")
                              };
            var feedback = await FeedbackRepo.CreateFeedbackAsync(newFeedback);
            ResetCache(feedback);
            return new FeedbackCreateResponse
                   {
                       Feedback = feedback.MapToFeedbackDto(currentUserAuth)
                   };
        }

        #endregion
    }
}