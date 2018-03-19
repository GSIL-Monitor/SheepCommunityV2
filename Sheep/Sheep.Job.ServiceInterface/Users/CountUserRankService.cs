using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Common.Auth;
using Sheep.Job.ServiceInterface.Properties;
using Sheep.Job.ServiceModel.Users;
using Sheep.Model.Bookstore;
using Sheep.Model.Content;
using Sheep.Model.Membership;
using Sheep.Model.Membership.Entities;

namespace Sheep.Job.ServiceInterface.Users
{
    /// <summary>
    ///     统计一组用户排行服务接口。
    /// </summary>
    public class CountUserRankService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CountUserRankService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置统计一组用户排行的校验器。
        /// </summary>
        public IValidator<UserRankCount> UserRankCountValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置用户排行的存储库。
        /// </summary>
        public IUserRankRepository UserRankRepo { get; set; }

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

        #region 统计一组用户排行

        /// <summary>
        ///     统计一组用户排行。
        /// </summary>
        public async Task<object> Put(UserRankCount request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    UserRankCountValidator.ValidateAndThrow(request, ApplyTo.Put);
            //}
            var existingUserAuths = await ((IUserAuthRepositoryExtended) AuthRepo).FindUserAuthsAsync(null, null, null, null, null, null, null, null, null, null);
            if (existingUserAuths == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UsersNotFound));
            }
            var postViewsCountsMap = (await ViewRepo.GetViewsCountByAllUsersAsync("帖子", null, null)).ToDictionary(pair => pair.Key, pair => pair.Value);
            var paraggraphViewsCountsMap = (await ViewRepo.GetViewsCountByAllUsersAsync("节", null, null)).ToDictionary(pair => pair.Key, pair => pair.Value);
            var userRanks = new List<UserRank>(existingUserAuths.Count);
            foreach (var existingUser in existingUserAuths)
            {
                var existingUserRank = await UserRankRepo.GetUserRankAsync(existingUser.Id);
                UserRank userRank;
                if (existingUserRank != null)
                {
                    userRank = await UserRankRepo.UpdateUserRankAsync(existingUserRank, new UserRank
                                                                                        {
                                                                                            Id = existingUser.Id,
                                                                                            LastPostViewsCount = existingUserRank.PostViewsCount,
                                                                                            LastPostViewsRank = existingUserRank.PostViewsRank,
                                                                                            PostViewsCount = postViewsCountsMap.GetValueOrDefault(existingUser.Id),
                                                                                            PostViewsRank = 0,
                                                                                            LastParagraphViewsCount = existingUserRank.ParagraphViewsCount,
                                                                                            LastParagraphViewsRank = existingUserRank.ParagraphViewsRank,
                                                                                            ParagraphViewsCount = paraggraphViewsCountsMap.GetValueOrDefault(existingUser.Id),
                                                                                            ParagraphViewsRank = 0
                                                                                        });
                }
                else
                {
                    userRank = await UserRankRepo.CreateUserRankAsync(new UserRank
                                                                      {
                                                                          Id = existingUser.Id,
                                                                          LastPostViewsCount = 0,
                                                                          LastPostViewsRank = int.MaxValue,
                                                                          PostViewsCount = postViewsCountsMap.GetValueOrDefault(existingUser.Id),
                                                                          PostViewsRank = 0,
                                                                          LastParagraphViewsCount = 0,
                                                                          LastParagraphViewsRank = int.MaxValue,
                                                                          ParagraphViewsCount = paraggraphViewsCountsMap.GetValueOrDefault(existingUser.Id),
                                                                          ParagraphViewsRank = 0
                                                                      });
                }
                userRanks.Add(userRank);
            }
            var rankedByPostViewsCountUserRanks = userRanks.OrderByDescending(userRank => userRank.PostViewsCount).ToList();
            for (var index = 0; index < rankedByPostViewsCountUserRanks.Count; index++)
            {
                var userRank = rankedByPostViewsCountUserRanks[index];
                userRank.PostViewsRank = index + 1;
            }
            var rankedByParagraphViewsCountUserRanks = userRanks.OrderByDescending(userRank => userRank.ParagraphViewsCount).ToList();
            for (var index = 0; index < rankedByParagraphViewsCountUserRanks.Count; index++)
            {
                var userRank = rankedByParagraphViewsCountUserRanks[index];
                userRank.ParagraphViewsRank = index + 1;
            }
            foreach (var userRank in userRanks)
            {
                await UserRankRepo.UpdateUserRankAsync(userRank, userRank);
            }
            return new UserRankCountResponse();
        }

        #endregion
    }
}