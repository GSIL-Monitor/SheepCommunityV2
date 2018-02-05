using System.Threading.Tasks;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Model.Content;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.AbuseReports;

namespace Sheep.ServiceInterface.AbuseReports
{
    /// <summary>
    ///     删除一个举报服务接口。
    /// </summary>
    public class DeleteAbuseReportService : ChangeAbuseReportService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(DeleteAbuseReportService));

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
        public IValidator<AbuseReportDelete> AbuseReportDeleteValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置举报的存储库。
        /// </summary>
        public IAbuseReportRepository AbuseReportRepo { get; set; }

        /// <summary>
        ///     获取及设置帖子的存储库。
        /// </summary>
        public IPostRepository PostRepo { get; set; }

        /// <summary>
        ///     获取及设置评论的存储库。
        /// </summary>
        public ICommentRepository CommentRepo { get; set; }

        /// <summary>
        ///     获取及设置回复的存储库。
        /// </summary>
        public IReplyRepository ReplyRepo { get; set; }

        #endregion

        #region 删除一个举报

        /// <summary>
        ///     删除一个举报。
        /// </summary>
        public async Task<object> Delete(AbuseReportDelete request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    AbuseReportDeleteValidator.ValidateAndThrow(request, ApplyTo.Delete);
            //}
            var existingAbuseReport = await AbuseReportRepo.GetAbuseReportAsync(request.ReportId);
            if (existingAbuseReport == null)
            {
                throw HttpError.NotFound(string.Format(Resources.AbuseReportNotFound, request.ReportId));
            }
            //var currentUserId = GetSession().UserAuthId.ToInt(0);
            //if (existingAbuseReport.UserId != currentUserId)
            //{
            //    throw HttpError.Unauthorized(Resources.LoginAsAuthorRequired);
            //}
            await AbuseReportRepo.DeleteAbuseReportAsync(request.ReportId);
            ResetCache(existingAbuseReport);
            switch (existingAbuseReport.ParentType)
            {
                case "帖子":
                    await PostRepo.IncrementPostAbuseReportsCountAsync(existingAbuseReport.ParentId, -1);
                    break;
            }
            return new AbuseReportDeleteResponse();
        }

        #endregion
    }
}