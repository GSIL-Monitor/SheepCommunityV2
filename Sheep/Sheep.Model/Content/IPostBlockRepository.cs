using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sheep.Model.Content.Entities;

namespace Sheep.Model.Content
{
    /// <summary>
    ///     帖子屏蔽的存储库的接口定义。
    /// </summary>
    public interface IPostBlockRepository
    {
        #region 获取

        /// <summary>
        ///     根据编号获取帖子屏蔽。
        /// </summary>
        /// <param name="blockId">帖子屏蔽的编号。</param>
        /// <returns>帖子屏蔽。</returns>
        PostBlock GetPostBlock(string blockId);

        /// <summary>
        ///     异步根据编号获取帖子屏蔽。
        /// </summary>
        /// <param name="blockId">帖子屏蔽的编号。</param>
        /// <returns>帖子屏蔽。</returns>
        Task<PostBlock> GetPostBlockAsync(string blockId);

        /// <summary>
        ///     根据帖子与屏蔽者获取屏蔽。
        /// </summary>
        /// <param name="postId">帖子的编号。</param>
        /// <param name="blockerId">屏蔽者的屏蔽者编号。</param>
        /// <returns>屏蔽。</returns>
        PostBlock GetPostBlock(string postId, int blockerId);

        /// <summary>
        ///     异步根据帖子与屏蔽者获取屏蔽。
        /// </summary>
        /// <param name="postId">帖子的编号。</param>
        /// <param name="blockerId">屏蔽者的屏蔽者编号。</param>
        /// <returns>屏蔽。</returns>
        Task<PostBlock> GetPostBlockAsync(string postId, int blockerId);

        /// <summary>
        ///     查找帖子屏蔽。
        /// </summary>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>帖子屏蔽列表。</returns>
        List<PostBlock> FindPostBlocks(DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步查找帖子屏蔽。
        /// </summary>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>帖子屏蔽列表。</returns>
        Task<List<PostBlock>> FindPostBlocksAsync(DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     根据上级查找帖子屏蔽。
        /// </summary>
        /// <param name="postId">帖子的编号。</param>
        /// <param name="blockerId">屏蔽者的用户编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>帖子屏蔽列表。</returns>
        List<PostBlock> FindPostBlocksByPost(string postId, int? blockerId, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据上级查找帖子屏蔽。
        /// </summary>
        /// <param name="postId">帖子的编号。</param>
        /// <param name="blockerId">屏蔽者的用户编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>帖子屏蔽列表。</returns>
        Task<List<PostBlock>> FindPostBlocksByPostAsync(string postId, int? blockerId, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     根据屏蔽者查找帖子屏蔽。
        /// </summary>
        /// <param name="blockerId">屏蔽者的用户编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>帖子屏蔽列表。</returns>
        List<PostBlock> FindPostBlocksByBlocker(int blockerId, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据屏蔽者查找帖子屏蔽。
        /// </summary>
        /// <param name="blockerId">屏蔽者的用户编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>帖子屏蔽列表。</returns>
        Task<List<PostBlock>> FindPostBlocksByBlockerAsync(int blockerId, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit);

        #endregion

        #region 统计

        /// <summary>
        ///     获取帖子屏蔽数量。
        /// </summary>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <returns>帖子屏蔽数量。</returns>
        int GetPostBlocksCount(DateTime? createdSince, DateTime? modifiedSince);

        /// <summary>
        ///     异步获取帖子屏蔽数量。
        /// </summary>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <returns>帖子屏蔽数量。</returns>
        Task<int> GetPostBlocksCountAsync(DateTime? createdSince, DateTime? modifiedSince);

        /// <summary>
        ///     根据上级获取帖子屏蔽数量。
        /// </summary>
        /// <param name="postId">帖子的编号。</param>
        /// <param name="blockerId">屏蔽者的用户编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <returns>帖子屏蔽数量。</returns>
        int GetPostBlocksCountByPost(string postId, int? blockerId, DateTime? createdSince, DateTime? modifiedSince);

        /// <summary>
        ///     异步根据上级获取帖子屏蔽数量。
        /// </summary>
        /// <param name="postId">帖子的编号。</param>
        /// <param name="blockerId">屏蔽者的用户编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <returns>帖子屏蔽数量。</returns>
        Task<int> GetPostBlocksCountByPostAsync(string postId, int? blockerId, DateTime? createdSince, DateTime? modifiedSince);

        /// <summary>
        ///     根据上级列表获取帖子屏蔽数量列表。
        /// </summary>
        /// <param name="postIds">帖子的编号的列表。</param>
        /// <param name="blockerId">屏蔽者的用户编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <returns>帖子屏蔽数量列表。</returns>
        List<KeyValuePair<string, int>> GetPostBlocksCountByPosts(List<string> postIds, int? blockerId, DateTime? createdSince, DateTime? modifiedSince);

        /// <summary>
        ///     异步根据上级列表获取帖子屏蔽数量列表。
        /// </summary>
        /// <param name="postIds">帖子的编号的列表。</param>
        /// <param name="blockerId">屏蔽者的用户编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <returns>帖子屏蔽数量。</returns>
        Task<List<KeyValuePair<string, int>>> GetPostBlocksCountByPostsAsync(List<string> postIds, int? blockerId, DateTime? createdSince, DateTime? modifiedSince);

        /// <summary>
        ///     根据屏蔽者获取帖子屏蔽数量。
        /// </summary>
        /// <param name="blockerId">屏蔽者的用户编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <returns>帖子屏蔽数量。</returns>
        int GetPostBlocksCountByBlocker(int blockerId, DateTime? createdSince, DateTime? modifiedSince);

        /// <summary>
        ///     异步根据屏蔽者获取帖子屏蔽数量。
        /// </summary>
        /// <param name="blockerId">屏蔽者的用户编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <returns>帖子屏蔽数量。</returns>
        Task<int> GetPostBlocksCountByBlockerAsync(int blockerId, DateTime? createdSince, DateTime? modifiedSince);

        #endregion

        #region 计算

        /// <summary>
        ///     计算屏蔽者帖子屏蔽的得分。
        /// </summary>
        /// <param name="blockerId">屏蔽者的用户编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <returns>得分。</returns>
        float CalculateBlockerPostBlocksScore(int blockerId, DateTime? createdSince, DateTime? modifiedSince);

        /// <summary>
        ///     异步计算屏蔽者帖子屏蔽的得分。
        /// </summary>
        /// <param name="blockerId">屏蔽者的用户编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <returns>得分。</returns>
        Task<float> CalculateBlockerPostBlocksScoreAsync(int blockerId, DateTime? createdSince, DateTime? modifiedSince);

        #endregion

        #region 写入

        /// <summary>
        ///     创建一个新的帖子屏蔽。
        /// </summary>
        /// <param name="newPostBlock">新的帖子屏蔽。</param>
        /// <returns>创建后的帖子屏蔽。</returns>
        PostBlock CreatePostBlock(PostBlock newPostBlock);

        /// <summary>
        ///     异步创建一个新的帖子屏蔽。
        /// </summary>
        /// <param name="newPostBlock">新的帖子屏蔽。</param>
        /// <returns>创建后的帖子屏蔽。</returns>
        Task<PostBlock> CreatePostBlockAsync(PostBlock newPostBlock);

        /// <summary>
        ///     更新一个帖子屏蔽。
        /// </summary>
        /// <param name="existingPostBlock">原有的帖子屏蔽。</param>
        /// <param name="newPostBlock">新的帖子屏蔽。</param>
        /// <returns>更新后的帖子屏蔽。</returns>
        PostBlock UpdatePostBlock(PostBlock existingPostBlock, PostBlock newPostBlock);

        /// <summary>
        ///     异步更新一个帖子屏蔽。
        /// </summary>
        /// <param name="existingPostBlock">原有的帖子屏蔽。</param>
        /// <param name="newPostBlock">新的帖子屏蔽。</param>
        /// <returns>更新后的帖子屏蔽。</returns>
        Task<PostBlock> UpdatePostBlockAsync(PostBlock existingPostBlock, PostBlock newPostBlock);

        /// <summary>
        ///     删除一个帖子屏蔽。
        /// </summary>
        /// <param name="blockId">帖子屏蔽的编号。</param>
        void DeletePostBlock(string blockId);

        /// <summary>
        ///     异步删除一个帖子屏蔽。
        /// </summary>
        /// <param name="blockId">帖子屏蔽的编号。</param>
        Task DeletePostBlockAsync(string blockId);

        #endregion
    }
}