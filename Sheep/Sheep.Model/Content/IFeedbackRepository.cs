using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sheep.Model.Content.Entities;

namespace Sheep.Model.Content
{
    /// <summary>
    ///     反馈的存储库的接口定义。
    /// </summary>
    public interface IFeedbackRepository
    {
        #region 获取

        /// <summary>
        ///     根据编号获取反馈。
        /// </summary>
        /// <param name="feedbackId">反馈的编号。</param>
        /// <returns>反馈。</returns>
        Feedback GetFeedback(string feedbackId);

        /// <summary>
        ///     异步根据编号获取反馈。
        /// </summary>
        /// <param name="feedbackId">反馈的编号。</param>
        /// <returns>反馈。</returns>
        Task<Feedback> GetFeedbackAsync(string feedbackId);

        /// <summary>
        ///     查找反馈。
        /// </summary>
        /// <param name="contentFilter">过滤内容的表达式。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>反馈列表。</returns>
        List<Feedback> FindFeedbacks(string contentFilter, DateTime? createdSince, DateTime? modifiedSince, string status, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步查找反馈。
        /// </summary>
        /// <param name="contentFilter">过滤内容的表达式。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>反馈列表。</returns>
        Task<List<Feedback>> FindFeedbacksAsync(string contentFilter, DateTime? createdSince, DateTime? modifiedSince, string status, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     根据用户查找反馈。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>反馈列表。</returns>
        List<Feedback> FindFeedbacksByUser(int userId, DateTime? createdSince, DateTime? modifiedSince, string status, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据用户查找反馈。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>反馈列表。</returns>
        Task<List<Feedback>> FindFeedbacksByUserAsync(int userId, DateTime? createdSince, DateTime? modifiedSince, string status, string orderBy, bool? descending, int? skip, int? limit);

        #endregion

        #region 统计

        /// <summary>
        ///     获取反馈数量。
        /// </summary>
        /// <param name="contentFilter">过滤内容的表达式。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>反馈数量。</returns>
        int GetFeedbacksCount(string contentFilter, DateTime? createdSince, DateTime? modifiedSince, string status);

        /// <summary>
        ///     异步获取反馈数量。
        /// </summary>
        /// <param name="contentFilter">过滤内容的表达式。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>反馈数量。</returns>
        Task<int> GetFeedbacksCountAsync(string contentFilter, DateTime? createdSince, DateTime? modifiedSince, string status);

        /// <summary>
        ///     根据用户获取反馈数量。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>反馈数量。</returns>
        int GetFeedbacksCountByUser(int userId, DateTime? createdSince, DateTime? modifiedSince, string status);

        /// <summary>
        ///     异步根据用户获取反馈数量。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>反馈数量。</returns>
        Task<int> GetFeedbacksCountByUserAsync(int userId, DateTime? createdSince, DateTime? modifiedSince, string status);

        #endregion

        #region 计算

        /// <summary>
        ///     计算用户反馈的得分。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>得分。</returns>
        float CalculateUserFeedbacksScore(int userId, DateTime? createdSince, DateTime? modifiedSince, string status);

        /// <summary>
        ///     异步计算用户反馈的得分。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>得分。</returns>
        Task<float> CalculateUserFeedbacksScoreAsync(int userId, DateTime? createdSince, DateTime? modifiedSince, string status);

        #endregion

        #region 写入

        /// <summary>
        ///     创建一个新的反馈。
        /// </summary>
        /// <param name="newFeedback">新的反馈。</param>
        /// <returns>创建后的反馈。</returns>
        Feedback CreateFeedback(Feedback newFeedback);

        /// <summary>
        ///     异步创建一个新的反馈。
        /// </summary>
        /// <param name="newFeedback">新的反馈。</param>
        /// <returns>创建后的反馈。</returns>
        Task<Feedback> CreateFeedbackAsync(Feedback newFeedback);

        /// <summary>
        ///     更新一个反馈。
        /// </summary>
        /// <param name="existingFeedback">原有的反馈。</param>
        /// <param name="newFeedback">新的反馈。</param>
        /// <returns>更新后的反馈。</returns>
        Feedback UpdateFeedback(Feedback existingFeedback, Feedback newFeedback);

        /// <summary>
        ///     异步更新一个反馈。
        /// </summary>
        /// <param name="existingFeedback">原有的反馈。</param>
        /// <param name="newFeedback">新的反馈。</param>
        /// <returns>更新后的反馈。</returns>
        Task<Feedback> UpdateFeedbackAsync(Feedback existingFeedback, Feedback newFeedback);

        /// <summary>
        ///     删除一个反馈。
        /// </summary>
        /// <param name="feedbackId">反馈的编号。</param>
        void DeleteFeedback(string feedbackId);

        /// <summary>
        ///     异步删除一个反馈。
        /// </summary>
        /// <param name="feedbackId">反馈的编号。</param>
        Task DeleteFeedbackAsync(string feedbackId);

        #endregion
    }
}