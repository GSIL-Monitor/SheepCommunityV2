﻿using System.Threading.Tasks;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Common.Auth;
using Sheep.Model.Content;
using Sheep.Model.Content.Entities;
using Sheep.ServiceInterface.AbuseReports.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.AbuseReports;

namespace Sheep.ServiceInterface.AbuseReports
{
    /// <summary>
    ///     新建一个举报服务接口。
    /// </summary>
    public class CreateAbuseReportService : ChangeAbuseReportService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CreateAbuseReportService));

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
        public IValidator<AbuseReportCreate> AbuseReportCreateValidator { get; set; }

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

        #region 新建一个举报

        /// <summary>
        ///     新建一个举报。
        /// </summary>
        public async Task<object> Post(AbuseReportCreate request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    AbuseReportCreateValidator.ValidateAndThrow(request, ApplyTo.Post);
            //}
            var currentUserId = GetSession().UserAuthId.ToInt(0);
            var currentUser = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(currentUserId.ToString());
            if (currentUser == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, currentUserId));
            }
            var newAbuseReport = new AbuseReport
                                 {
                                     ParentType = request.ParentType,
                                     ParentId = request.ParentId,
                                     UserId = currentUserId,
                                     Reason = request.Reason?.Replace("\"", "'")
                                 };
            var report = await AbuseReportRepo.CreateAbuseReportAsync(newAbuseReport);
            ResetCache(report);
            string title = null;
            string pictureUrl = null;
            IUserAuth abuseUser = null;
            switch (report.ParentType)
            {
                case "用户":
                    abuseUser = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(report.ParentId);
                    if (abuseUser != null)
                    {
                        title = abuseUser.DisplayName;
                        pictureUrl = abuseUser.Meta.GetValueOrDefault("AvatarUrl");
                    }
                    break;
                case "帖子":
                    await PostRepo.IncrementPostAbuseReportsCountAsync(report.ParentId, 1);
                    var post = await PostRepo.GetPostAsync(report.ParentId);
                    if (post != null)
                    {
                        title = post.Title;
                        pictureUrl = post.PictureUrl;
                        abuseUser = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(post.AuthorId.ToString());
                    }
                    break;
                case "评论":
                    var comment = await CommentRepo.GetCommentAsync(report.ParentId);
                    if (comment != null)
                    {
                        title = comment.Content;
                        abuseUser = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(comment.UserId.ToString());
                    }
                    break;
                case "回复":
                    var reply = await ReplyRepo.GetReplyAsync(report.ParentId);
                    if (reply != null)
                    {
                        title = reply.Content;
                        abuseUser = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(reply.UserId.ToString());
                    }
                    break;
            }
            return new AbuseReportCreateResponse
                   {
                       AbuseReport = report.MapToAbuseReportDto(title, pictureUrl, abuseUser, currentUser)
                   };
        }

        #endregion
    }
}