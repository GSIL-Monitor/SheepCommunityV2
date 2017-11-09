using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sheep.Model.Corp.Entities;

namespace Sheep.Model.Corp
{
    /// <summary>
    ///     群组的存储库的接口定义。
    /// </summary>
    public interface IGroupRepository
    {
        #region 获取

        /// <summary>
        ///     获取群组。
        /// </summary>
        /// <param name="groupId">群组编号。</param>
        /// <returns>群组。</returns>
        Group GetGroup(string groupId);

        /// <summary>
        ///     异步获取群组。
        /// </summary>
        /// <param name="groupId">群组编号。</param>
        /// <returns>群组。</returns>
        Task<Group> GetGroupAsync(string groupId);

        /// <summary>
        ///     根据名称获取群组。
        /// </summary>
        /// <param name="displayName">显示名称。</param>
        /// <returns>群组。</returns>
        Group GetGroupByDisplayName(string displayName);

        /// <summary>
        ///     异步根据名称获取群组。
        /// </summary>
        /// <param name="displayName">显示名称。</param>
        /// <returns>群组。</returns>
        Task<Group> GetGroupByDisplayNameAsync(string displayName);

        /// <summary>
        ///     根据关联的第三方群组编号获取群组。
        /// </summary>
        /// <param name="refId">关联的第三方群组编号。</param>
        /// <returns>群组。</returns>
        Group GetGroupByRefId(string refId);

        /// <summary>
        ///     异步根据关联的第三方群组编号获取群组。
        /// </summary>
        /// <param name="refId">关联的第三方群组编号。</param>
        /// <returns>群组。</returns>
        Task<Group> GetGroupByRefIdAsync(string refId);

        /// <summary>
        ///     根据名称查找群组。
        /// </summary>
        /// <param name="nameFilter"> 过滤显示名称及描述的表达式。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="joinMode">过滤加入群组的方式。</param>
        /// <param name="isPublic">过滤非群组成员是否可以访问群组内容。</param>
        /// <param name="accountStatus"> 过滤帐户状态。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>群组列表。</returns>
        List<Group> FindGroups(string nameFilter, DateTime? createdSince, DateTime? modifiedSince, string joinMode, bool? isPublic, string accountStatus, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据名称查找群组。
        /// </summary>
        /// <param name="nameFilter"> 过滤显示名称及描述的表达式。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="joinMode">过滤加入群组的方式。</param>
        /// <param name="isPublic">过滤非群组成员是否可以访问群组内容。</param>
        /// <param name="accountStatus"> 过滤帐户状态。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>群组列表。</returns>
        Task<List<Group>> FindGroupsAsync(string nameFilter, DateTime? createdSince, DateTime? modifiedSince, string joinMode, bool? isPublic, string accountStatus, string orderBy, bool? descending, int? skip, int? limit);

        #endregion

        #region 写入

        /// <summary>
        ///     创建一个新的群组。
        /// </summary>
        /// <param name="newGroup">新的群组。</param>
        /// <returns>创建后的群组。</returns>
        Group CreateGroup(Group newGroup);

        /// <summary>
        ///     异步创建一个新的群组。
        /// </summary>
        /// <param name="newGroup">新的群组。</param>
        /// <returns>创建后的群组。</returns>
        Task<Group> CreateGroupAsync(Group newGroup);

        /// <summary>
        ///     更新一个群组。
        /// </summary>
        /// <param name="existingGroup">原有的群组。</param>
        /// <param name="newGroup">新的群组。</param>
        /// <returns>更新后的群组。</returns>
        Group UpdateGroup(Group existingGroup, Group newGroup);

        /// <summary>
        ///     异步更新一个群组。
        /// </summary>
        /// <param name="existingGroup">原有的群组。</param>
        /// <param name="newGroup">新的群组。</param>
        /// <returns>更新后的群组。</returns>
        Task<Group> UpdateGroupAsync(Group existingGroup, Group newGroup);

        /// <summary>
        ///     删除一个群组。
        /// </summary>
        /// <param name="groupId">群组编号。</param>
        void DeleteGroup(string groupId);

        /// <summary>
        ///     异步删除一个群组。
        /// </summary>
        /// <param name="groupId">群组编号。</param>
        Task DeleteGroupAsync(string groupId);

        #endregion
    }
}