using System.Collections.Generic;
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
    ///     更新一个反馈的状态服务接口。
    /// </summary>
    public class UpdateFeedbackStatusService : ChangeFeedbackService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(UpdateFeedbackStatusService));

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
        ///     获取及设置更新一个反馈的状态的校验器。
        /// </summary>
        public IValidator<FeedbackUpdateStatus> FeedbackUpdateStatusValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置举报的存储库。
        /// </summary>
        public IFeedbackRepository FeedbackRepo { get; set; }

        #endregion

        #region 更新一个反馈的状态

        /// <summary>
        ///     更新一个反馈的状态。
        /// </summary>
        public async Task<object> Put(FeedbackUpdateStatus request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    FeedbackUpdateStatusValidator.ValidateAndThrow(request, ApplyTo.Put);
            //}
            var existingFeedback = await FeedbackRepo.GetFeedbackAsync(request.FeedbackId);
            if (existingFeedback == null)
            {
                throw HttpError.NotFound(string.Format(Resources.FeedbackNotFound, request.FeedbackId));
            }
            var currentUserAuth = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(existingFeedback.UserId.ToString());
            if (currentUserAuth == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, existingFeedback.UserId));
            }
            var newFeedback = new Feedback();
            newFeedback.PopulateWith(existingFeedback);
            newFeedback.Meta = existingFeedback.Meta == null ? new Dictionary<string, string>() : new Dictionary<string, string>(existingFeedback.Meta);
            newFeedback.Status = request.Status;
            var feedback = await FeedbackRepo.UpdateFeedbackAsync(existingFeedback, newFeedback);
            ResetCache(feedback);
            return new FeedbackUpdateResponse
                   {
                       Feedback = feedback.MapToFeedbackDto(currentUserAuth)
                   };
        }

        #endregion
    }
}