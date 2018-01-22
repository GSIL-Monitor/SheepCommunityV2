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
using Sheep.ServiceInterface.AbuseReports.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.AbuseReports;

namespace Sheep.ServiceInterface.AbuseReports
{
    /// <summary>
    ///     根据用户列举一组举报信息服务接口。
    /// </summary>
    public class ListAbuseReportByUserService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListAbuseReportByUserService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置列举一组举报的校验器。
        /// </summary>
        public IValidator<AbuseReportListByUser> AbuseReportListByUserValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置举报的存储库。
        /// </summary>
        public IAbuseReportRepository AbuseReportRepo { get; set; }

        /// <summary>
        ///     获取及设置投票的存储库。
        /// </summary>
        public IVoteRepository VoteRepo { get; set; }

        #endregion

        #region 列举一组举报

        /// <summary>
        ///     列举一组举报。
        /// </summary>
        //[CacheResponse(Duration = 3600)]
        public async Task<object> Get(AbuseReportListByUser request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    AbuseReportListByUserValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var existingAbuseReports = await AbuseReportRepo.FindAbuseReportsByUserAsync(request.UserId, request.ParentType, request.CreatedSince?.FromUnixTime(), request.ModifiedSince?.FromUnixTime(), request.Status, request.OrderBy, request.Descending, request.Skip, request.Limit);
            if (existingAbuseReports == null)
            {
                throw HttpError.NotFound(string.Format(Resources.AbuseReportsNotFound));
            }
            var usersMap = (await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthsAsync(existingAbuseReports.Select(report => report.UserId.ToString()).Distinct().ToList())).ToDictionary(userAuth => userAuth.Id, userAuth => userAuth);
            var reportsDto = existingAbuseReports.Select(report => report.MapToAbuseReportDto(usersMap.GetValueOrDefault(report.UserId))).ToList();
            return new AbuseReportListResponse
                   {
                       AbuseReports = reportsDto
                   };
        }

        #endregion
    }
}