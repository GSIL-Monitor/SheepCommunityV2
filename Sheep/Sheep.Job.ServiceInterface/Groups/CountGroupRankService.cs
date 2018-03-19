using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Job.ServiceInterface.Properties;
using Sheep.Job.ServiceModel.Groups;
using Sheep.Model.Bookstore;
using Sheep.Model.Content;
using Sheep.Model.Membership;
using Sheep.Model.Membership.Entities;

namespace Sheep.Job.ServiceInterface.Groups
{
    /// <summary>
    ///     统计一组群组排行服务接口。
    /// </summary>
    public class CountGroupRankService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CountGroupRankService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置统计一组群组排行的校验器。
        /// </summary>
        public IValidator<GroupRankCount> GroupRankCountValidator { get; set; }

        /// <summary>
        ///     获取及设置群组的存储库。
        /// </summary>
        public IGroupRepository GroupRepo { get; set; }

        /// <summary>
        ///     获取及设置群组成员的存储库。
        /// </summary>
        public IGroupMemberRepository GroupMemberRepo { get; set; }

        /// <summary>
        ///     获取及设置群组排行的存储库。
        /// </summary>
        public IGroupRankRepository GroupRankRepo { get; set; }

        /// <summary>
        ///     获取及设置查看的存储库。
        /// </summary>
        public IViewRepository ViewRepo { get; set; }

        /// <summary>
        ///     获取及设置帖子的存储库。
        /// </summary>
        public IPostRepository PostRepo { get; set; }

        /// <summary>
        ///     获取及设置节的存储库。
        /// </summary>
        public IParagraphRepository ParagraphRepo { get; set; }

        #endregion

        #region 统计一组群组排行

        /// <summary>
        ///     统计一组群组排行。
        /// </summary>
        public async Task<object> Put(GroupRankCount request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    GroupRankCountValidator.ValidateAndThrow(request, ApplyTo.Put);
            //}
            var existingGroups = await GroupRepo.FindGroupsAsync(null, null, null, null, null, null, null);
            if (existingGroups == null)
            {
                throw HttpError.NotFound(string.Format(Resources.GroupsNotFound));
            }
            var postViewsCountsMap = (await ViewRepo.GetPostViewsCountByAllGroupsAsync()).ToDictionary(pair => pair.Key, pair => pair.Value);
            var paragraphViewsCountsMap = (await ViewRepo.GetParagraphViewsCountByAllGroupsAsync()).ToDictionary(pair => pair.Key, pair => pair.Value);
            var groupRanks = new List<GroupRank>(existingGroups.Count);
            foreach (var existingGroup in existingGroups)
            {
                var existingGroupRank = await GroupRankRepo.GetGroupRankAsync(existingGroup.Id);
                GroupRank groupRank;
                if (existingGroupRank != null)
                {
                    groupRank = await GroupRankRepo.UpdateGroupRankAsync(existingGroupRank, new GroupRank
                                                                                            {
                                                                                                Id = existingGroup.Id,
                                                                                                LastPostViewsCount = existingGroupRank.PostViewsCount,
                                                                                                LastPostViewsRank = existingGroupRank.PostViewsRank,
                                                                                                PostViewsCount = postViewsCountsMap.GetValueOrDefault(existingGroup.Id),
                                                                                                PostViewsRank = 0,
                                                                                                LastParagraphViewsCount = existingGroupRank.ParagraphViewsCount,
                                                                                                LastParagraphViewsRank = existingGroupRank.ParagraphViewsRank,
                                                                                                ParagraphViewsCount = paragraphViewsCountsMap.GetValueOrDefault(existingGroup.Id),
                                                                                                ParagraphViewsRank = 0
                                                                                            });
                }
                else
                {
                    groupRank = await GroupRankRepo.CreateGroupRankAsync(new GroupRank
                                                                         {
                                                                             Id = existingGroup.Id,
                                                                             LastPostViewsCount = 0,
                                                                             LastPostViewsRank = int.MaxValue,
                                                                             PostViewsCount = postViewsCountsMap.GetValueOrDefault(existingGroup.Id),
                                                                             PostViewsRank = 0,
                                                                             LastParagraphViewsCount = 0,
                                                                             LastParagraphViewsRank = int.MaxValue,
                                                                             ParagraphViewsCount = paragraphViewsCountsMap.GetValueOrDefault(existingGroup.Id),
                                                                             ParagraphViewsRank = 0
                                                                         });
                }
                groupRanks.Add(groupRank);
            }
            var rankedByPostViewsCountGroupRanks = groupRanks.OrderByDescending(groupRank => groupRank.PostViewsCount).ToList();
            for (var index = 0; index < rankedByPostViewsCountGroupRanks.Count; index++)
            {
                var groupRank = rankedByPostViewsCountGroupRanks[index];
                groupRank.PostViewsRank = index + 1;
            }
            var rankedByParagraphViewsCountGroupRanks = groupRanks.OrderByDescending(groupRank => groupRank.ParagraphViewsCount).ToList();
            for (var index = 0; index < rankedByParagraphViewsCountGroupRanks.Count; index++)
            {
                var groupRank = rankedByParagraphViewsCountGroupRanks[index];
                groupRank.ParagraphViewsRank = index + 1;
            }
            foreach (var groupRank in groupRanks)
            {
                await GroupRankRepo.UpdateGroupRankAsync(groupRank, groupRank);
            }
            return new GroupRankCountResponse();
        }

        #endregion
    }
}