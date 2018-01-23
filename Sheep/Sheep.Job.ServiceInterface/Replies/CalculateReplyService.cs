using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Text;
using Sheep.Job.ServiceInterface.Properties;
using Sheep.Job.ServiceModel.Replies;
using Sheep.Model.Content;

namespace Sheep.Job.ServiceInterface.Replies
{
    /// <summary>
    ///     计算一组回复分数服务接口。
    /// </summary>
    public class CalculateReplyService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CalculateReplyService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置计算一组回复分数的校验器。
        /// </summary>
        public IValidator<ReplyCalculate> ReplyCalculateValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置回复的存储库。
        /// </summary>
        public IReplyRepository ReplyRepo { get; set; }

        #endregion

        #region 计算一组回复分数

        /// <summary>
        ///     计算一组回复分数。
        /// </summary>
        public async Task<object> Put(ReplyCalculate request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    ReplyCalculateValidator.ValidateAndThrow(request, ApplyTo.Put);
            //}
            var existingReplies = await ReplyRepo.FindRepliesAsync(request.ParentType, request.CreatedSince?.FromUnixTime(), request.ModifiedSince?.FromUnixTime(), "审核通过", request.OrderBy, request.Descending, request.Skip, request.Limit);
            if (existingReplies == null)
            {
                throw HttpError.NotFound(string.Format(Resources.RepliesNotFound));
            }
            foreach (var existingReply in existingReplies)
            {
                await ReplyRepo.UpdateReplyContentQualityAsync(existingReply.Id, ReplyRepo.CalculateReplyContentQuality(existingReply));
            }
            return new ReplyCalculateResponse();
        }

        #endregion
    }
}