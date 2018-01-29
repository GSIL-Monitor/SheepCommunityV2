using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sheep.Model.Content.Entities;

namespace Sheep.Model.Content
{
    /// <summary>
    ///     推荐的存储库的接口定义。
    /// </summary>
    public interface IRecommendationRepository
    {
        #region 获取

        /// <summary>
        ///     根据编号获取推荐。
        /// </summary>
        /// <param name="recommendationId">推荐的编号。</param>
        /// <returns>推荐。</returns>
        Recommendation GetRecommendation(string recommendationId);

        /// <summary>
        ///     异步根据编号获取推荐。
        /// </summary>
        /// <param name="recommendationId">推荐的编号。</param>
        /// <returns>推荐。</returns>
        Task<Recommendation> GetRecommendationAsync(string recommendationId);

        /// <summary>
        ///     查找推荐。
        /// </summary>
        /// <param name="contentType">内容的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="position">位置。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>推荐列表。</returns>
        List<Recommendation> FindRecommendations(string contentType, DateTime? createdSince, DateTime? modifiedSince, int? position, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步查找推荐。
        /// </summary>
        /// <param name="contentType">内容的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="position">位置。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>推荐列表。</returns>
        Task<List<Recommendation>> FindRecommendationsAsync(string contentType, DateTime? createdSince, DateTime? modifiedSince, int? position, string orderBy, bool? descending, int? skip, int? limit);

        #endregion

        #region 统计

        /// <summary>
        ///     获取推荐数量。
        /// </summary>
        /// <param name="contentType">内容的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="position">位置。</param>
        /// <returns>推荐数量。</returns>
        int GetRecommendationsCount(string contentType, DateTime? createdSince, DateTime? modifiedSince, int? position);

        /// <summary>
        ///     异步获取推荐数量。
        /// </summary>
        /// <param name="contentType">内容的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="position">位置。</param>
        /// <returns>推荐数量。</returns>
        Task<int> GetRecommendationsCountAsync(string contentType, DateTime? createdSince, DateTime? modifiedSince, int? position);

        #endregion

        #region 写入

        /// <summary>
        ///     创建一个新的推荐。
        /// </summary>
        /// <param name="newRecommendation">新的推荐。</param>
        /// <returns>创建后的推荐。</returns>
        Recommendation CreateRecommendation(Recommendation newRecommendation);

        /// <summary>
        ///     异步创建一个新的推荐。
        /// </summary>
        /// <param name="newRecommendation">新的推荐。</param>
        /// <returns>创建后的推荐。</returns>
        Task<Recommendation> CreateRecommendationAsync(Recommendation newRecommendation);

        /// <summary>
        ///     更新一个推荐。
        /// </summary>
        /// <param name="existingRecommendation">原有的推荐。</param>
        /// <param name="newRecommendation">新的推荐。</param>
        /// <returns>更新后的推荐。</returns>
        Recommendation UpdateRecommendation(Recommendation existingRecommendation, Recommendation newRecommendation);

        /// <summary>
        ///     异步更新一个推荐。
        /// </summary>
        /// <param name="existingRecommendation">原有的推荐。</param>
        /// <param name="newRecommendation">新的推荐。</param>
        /// <returns>更新后的推荐。</returns>
        Task<Recommendation> UpdateRecommendationAsync(Recommendation existingRecommendation, Recommendation newRecommendation);

        /// <summary>
        ///     删除一个推荐。
        /// </summary>
        /// <param name="recommendationId">推荐的编号。</param>
        void DeleteRecommendation(string recommendationId);

        /// <summary>
        ///     异步删除一个推荐。
        /// </summary>
        /// <param name="recommendationId">推荐的编号。</param>
        Task DeleteRecommendationAsync(string recommendationId);

        /// <summary>
        ///     删除一组推荐。
        /// </summary>
        /// <param name="recommendationIds">推荐的编号列表。</param>
        void DeleteRecommendations(List<string> recommendationIds);

        /// <summary>
        ///     异步删除一组推荐。
        /// </summary>
        /// <param name="recommendationIds">推荐的编号列表。</param>
        Task DeleteRecommendationsAsync(List<string> recommendationIds);

        #endregion
    }
}