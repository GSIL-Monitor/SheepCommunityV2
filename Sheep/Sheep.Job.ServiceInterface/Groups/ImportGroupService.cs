using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Text;
using Sheep.Job.ServiceInterface.Properties;
using Sheep.Job.ServiceModel.Groups;
using Sheep.Model.Membership;
using Sheep.Model.Membership.Entities;

namespace Sheep.Job.ServiceInterface.Groups
{
    /// <summary>
    ///     导入一组群组服务接口。
    /// </summary>
    public class ImportGroupService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ImportGroupService));

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
        ///     获取及设置导入一组群组的校验器。
        /// </summary>
        public IValidator<GroupImport> GroupImportValidator { get; set; }

        /// <summary>
        ///     获取及设置群组的存储库。
        /// </summary>
        public IGroupRepository GroupRepo { get; set; }

        /// <summary>
        ///     获取及设置群组成员的存储库。
        /// </summary>
        public IGroupMemberRepository GroupMemberRepo { get; set; }

        #endregion

        #region 导入一组群组

        /// <summary>
        ///     导入一组群组。
        /// </summary>
        public async Task<object> Put(GroupImport request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    GroupImportValidator.ValidateAndThrow(request, ApplyTo.Put);
            //}
            var existingGroups = await GroupRepo.FindGroupsAsync(string.Empty, request.CreatedSince?.FromUnixTime(), request.ModifiedSince?.FromUnixTime(), request.OrderBy, request.Descending, request.Skip, request.Limit);
            if (existingGroups == null)
            {
                throw HttpError.NotFound(string.Format(Resources.GroupsNotFound));
            }
            var teamQueryResponse = await NimClient.PostAsync(new TeamQueryRequest
                                                              {
                                                                  TeamIds = existingGroups.Select(group => group.Id).ToList(),
                                                                  Operation = 1
                                                              });
            if (teamQueryResponse.Code != 200)
            {
                throw new HttpError(HttpStatusCode.BadRequest, teamQueryResponse.Description.ToString());
            }
            var groupsMap = existingGroups.ToDictionary(group => group.Id, group => group);
            if (teamQueryResponse.TeamInfos != null)
            {
                foreach (var teamInfo in teamQueryResponse.TeamInfos)
                {
                    if (groupsMap.TryGetValue(teamInfo.TeamId, out var existingGroup))
                    {
                        await GroupRepo.UpdateGroupAsync(existingGroup, new Group
                                                                        {
                                                                            DisplayName = teamInfo.TeamName,
                                                                            Description = teamInfo.Intro,
                                                                            FullName = existingGroup.FullName,
                                                                            FullNameVerified = existingGroup.FullNameVerified,
                                                                            IconUrl = existingGroup.IconUrl,
                                                                            CoverPhotoUrl = existingGroup.CoverPhotoUrl
                                                                        });
                        if (teamInfo.MemberAccountIds != null)
                        {
                            foreach (var memberAccountId in teamInfo.MemberAccountIds)
                            {
                                await GroupMemberRepo.CreateGroupMemberAsync(new GroupMember
                                                                             {
                                                                                 GroupId = teamInfo.TeamId,
                                                                                 UserId = memberAccountId.ToInt()
                                                                             });
                            }
                        }
                    }
                }
            }
            return new GroupImportResponse();
        }

        #endregion
    }
}