using System.Collections.Generic;
using System.Threading.Tasks;
using Sheep.Model.Membership.Entities;

namespace Sheep.Model.Membership
{
    /// <summary>
    ///     群主成员的存储库的接口定义。
    /// </summary>
    public interface IGroupMemberRepository
    {
        #region 获取

        /// <summary>
        ///     根据群组与成员获取群主成员。
        /// </summary>
        /// <param name="groupId">群组编号。</param>
        /// <param name="userId">成员的用户编号。</param>
        /// <returns>群主成员。</returns>
        GroupMember GetGroupMember(string groupId, int userId);

        /// <summary>
        ///     异步根据群组与成员获取群主成员。
        /// </summary>
        /// <param name="groupId">群组编号。</param>
        /// <param name="userId">成员的用户编号。</param>
        /// <returns>群主成员。</returns>
        Task<GroupMember> GetGroupMemberAsync(string groupId, int userId);

        /// <summary>
        ///     根据群组查找群主成员。
        /// </summary>
        /// <param name="groupId">群组编号。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>群主成员列表。</returns>
        List<GroupMember> FindGroupMembersByGroup(string groupId, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据群组查找群主成员。
        /// </summary>
        /// <param name="groupId">群组编号。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>群主成员列表。</returns>
        Task<List<GroupMember>> FindGroupMembersByGroupAsync(string groupId, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     根据成员查找群主成员。
        /// </summary>
        /// <param name="userId">成员的用户编号。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>群主成员列表。</returns>
        List<GroupMember> FindGroupMembersByUser(int userId, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据成员查找群主成员。
        /// </summary>
        /// <param name="userId">成员的用户编号。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>群主成员列表。</returns>
        Task<List<GroupMember>> FindGroupMembersByUserAsync(int userId, string orderBy, bool? descending, int? skip, int? limit);

        #endregion

        #region 写入

        /// <summary>
        ///     创建一个新的群主成员。
        /// </summary>
        /// <param name="newGroupMember">新的群主成员。</param>
        /// <returns>创建后的群主成员。</returns>
        GroupMember CreateGroupMember(GroupMember newGroupMember);

        /// <summary>
        ///     异步创建一个新的群主成员。
        /// </summary>
        /// <param name="newGroupMember">新的群主成员。</param>
        /// <returns>创建后的群主成员。</returns>
        Task<GroupMember> CreateGroupMemberAsync(GroupMember newGroupMember);

        /// <summary>
        ///     取消一个群主成员。
        /// </summary>
        /// <param name="groupId">群组编号。</param>
        /// <param name="userId">成员的用户编号。</param>
        void DeleteGroupMember(string groupId, int userId);

        /// <summary>
        ///     异步取消一个群主成员。
        /// </summary>
        /// <param name="groupId">群组编号。</param>
        /// <param name="userId">成员的用户编号。</param>
        Task DeleteGroupMemberAsync(string groupId, int userId);

        #endregion
    }
}