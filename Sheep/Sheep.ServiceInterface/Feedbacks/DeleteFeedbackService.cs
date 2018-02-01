using System.Threading.Tasks;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Model.Content;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Feedbacks;

namespace Sheep.ServiceInterface.Feedbacks
{
    /// <summary>
    ///     删除一个举报服务接口。
    /// </summary>
    public class DeleteFeedbackService : ChangeFeedbackService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(DeleteFeedbackService));

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
        ///     获取及设置删除一个举报的校验器。
        /// </summary>
        public IValidator<FeedbackDelete> FeedbackDeleteValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置举报的存储库。
        /// </summary>
        public IFeedbackRepository FeedbackRepo { get; set; }

        #endregion

        #region 删除一个举报

        /// <summary>
        ///     删除一个举报。
        /// </summary>
        public async Task<object> Delete(FeedbackDelete request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    FeedbackDeleteValidator.ValidateAndThrow(request, ApplyTo.Delete);
            //}
            var existingFeedback = await FeedbackRepo.GetFeedbackAsync(request.FeedbackId);
            if (existingFeedback == null)
            {
                throw HttpError.NotFound(string.Format(Resources.FeedbackNotFound, request.FeedbackId));
            }
            var currentUserId = GetSession().UserAuthId.ToInt(0);
            if (existingFeedback.UserId != currentUserId)
            {
                throw HttpError.Unauthorized(Resources.LoginAsAuthorRequired);
            }
            await FeedbackRepo.DeleteFeedbackAsync(request.FeedbackId);
            ResetCache(existingFeedback);
            return new FeedbackDeleteResponse();
        }

        #endregion
    }
}