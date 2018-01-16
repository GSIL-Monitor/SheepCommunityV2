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
        ///     根据编号获取评论。
        /// </summary>
        /// <param name="commentId">评论的编号。</param>
        /// <returns>评论。</returns>
        Comment GetComment(string commentId);

        /// <summary>
        ///     异步根据编号获取评论。
        /// </summary>
        /// <param name="commentId">评论的编号。</param>
        /// <returns>评论。</returns>
        Task<Comment> GetCommentAsync(string commentId);

        /// <summary>
        ///     查找评论。
        /// </summary>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>评论列表。</returns>
        List<Comment> FindComments(string parentType, DateTime? createdSince, DateTime? modifiedSince, bool? isFeatured, string status, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步查找评论。
        /// </summary>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>评论列表。</returns>
        Task<List<Comment>> FindCommentsAsync(string parentType, DateTime? createdSince, DateTime? modifiedSince, bool? isFeatured, string status, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     根据上级查找评论。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="userId">用户的编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>评论列表。</returns>
        List<Comment> FindCommentsByParent(string parentId, int? userId, DateTime? createdSince, DateTime? modifiedSince, bool? isFeatured, string status, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据上级查找评论。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="userId">用户的编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>评论列表。</returns>
        Task<List<Comment>> FindCommentsByParentAsync(string parentId, int? userId, DateTime? createdSince, DateTime? modifiedSince, bool? isFeatured, string status, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     根据用户查找评论。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>评论列表。</returns>
        List<Comment> FindCommentsByUser(int userId, string parentType, DateTime? createdSince, DateTime? modifiedSince, bool? isFeatured, string status, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据用户查找评论。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>评论列表。</returns>
        Task<List<Comment>> FindCommentsByUserAsync(int userId, string parentType, DateTime? createdSince, DateTime? modifiedSince, bool? isFeatured, string status, string orderBy, bool? descending, int? skip, int? limit);

        #endregion

        #region 统计

        /// <summary>
        ///     获取评论数量。
        /// </summary>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>评论数量。</returns>
        int GetCommentsCount(string parentType, DateTime? createdSince, DateTime? modifiedSince, bool? isFeatured, string status);

        /// <summary>
        ///     异步获取评论数量。
        /// </summary>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>评论数量。</returns>
        Task<int> GetCommentsCountAsync(string parentType, DateTime? createdSince, DateTime? modifiedSince, bool? isFeatured, string status);

        /// <summary>
        ///     根据上级获取评论数量。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="userId">用户的编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>评论数量。</returns>
        int GetCommentsCountByParent(string parentId, int? userId, DateTime? createdSince, DateTime? modifiedSince, bool? isFeatured, string status);

        /// <summary>
        ///     异步根据上级获取评论数量。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="userId">用户的编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>评论数量。</returns>
        Task<int> GetCommentsCountByParentAsync(string parentId, int? userId, DateTime? createdSince, DateTime? modifiedSince, bool? isFeatured, string status);

        /// <summary>
        ///     根据上级列表获取评论数量列表。
        /// </summary>
        /// <param name="parentIds">上级的编号的列表。（如帖子编号）</param>
        /// <param name="userId">用户的编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>评论数量列表。</returns>
        List<KeyValuePair<string, int>> GetCommentsCountByParents(List<string> parentIds, int? userId, DateTime? createdSince, DateTime? modifiedSince, bool? isFeatured, string status);

        /// <summary>
        ///     异步根据上级列表获取评论数量列表。
        /// </summary>
        /// <param name="parentIds">上级的编号的列表。（如帖子编号）</param>
        /// <param name="userId">用户的编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>评论数量。</returns>
        Task<List<KeyValuePair<string, int>>> GetCommentsCountByParentsAsync(List<string> parentIds, int? userId, DateTime? createdSince, DateTime? modifiedSince, bool? isFeatured, string status);

        /// <summary>
        ///     根据用户获取评论数量。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>评论数量。</returns>
        int GetCommentsCountByUser(int userId, string parentType, DateTime? createdSince, DateTime? modifiedSince, bool? isFeatured, string status);

        /// <summary>
        ///     异步根据用户获取评论数量。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>评论数量。</returns>
        Task<int> GetCommentsCountByUserAsync(int userId, string parentType, DateTime? createdSince, DateTime? modifiedSince, bool? isFeatured, string status);

        #endregion

        #region 计算

        /// <summary>
        ///     计算内容的得分。
        /// </summary>
        /// <param name="comment">评论。</param>
        /// <returns>得分。</returns>
        float CalculateCommentContentScore(Comment comment);

        /// <summary>
        ///     异步计算内容的得分。
        /// </summary>
        /// <param name="comment">评论。</param>
        /// <returns>得分。</returns>
        Task<float> CalculateCommentContentScoreAsync(Comment comment);

        /// <summary>
        ///     计算精选的得分。
        /// </summary>
        /// <param name="comment">评论。</param>
        /// <returns>得分。</returns>
        float CalculateCommentFeaturedScore(Comment comment);

        /// <summary>
        ///     异步计算精选的得分。
        /// </summary>
        /// <param name="comment">评论。</param>
        /// <returns>得分。</returns>
        Task<float> CalculateCommentFeaturedScoreAsync(Comment comment);

        /// <summary>
        ///     计算回复的得分。
        /// </summary>
        /// <param name="comment">评论。</param>
        /// <returns>得分。</returns>
        float CalculateCommentRepliesScore(Comment comment);

        /// <summary>
        ///     异步计算回复的得分。
        /// </summary>
        /// <param name="comment">评论。</param>
        /// <returns>得分。</returns>
        Task<float> CalculateCommentRepliesScoreAsync(Comment comment);

        /// <summary>
        ///     计算投票的得分。
        /// </summary>
        /// <param name="comment">评论。</param>
        /// <returns>得分。</returns>
        float CalculateCommentVotesScore(Comment comment);

        /// <summary>
        ///     异步计算投票的得分。
        /// </summary>
        /// <param name="comment">评论。</param>
        /// <returns>得分。</returns>
        Task<float> CalculateCommentVotesScoreAsync(Comment comment);

        /// <summary>
        ///     计算内容质量的得分。
        /// </summary>
        /// <param name="comment">评论。</param>
        /// <param name="contentWeight">内容的权重。</param>
        /// <param name="featuredWeight">精选的权重。</param>
        /// <param name="repliesWeight">回复的权重。</param>
        /// <param name="votesWeight">投票的权重。</param>
        /// <param name="decayHalfLife">得分的半衰期。（天）</param>
        /// <returns>得分。</returns>
        float CalculateCommentContentQuality(Comment comment, float contentWeight = 1.0f, float featuredWeight = 1.0f, float repliesWeight = 1.0f, float votesWeight = 1.0f, int decayHalfLife = 180);

        /// <summary>
        ///     异步计算内容质量的得分。
        /// </summary>
        /// <param name="comment">评论。</param>
        /// <param name="contentWeight">内容的权重。</param>
        /// <param name="featuredWeight">精选的权重。</param>
        /// <param name="repliesWeight">回复的权重。</param>
        /// <param name="votesWeight">投票的权重。</param>
        /// <param name="decayHalfLife">得分的半衰期。（天）</param>
        /// <returns>得分。</returns>
        Task<float> CalculateCommentContentQualityAsync(Comment comment, float contentWeight = 1.0f, float featuredWeight = 1.0f, float repliesWeight = 1.0f, float votesWeight = 1.0f, int decayHalfLife = 180);

        /// <summary>
        ///     计算用户评论的得分。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>得分。</returns>
        float CalculateUserCommentsScore(int userId, string parentType, DateTime? createdSince, DateTime? modifiedSince, bool? isFeatured, string status);

        /// <summary>
        ///     异步计算用户评论的得分。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>得分。</returns>
        Task<float> CalculateUserCommentsScoreAsync(int userId, string parentType, DateTime? createdSince, DateTime? modifiedSince, bool? isFeatured, string status);

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
        void IncrementCommentRepliesCount(string commentId, int count);

        /// <summary>
        ///     异步增加一个评论的回复的次数。
        /// </summary>
        /// <param name="commentId">评论的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task IncrementCommentRepliesCountAsync(string commentId, int count);

        /// <summary>
        ///     增加一个评论的投票的次数。
        /// </summary>
        /// <param name="commentId">评论的编号。</param>
        /// <param name="count">增加的数量。</param>
        void IncrementCommentVotesCount(string commentId, int count);

        /// <summary>
        ///     异步增加一个评论的投票的次数。
        /// </summary>
        /// <param name="commentId">评论的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task IncrementCommentVotesCountAsync(string commentId, int count);

        /// <summary>
        ///     增加一个评论的赞成投票的次数。
        /// </summary>
        /// <param name="commentId">评论的编号。</param>
        /// <param name="count">增加的数量。</param>
        void IncrementCommentYesVotesCount(string commentId, int count);

        /// <summary>
        ///     异步增加一个评论的赞成投票的次数。
        /// </summary>
        /// <param name="commentId">评论的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task IncrementCommentYesVotesCountAsync(string commentId, int count);

        /// <summary>
        ///     增加一个评论的反对投票的次数。
        /// </summary>
        /// <param name="commentId">评论的编号。</param>
        /// <param name="count">增加的数量。</param>
        void IncrementCommentNoVotesCount(string commentId, int count);

        /// <summary>
        ///     异步增加一个评论的反对投票的次数。
        /// </summary>
        /// <param name="commentId">评论的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task IncrementCommentNoVotesCountAsync(string commentId, int count);

        /// <summary>
        ///     增加一个评论的赞成投票的次数及总投票的次数。
        /// </summary>
        /// <param name="commentId">评论的编号。</param>
        /// <param name="count">增加的数量。</param>
        void IncrementCommentVotesAndYesVotesCount(string commentId, int count);

        /// <summary>
        ///     异步增加一个评论的赞成投票的次数及总投票的次数。
        /// </summary>
        /// <param name="commentId">评论的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task IncrementCommentVotesAndYesVotesCountAsync(string commentId, int count);

        /// <summary>
        ///     增加一个评论的反对投票的次数及总投票的次数。
        /// </summary>
        /// <param name="commentId">评论的编号。</param>
        /// <param name="count">增加的数量。</param>
        void IncrementCommentVotesAndNoVotesCount(string commentId, int count);

        /// <summary>
        ///     异步增加一个评论的反对投票的次数及总投票的次数。
        /// </summary>
        /// <param name="commentId">评论的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task IncrementCommentVotesAndNoVotesCountAsync(string commentId, int count);

        /// <summary>
        ///     更新一个评论的内容质量的评分。
        /// </summary>
        /// <param name="commentId">评论的编号。</param>
        /// <param name="value">更新的数值。</param>
        void UpdateCommentContentQuality(string commentId, float value);

        /// <summary>
        ///     异步更新一个评论的内容质量的评分。
        /// </summary>
        /// <param name="commentId">评论的编号。</param>
        /// <param name="value">更新的数值。</param>
        Task UpdateCommentContentQualityAsync(string commentId, float value);

        #endregion
    }
}