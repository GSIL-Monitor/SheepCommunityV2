using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sheep.Model.Friendship.Entities;

namespace Sheep.Model.Friendship
{
    /// <summary>
    ///     关注的存储库的接口定义。
    /// </summary>
    public interface IFollowRepository
    {
        #region 获取

        /// <summary>
        ///     根据被关注者与关注者获取关注。
        /// </summary>
        /// <param name="ownerId">被关注者的用户编号。</param>
        /// <param name="followerId">关注者的用户编号。</param>
        /// <returns>关注。</returns>
        Follow GetFollow(int ownerId, int followerId);

        /// <summary>
        ///     异步根据被关注者与关注者获取关注。
        /// </summary>
        /// <param name="ownerId">被关注者的用户编号。</param>
        /// <param name="followerId">关注者的用户编号。</param>
        /// <returns>关注。</returns>
        Task<Follow> GetFollowAsync(int ownerId, int followerId);

        /// <summary>
        ///     根据被关注者查找关注。
        /// </summary>
        /// <param name="ownerId">被关注者的用户编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>关注列表。</returns>
        List<Follow> FindFollowsByOwner(int ownerId, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据被关注者查找关注。
        /// </summary>
        /// <param name="ownerId">被关注者的用户编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>关注列表。</returns>
        Task<List<Follow>> FindFollowsByOwnerAsync(int ownerId, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     根据关注者查找关注。
        /// </summary>
        /// <param name="followerId">关注者的用户编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>关注列表。</returns>
        List<Follow> FindFollowsByFollower(int followerId, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据关注者查找关注。
        /// </summary>
        /// <param name="followerId">关注者的用户编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>关注列表。</returns>
        Task<List<Follow>> FindFollowsByFollowerAsync(int followerId, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit);

        #endregion

        #region 写入

        /// <summary>
        ///     创建一个新的关注。
        /// </summary>
        /// <param name="newFollow">新的关注。</param>
        /// <returns>创建后的关注。</returns>
        Follow CreateFollow(Follow newFollow);

        /// <summary>
        ///     异步创建一个新的关注。
        /// </summary>
        /// <param name="newFollow">新的关注。</param>
        /// <returns>创建后的关注。</returns>
        Task<Follow> CreateFollowAsync(Follow newFollow);

        /// <summary>
        ///     更新一个关注。
        /// </summary>
        /// <param name="existingFollow">原有的关注。</param>
        /// <param name="newFollow">新的关注。</param>
        /// <returns>更新后的关注。</returns>
        Follow UpdateFollow(Follow existingFollow, Follow newFollow);

        /// <summary>
        ///     异步更新一个关注。
        /// </summary>
        /// <param name="existingFollow">原有的关注。</param>
        /// <param name="newFollow">新的关注。</param>
        /// <returns>更新后的关注。</returns>
        Task<Follow> UpdateFollowAsync(Follow existingFollow, Follow newFollow);

        /// <summary>
        ///     取消一个关注。
        /// </summary>
        /// <param name="ownerId">被关注者的用户编号。</param>
        /// <param name="followerId">关注者的用户编号。</param>
        void DeleteFollow(int ownerId, int followerId);

        /// <summary>
        ///     异步取消一个关注。
        /// </summary>
        /// <param name="ownerId">被关注者的用户编号。</param>
        /// <param name="followerId">关注者的用户编号。</param>
        Task DeleteFollowAsync(int ownerId, int followerId);

        #endregion
    }
}