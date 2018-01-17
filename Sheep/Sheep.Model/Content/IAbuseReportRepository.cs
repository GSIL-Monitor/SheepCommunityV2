using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sheep.Model.Content.Entities;

namespace Sheep.Model.Content
{
    /// <summary>
    ///     举报的存储库的接口定义。
    /// </summary>
    public interface IAbuseReportRepository
    {
        #region 获取

        /// <summary>
        ///     根据编号获取举报。
        /// </summary>
        /// <param name="reportId">举报的编号。</param>
        /// <returns>举报。</returns>
        AbuseReport GetAbuseReport(string reportId);

        /// <summary>
        ///     异步根据编号获取举报。
        /// </summary>
        /// <param name="reportId">举报的编号。</param>
        /// <returns>举报。</returns>
        Task<AbuseReport> GetAbuseReportAsync(string reportId);

        /// <summary>
        ///     查找举报。
        /// </summary>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>举报列表。</returns>
        List<AbuseReport> FindAbuseReports(string parentType, DateTime? createdSince, DateTime? modifiedSince, string status, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步查找举报。
        /// </summary>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>举报列表。</returns>
        Task<List<AbuseReport>> FindAbuseReportsAsync(string parentType, DateTime? createdSince, DateTime? modifiedSince, string status, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     根据上级查找举报。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="userId">用户的编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>举报列表。</returns>
        List<AbuseReport> FindAbuseReportsByParent(string parentId, int? userId, DateTime? createdSince, DateTime? modifiedSince, string status, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据上级查找举报。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="userId">用户的编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>举报列表。</returns>
        Task<List<AbuseReport>> FindAbuseReportsByParentAsync(string parentId, int? userId, DateTime? createdSince, DateTime? modifiedSince, string status, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     根据用户查找举报。
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
        /// <returns>举报列表。</returns>
        List<AbuseReport> FindAbuseReportsByUser(int userId, string parentType, DateTime? createdSince, DateTime? modifiedSince, string status, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据用户查找举报。
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
        /// <returns>举报列表。</returns>
        Task<List<AbuseReport>> FindAbuseReportsByUserAsync(int userId, string parentType, DateTime? createdSince, DateTime? modifiedSince, string status, string orderBy, bool? descending, int? skip, int? limit);

        #endregion

        #region 统计

        /// <summary>
        ///     获取举报数量。
        /// </summary>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>举报数量。</returns>
        int GetAbuseReportsCount(string parentType, DateTime? createdSince, DateTime? modifiedSince, string status);

        /// <summary>
        ///     异步获取举报数量。
        /// </summary>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>举报数量。</returns>
        Task<int> GetAbuseReportsCountAsync(string parentType, DateTime? createdSince, DateTime? modifiedSince, string status);

        /// <summary>
        ///     根据上级获取举报数量。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="userId">用户的编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>举报数量。</returns>
        int GetAbuseReportsCountByParent(string parentId, int? userId, DateTime? createdSince, DateTime? modifiedSince, string status);

        /// <summary>
        ///     异步根据上级获取举报数量。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="userId">用户的编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>举报数量。</returns>
        Task<int> GetAbuseReportsCountByParentAsync(string parentId, int? userId, DateTime? createdSince, DateTime? modifiedSince, string status);

        /// <summary>
        ///     根据上级列表获取举报数量列表。
        /// </summary>
        /// <param name="parentIds">上级的编号的列表。（如帖子编号）</param>
        /// <param name="userId">用户的编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>举报数量列表。</returns>
        List<KeyValuePair<string, int>> GetAbuseReportsCountByParents(List<string> parentIds, int? userId, DateTime? createdSince, DateTime? modifiedSince, string status);

        /// <summary>
        ///     异步根据上级列表获取举报数量列表。
        /// </summary>
        /// <param name="parentIds">上级的编号的列表。（如帖子编号）</param>
        /// <param name="userId">用户的编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>举报数量。</returns>
        Task<List<KeyValuePair<string, int>>> GetAbuseReportsCountByParentsAsync(List<string> parentIds, int? userId, DateTime? createdSince, DateTime? modifiedSince, string status);

        /// <summary>
        ///     根据用户获取举报数量。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>举报数量。</returns>
        int GetAbuseReportsCountByUser(int userId, string parentType, DateTime? createdSince, DateTime? modifiedSince, string status);

        /// <summary>
        ///     异步根据用户获取举报数量。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>举报数量。</returns>
        Task<int> GetAbuseReportsCountByUserAsync(int userId, string parentType, DateTime? createdSince, DateTime? modifiedSince, string status);

        #endregion

        #region 计算

        /// <summary>
        ///     计算用户举报的得分。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>得分。</returns>
        float CalculateUserAbuseReportsScore(int userId, string parentType, DateTime? createdSince, DateTime? modifiedSince, string status);

        /// <summary>
        ///     异步计算用户举报的得分。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>得分。</returns>
        Task<float> CalculateUserAbuseReportsScoreAsync(int userId, string parentType, DateTime? createdSince, DateTime? modifiedSince, string status);

        #endregion

        #region 写入

        /// <summary>
        ///     创建一个新的举报。
        /// </summary>
        /// <param name="newAbuseReport">新的举报。</param>
        /// <returns>创建后的举报。</returns>
        AbuseReport CreateAbuseReport(AbuseReport newAbuseReport);

        /// <summary>
        ///     异步创建一个新的举报。
        /// </summary>
        /// <param name="newAbuseReport">新的举报。</param>
        /// <returns>创建后的举报。</returns>
        Task<AbuseReport> CreateAbuseReportAsync(AbuseReport newAbuseReport);

        /// <summary>
        ///     更新一个举报。
        /// </summary>
        /// <param name="existingAbuseReport">原有的举报。</param>
        /// <param name="newAbuseReport">新的举报。</param>
        /// <returns>更新后的举报。</returns>
        AbuseReport UpdateAbuseReport(AbuseReport existingAbuseReport, AbuseReport newAbuseReport);

        /// <summary>
        ///     异步更新一个举报。
        /// </summary>
        /// <param name="existingAbuseReport">原有的举报。</param>
        /// <param name="newAbuseReport">新的举报。</param>
        /// <returns>更新后的举报。</returns>
        Task<AbuseReport> UpdateAbuseReportAsync(AbuseReport existingAbuseReport, AbuseReport newAbuseReport);

        /// <summary>
        ///     删除一个举报。
        /// </summary>
        /// <param name="reportId">举报的编号。</param>
        void DeleteAbuseReport(string reportId);

        /// <summary>
        ///     异步删除一个举报。
        /// </summary>
        /// <param name="reportId">举报的编号。</param>
        Task DeleteAbuseReportAsync(string reportId);

        #endregion
    }
}