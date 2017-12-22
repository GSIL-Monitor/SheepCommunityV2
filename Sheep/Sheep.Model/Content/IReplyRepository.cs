using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sheep.Model.Content.Entities;

namespace Sheep.Model.Content
{
    /// <summary>
    ///     回复的存储库的接口定义。
    /// </summary>
    public interface IReplyRepository
    {
        #region 获取

        /// <summary>
        ///     根据上级与用户获取回复。
        /// </summary>
        /// <param name="replyId">回复的编号。</param>
        /// <returns>回复。</returns>
        Reply GetReply(string replyId);

        /// <summary>
        ///     异步根据上级与用户获取回复。
        /// </summary>
        /// <param name="replyId">回复的编号。</param>
        /// <returns>回复。</returns>
        Task<Reply> GetReplyAsync(string replyId);

        /// <summary>
        ///     根据上级查找回复。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>回复列表。</returns>
        List<Reply> FindRepliesByParent(string parentId, DateTime? createdSince, DateTime? modifiedSince, string status, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据上级查找回复。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>回复列表。</returns>
        Task<List<Reply>> FindRepliesByParentAsync(string parentId, DateTime? createdSince, DateTime? modifiedSince, string status, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     根据用户查找回复。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>回复列表。</returns>
        List<Reply> FindRepliesByUser(int userId, string parentType, DateTime? createdSince, DateTime? modifiedSince, string status, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据用户查找回复。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>回复列表。</returns>
        Task<List<Reply>> FindRepliesByUserAsync(int userId, string parentType, DateTime? createdSince, DateTime? modifiedSince, string status, string orderBy, bool? descending, int? skip, int? limit);

        #endregion

        #region 统计

        /// <summary>
        ///     根据上级获取回复数量。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>回复数量。</returns>
        int GetRepliesCountByParent(string parentId, DateTime? createdSince, DateTime? modifiedSince, string status);

        /// <summary>
        ///     异步根据上级获取回复数量。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>回复数量。</returns>
        Task<int> GetRepliesCountByParentAsync(string parentId, DateTime? createdSince, DateTime? modifiedSince, string status);

        /// <summary>
        ///     根据用户获取回复数量。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>回复数量。</returns>
        int GetRepliesCountByUser(int userId, string parentType, DateTime? createdSince, DateTime? modifiedSince, string status);

        /// <summary>
        ///     异步根据用户获取回复数量。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>回复数量。</returns>
        Task<int> GetRepliesCountByUserAsync(int userId, string parentType, DateTime? createdSince, DateTime? modifiedSince, string status);

        #endregion

        #region 写入

        /// <summary>
        ///     创建一个新的回复。
        /// </summary>
        /// <param name="newReply">新的回复。</param>
        /// <returns>创建后的回复。</returns>
        Reply CreateReply(Reply newReply);

        /// <summary>
        ///     异步创建一个新的回复。
        /// </summary>
        /// <param name="newReply">新的回复。</param>
        /// <returns>创建后的回复。</returns>
        Task<Reply> CreateReplyAsync(Reply newReply);

        /// <summary>
        ///     更新一个回复。
        /// </summary>
        /// <param name="existingReply">原有的回复。</param>
        /// <param name="newReply">新的回复。</param>
        /// <returns>更新后的回复。</returns>
        Reply UpdateReply(Reply existingReply, Reply newReply);

        /// <summary>
        ///     异步更新一个回复。
        /// </summary>
        /// <param name="existingReply">原有的回复。</param>
        /// <param name="newReply">新的回复。</param>
        /// <returns>更新后的回复。</returns>
        Task<Reply> UpdateReplyAsync(Reply existingReply, Reply newReply);

        /// <summary>
        ///     删除一个回复。
        /// </summary>
        /// <param name="replyId">回复的编号。</param>
        void DeleteReply(string replyId);

        /// <summary>
        ///     异步删除一个回复。
        /// </summary>
        /// <param name="replyId">回复的编号。</param>
        Task DeleteReplyAsync(string replyId);

        /// <summary>
        ///     增加一个回复的投票的次数。
        /// </summary>
        /// <param name="replyId">回复的编号。</param>
        /// <param name="count">增加的数量。</param>
        Reply IncrementReplyVotesCount(string replyId, int count);

        /// <summary>
        ///     异步增加一个回复的投票的次数。
        /// </summary>
        /// <param name="replyId">回复的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task<Reply> IncrementReplyVotesCountAsync(string replyId, int count);

        /// <summary>
        ///     增加一个回复的赞成投票的次数。
        /// </summary>
        /// <param name="replyId">回复的编号。</param>
        /// <param name="count">增加的数量。</param>
        Reply IncrementReplyYesVotesCount(string replyId, int count);

        /// <summary>
        ///     异步增加一个回复的赞成投票的次数。
        /// </summary>
        /// <param name="replyId">回复的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task<Reply> IncrementReplyYesVotesCountAsync(string replyId, int count);

        /// <summary>
        ///     增加一个回复的反对投票的次数。
        /// </summary>
        /// <param name="replyId">回复的编号。</param>
        /// <param name="count">增加的数量。</param>
        Reply IncrementReplyNoVotesCount(string replyId, int count);

        /// <summary>
        ///     异步增加一个回复的反对投票的次数。
        /// </summary>
        /// <param name="replyId">回复的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task<Reply> IncrementReplyNoVotesCountAsync(string replyId, int count);

        /// <summary>
        ///     增加一个回复的赞成投票的次数及总投票的次数。
        /// </summary>
        /// <param name="replyId">回复的编号。</param>
        /// <param name="count">增加的数量。</param>
        Reply IncrementReplyVotesAndYesVotesCount(string replyId, int count);

        /// <summary>
        ///     异步增加一个回复的赞成投票的次数及总投票的次数。
        /// </summary>
        /// <param name="replyId">回复的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task<Reply> IncrementReplyVotesAndYesVotesCountAsync(string replyId, int count);

        /// <summary>
        ///     增加一个回复的反对投票的次数及总投票的次数。
        /// </summary>
        /// <param name="replyId">回复的编号。</param>
        /// <param name="count">增加的数量。</param>
        Reply IncrementReplyVotesAndNoVotesCount(string replyId, int count);

        /// <summary>
        ///     异步增加一个回复的反对投票的次数及总投票的次数。
        /// </summary>
        /// <param name="replyId">回复的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task<Reply> IncrementReplyVotesAndNoVotesCountAsync(string replyId, int count);

        #endregion
    }
}