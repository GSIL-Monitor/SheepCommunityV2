using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Common.Auth;
using Sheep.Model.Content;
using Sheep.ServiceInterface.AbuseReports.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.AbuseReports;

namespace Sheep.ServiceInterface.AbuseReports
{
    /// <summary>
    ///     显示一个举报服务接口。
    /// </summary>
    [CompressResponse]
    public class ShowAbuseReportService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowAbuseReportService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置显示一个举报的校验器。
        /// </summary>
        public IValidator<AbuseReportShow> AbuseReportShowValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

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

        /// <summary>
        ///     获取及设置举报的存储库。
        /// </summary>
        public IAbuseReportRepository AbuseReportRepo { get; set; }

        #endregion

        #region 显示一个举报

        /// <summary>
        ///     显示一个举报。
        /// </summary>
        public async Task<object> Get(AbuseReportShow request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    AbuseReportShowValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var existingAbuseReport = await AbuseReportRepo.GetAbuseReportAsync(request.ReportId);
            if (existingAbuseReport == null)
            {
                throw HttpError.NotFound(string.Format(Resources.AbuseReportNotFound, request.ReportId));
            }
            var user = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(existingAbuseReport.UserId.ToString());
            if (user == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, existingAbuseReport.UserId));
            }
            string title = null;
            string pictureUrl = null;
            IUserAuth abuseUser = null;
            switch (existingAbuseReport.ParentType)
            {
                case "用户":
                    abuseUser = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(existingAbuseReport.ParentId);
                    if (abuseUser != null)
                    {
                        title = abuseUser.DisplayName;
                        pictureUrl = abuseUser.Meta.GetValueOrDefault("AvatarUrl");
                    }
                    break;
                case "帖子":
                    var post = await PostRepo.GetPostAsync(existingAbuseReport.ParentId);
                    if (post != null)
                    {
                        title = post.Title;
                        pictureUrl = post.PictureUrl;
                        abuseUser = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(post.AuthorId.ToString());
                    }
                    break;
                case "评论":
                    var comment = await CommentRepo.GetCommentAsync(existingAbuseReport.ParentId);
                    if (comment != null)
                    {
                        title = comment.Content;
                        abuseUser = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(comment.UserId.ToString());
                    }
                    break;
                case "回复":
                    var reply = await ReplyRepo.GetReplyAsync(existingAbuseReport.ParentId);
                    if (reply != null)
                    {
                        title = reply.Content;
                        abuseUser = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(reply.UserId.ToString());
                    }
                    break;
            }
            var reportDto = existingAbuseReport.MapToAbuseReportDto(title, pictureUrl, abuseUser, user);
            return new AbuseReportShowResponse
                   {
                       AbuseReport = reportDto
                   };
        }

        #endregion
    }
}