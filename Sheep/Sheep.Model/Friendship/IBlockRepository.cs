using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sheep.Model.Friendship.Entities;

namespace Sheep.Model.Friendship
{
    /// <summary>
    ///     屏蔽的存储库的接口定义。
    /// </summary>
    public interface IBlockRepository
    {
        #region 获取

        /// <summary>
        ///     根据被屏蔽者与屏蔽者获取屏蔽。
        /// </summary>
        /// <param name="blockeeId">被屏蔽者的用户编号。</param>
        /// <param name="blockerId">屏蔽者的用户编号。</param>
        /// <returns>屏蔽。</returns>
        Block GetBlock(int blockeeId, int blockerId);

        /// <summary>
        ///     异步根据被屏蔽者与屏蔽者获取屏蔽。
        /// </summary>
        /// <param name="blockeeId">被屏蔽者的用户编号。</param>
        /// <param name="blockerId">屏蔽者的用户编号。</param>
        /// <returns>屏蔽。</returns>
        Task<Block> GetBlockAsync(int blockeeId, int blockerId);

        /// <summary>
        ///     根据被屏蔽者查找屏蔽。
        /// </summary>
        /// <param name="blockeeId">被屏蔽者的用户编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>屏蔽列表。</returns>
        List<Block> FindBlocksByBlockee(int blockeeId, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据被屏蔽者查找屏蔽。
        /// </summary>
        /// <param name="blockeeId">被屏蔽者的用户编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>屏蔽列表。</returns>
        Task<List<Block>> FindBlocksByBlockeeAsync(int blockeeId, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     根据屏蔽者查找屏蔽。
        /// </summary>
        /// <param name="blockerId">屏蔽者的用户编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>屏蔽列表。</returns>
        List<Block> FindBlocksByBlocker(int blockerId, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据屏蔽者查找屏蔽。
        /// </summary>
        /// <param name="blockerId">屏蔽者的用户编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>屏蔽列表。</returns>
        Task<List<Block>> FindBlocksByBlockerAsync(int blockerId, DateTime? createdSince, DateTime? modifiedSince, string orderBy, bool? descending, int? skip, int? limit);

        #endregion

        #region 写入

        /// <summary>
        ///     创建一个新的屏蔽。
        /// </summary>
        /// <param name="newBlock">新的屏蔽。</param>
        /// <returns>创建后的屏蔽。</returns>
        Block CreateBlock(Block newBlock);

        /// <summary>
        ///     异步创建一个新的屏蔽。
        /// </summary>
        /// <param name="newBlock">新的屏蔽。</param>
        /// <returns>创建后的屏蔽。</returns>
        Task<Block> CreateBlockAsync(Block newBlock);

        /// <summary>
        ///     更新一个屏蔽。
        /// </summary>
        /// <param name="existingBlock">原有的屏蔽。</param>
        /// <param name="newBlock">新的屏蔽。</param>
        /// <returns>更新后的屏蔽。</returns>
        Block UpdateBlock(Block existingBlock, Block newBlock);

        /// <summary>
        ///     异步更新一个屏蔽。
        /// </summary>
        /// <param name="existingBlock">原有的屏蔽。</param>
        /// <param name="newBlock">新的屏蔽。</param>
        /// <returns>更新后的屏蔽。</returns>
        Task<Block> UpdateBlockAsync(Block existingBlock, Block newBlock);

        /// <summary>
        ///     取消一个屏蔽。
        /// </summary>
        /// <param name="blockeeId">被屏蔽者的用户编号。</param>
        /// <param name="blockerId">屏蔽者的用户编号。</param>
        void DeleteBlock(int blockeeId, int blockerId);

        /// <summary>
        ///     异步取消一个屏蔽。
        /// </summary>
        /// <param name="blockeeId">被屏蔽者的用户编号。</param>
        /// <param name="blockerId">屏蔽者的用户编号。</param>
        Task DeleteBlockAsync(int blockeeId, int blockerId);

        #endregion
    }
}