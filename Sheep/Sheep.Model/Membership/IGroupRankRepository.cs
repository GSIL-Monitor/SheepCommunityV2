using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sheep.Model.Membership.Entities;

namespace Sheep.Model.Membership
{
    /// <summary>
    ///     群组排行的存储库的接口定义。
    /// </summary>
    public interface IGroupRankRepository
    {
        #region 获取

        /// <summary>
        ///     获取群组排行。
        /// </summary>
        /// <param name="groupId">群组编号。</param>
        /// <returns>群组排行。</returns>
        GroupRank GetGroupRank(string groupId);

        /// <summary>
        ///     异步获取群组排行。
        /// </summary>
        /// <param name="groupId">群组编号。</param>
        /// <returns>群组排行。</returns>
        Task<GroupRank> GetGroupRankAsync(string groupId);

        /// <summary>
        ///     查找群组排行。
        /// </summary>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>群组排行列表。</returns>
        List<GroupRank> FindGroupRanks(DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步查找群组排行。
        /// </summary>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>群组排行列表。</returns>
        Task<List<GroupRank>> FindGroupRanksAsync(DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     根据群组编号列表查找群组排行。
        /// </summary>
        /// <param name="groupIds">群组编号列表。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>群组列表。</returns>
        List<GroupRank> FindGroupRanks(List<string> groupIds, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据群组编号列表查找群组排行。
        /// </summary>
        /// <param name="groupIds">群组编号列表。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>群组列表。</returns>
        Task<List<GroupRank>> FindGroupRanksAsync(List<string> groupIds, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit);

        #endregion

        #region 写入

        /// <summary>
        ///     创建一个新的群组排行。
        /// </summary>
        /// <param name="newGroupRank">新的群组排行。</param>
        /// <returns>创建后的群组排行。</returns>
        GroupRank CreateGroupRank(GroupRank newGroupRank);

        /// <summary>
        ///     异步创建一个新的群组排行。
        /// </summary>
        /// <param name="newGroupRank">新的群组排行。</param>
        /// <returns>创建后的群组排行。</returns>
        Task<GroupRank> CreateGroupRankAsync(GroupRank newGroupRank);

        /// <summary>
        ///     更新一个群组排行。
        /// </summary>
        /// <param name="existingGroupRank">原有的群组排行。</param>
        /// <param name="newGroupRank">新的群组排行。</param>
        /// <returns>更新后的群组排行。</returns>
        GroupRank UpdateGroupRank(GroupRank existingGroupRank, GroupRank newGroupRank);

        /// <summary>
        ///     异步更新一个群组排行。
        /// </summary>
        /// <param name="existingGroupRank">原有的群组排行。</param>
        /// <param name="newGroupRank">新的群组排行。</param>
        /// <returns>更新后的群组排行。</returns>
        Task<GroupRank> UpdateGroupRankAsync(GroupRank existingGroupRank, GroupRank newGroupRank);

        /// <summary>
        ///     删除一个群组排行。
        /// </summary>
        /// <param name="groupId">群组编号。</param>
        void DeleteGroupRank(string groupId);

        /// <summary>
        ///     异步删除一个群组排行。
        /// </summary>
        /// <param name="groupId">群组编号。</param>
        Task DeleteGroupRankAsync(string groupId);

        #endregion
    }
}