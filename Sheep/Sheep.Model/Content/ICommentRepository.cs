using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sheep.Model.Content.Entities;

namespace Sheep.Model.Content
{
    /// <summary>
    ///     评论的存储库的接口定义。
    /// </summary>
    public interface ICommentRepository
    {
        #region 获取

        /// <summary>
        ///     根据上级与用户获取评论。
        /// </summary>
        /// <param name="commentId">评论的编号。</param>
        /// <returns>评论。</returns>
        Comment GetComment(string commentId);

        /// <summary>
        ///     异步根据上级与用户获取评论。
        /// </summary>
        /// <param name="commentId">评论的编号。</param>
        /// <returns>评论。</returns>
        Task<Comment> GetCommentAsync(string commentId);

        /// <summary>
        ///     根据上级查找评论。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>评论列表。</returns>
        List<Comment> FindCommentsByParent(string parentId, DateTime? createdSince, DateTime? modifiedSince, bool? isFeatured, string status, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据上级查找评论。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>评论列表。</returns>
        Task<List<Comment>> FindCommentsByParentAsync(string parentId, DateTime? createdSince, DateTime? modifiedSince, bool? isFeatured, string status, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     根据用户查找评论。
        /// </summary>
        /// <param name="userId">用户的用户编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>评论列表。</returns>
        List<Comment> FindCommentsByUser(int userId, DateTime? createdSince, DateTime? modifiedSince, bool? isFeatured, string status, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据用户查找评论。
        /// </summary>
        /// <param name="userId">用户的用户编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>评论列表。</returns>
        Task<List<Comment>> FindCommentsByUserAsync(int userId, DateTime? createdSince, DateTime? modifiedSince, bool? isFeatured, string status, string orderBy, bool? descending, int? skip, int? limit);

        #endregion

        #region 统计

        /// <summary>
        ///     根据上级获取评论数量。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>评论数量。</returns>
        int GetCommentsCountByParent(string parentId, DateTime? createdSince, DateTime? modifiedSince, bool? isFeatured, string status);

        /// <summary>
        ///     异步根据上级获取评论数量。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>评论数量。</returns>
        Task<int> GetCommentsCountByParentAsync(string parentId, DateTime? createdSince, DateTime? modifiedSince, bool? isFeatured, string status);

        /// <summary>
        ///     根据用户获取评论数量。
        /// </summary>
        /// <param name="userId">用户的用户编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>评论数量。</returns>
        int GetCommentsCountByUser(int userId, DateTime? createdSince, DateTime? modifiedSince, bool? isFeatured, string status);

        /// <summary>
        ///     异步根据用户获取评论数量。
        /// </summary>
        /// <param name="userId">用户的用户编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>评论数量。</returns>
        Task<int> GetCommentsCountByUserAsync(int userId, DateTime? createdSince, DateTime? modifiedSince, bool? isFeatured, string status);

        #endregion

        #region 写入

        /// <summary>
        ///     创建一个新的评论。
        /// </summary>
        /// <param name="newComment">新的评论。</param>
        /// <returns>创建后的评论。</returns>
        Comment CreateComment(Comment newComment);

        /// <summary>
        ///     异步创建一个新的评论。
        /// </summary>
        /// <param name="newComment">新的评论。</param>
        /// <returns>创建后的评论。</returns>
        Task<Comment> CreateCommentAsync(Comment newComment);

        /// <summary>
        ///     更新一个评论。
        /// </summary>
        /// <param name="existingComment">原有的评论。</param>
        /// <param name="newComment">新的评论。</param>
        /// <returns>更新后的评论。</returns>
        Comment UpdateComment(Comment existingComment, Comment newComment);

        /// <summary>
        ///     异步更新一个评论。
        /// </summary>
        /// <param name="existingComment">原有的评论。</param>
        /// <param name="newComment">新的评论。</param>
        /// <returns>更新后的评论。</returns>
        Task<Comment> UpdateCommentAsync(Comment existingComment, Comment newComment);

        /// <summary>
        ///     删除一个评论。
        /// </summary>
        /// <param name="commentId">评论的编号。</param>
        void DeleteComment(string commentId);

        /// <summary>
        ///     异步删除一个评论。
        /// </summary>
        /// <param name="commentId">评论的编号。</param>
        Task DeleteCommentAsync(string commentId);

        /// <summary>
        ///     增加一个评论的回复的次数。
        /// </summary>
        /// <param name="commentId">评论的编号。</param>
        /// <param name="count">增加的数量。</param>
        Comment IncrementCommentRepliesCount(string commentId, int count);

        /// <summary>
        ///     异步增加一个评论的回复的次数。
        /// </summary>
        /// <param name="commentId">评论的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task<Comment> IncrementCommentRepliesCountAsync(string commentId, int count);

        /// <summary>
        ///     增加一个评论的投票的次数。
        /// </summary>
        /// <param name="commentId">评论的编号。</param>
        /// <param name="count">增加的数量。</param>
        Comment IncrementCommentVotesCount(string commentId, int count);

        /// <summary>
        ///     异步增加一个评论的投票的次数。
        /// </summary>
        /// <param name="commentId">评论的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task<Comment> IncrementCommentVotesCountAsync(string commentId, int count);

        /// <summary>
        ///     增加一个评论的赞成投票的次数。
        /// </summary>
        /// <param name="commentId">评论的编号。</param>
        /// <param name="count">增加的数量。</param>
        Comment IncrementCommentYesVotesCount(string commentId, int count);

        /// <summary>
        ///     异步增加一个评论的赞成投票的次数。
        /// </summary>
        /// <param name="commentId">评论的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task<Comment> IncrementCommentYesVotesCountAsync(string commentId, int count);

        /// <summary>
        ///     增加一个评论的反对投票的次数。
        /// </summary>
        /// <param name="commentId">评论的编号。</param>
        /// <param name="count">增加的数量。</param>
        Comment IncrementCommentNoVotesCount(string commentId, int count);

        /// <summary>
        ///     异步增加一个评论的反对投票的次数。
        /// </summary>
        /// <param name="commentId">评论的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task<Comment> IncrementCommentNoVotesCountAsync(string commentId, int count);

        #endregion
    }
}