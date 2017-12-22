using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sheep.Model.Content.Entities;

namespace Sheep.Model.Content
{
    /// <summary>
    ///     查看的存储库的接口定义。
    /// </summary>
    public interface IViewRepository
    {
        #region 获取

        /// <summary>
        ///     根据上级查找查看。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="userId">用户的编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>查看列表。</returns>
        List<View> FindViewsByParent(string parentId, int? userId, DateTime? createdSince, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据上级查找查看。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="userId">用户的编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>查看列表。</returns>
        Task<List<View>> FindViewsByParentAsync(string parentId, int? userId, DateTime? createdSince, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     根据用户查找查看。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>查看列表。</returns>
        List<View> FindViewsByUser(int userId, string parentType, DateTime? createdSince, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据用户查找查看。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>查看列表。</returns>
        Task<List<View>> FindViewsByUserAsync(int userId, string parentType, DateTime? createdSince, string orderBy, bool? descending, int? skip, int? limit);

        #endregion

        #region 统计

        /// <summary>
        ///     根据上级获取查看数量。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="userId">用户的编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <returns>查看数量。</returns>
        int GetViewsCountByParent(string parentId, int? userId, DateTime? createdSince);

        /// <summary>
        ///     异步根据上级获取查看数量。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="userId">用户的编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <returns>查看数量。</returns>
        Task<int> GetViewsCountByParentAsync(string parentId, int? userId, DateTime? createdSince);

        /// <summary>
        ///     根据用户获取查看数量。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <returns>查看数量。</returns>
        int GetViewsCountByUser(int userId, string parentType, DateTime? createdSince);

        /// <summary>
        ///     异步根据用户获取查看数量。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <returns>查看数量。</returns>
        Task<int> GetViewsCountByUserAsync(int userId, string parentType, DateTime? createdSince);

        /// <summary>
        ///     根据用户获取上级数量。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <returns>上级数量。</returns>
        int GetParentsCountByUser(int userId, string parentType, DateTime? createdSince);

        /// <summary>
        ///     异步根据用户获取上级数量。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <returns>上级数量。</returns>
        Task<int> GetParentsCountByUserAsync(int userId, string parentType, DateTime? createdSince);

        /// <summary>
        ///     根据用户获取天数量。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="createdSince">过滤创建天在指定的时间之后。</param>
        /// <returns>天数量。</returns>
        int GetDaysCountByUser(int userId, string parentType, DateTime? createdSince);

        /// <summary>
        ///     异步根据用户获取天数量。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="createdSince">过滤创建天在指定的时间之后。</param>
        /// <returns>天数量。</returns>
        Task<int> GetDaysCountByUserAsync(int userId, string parentType, DateTime? createdSince);

        #endregion

        #region 写入

        /// <summary>
        ///     创建一个新的查看。
        /// </summary>
        /// <param name="newView">新的查看。</param>
        /// <returns>创建后的查看。</returns>
        View CreateView(View newView);

        /// <summary>
        ///     异步创建一个新的查看。
        /// </summary>
        /// <param name="newView">新的查看。</param>
        /// <returns>创建后的查看。</returns>
        Task<View> CreateViewAsync(View newView);

        /// <summary>
        ///     创建一组新的查看。
        /// </summary>
        /// <param name="newViews">一组新的查看。</param>
        void CreateViews(List<View> newViews);

        /// <summary>
        ///     异步创建一组新的查看。
        /// </summary>
        /// <param name="newViews">一组新的查看。</param>
        Task CreateViewsAsync(List<View> newViews);

        /// <summary>
        ///     取消一个查看。
        /// </summary>
        /// <param name="viewId">查看的编号。</param>
        void DeleteView(string viewId);

        /// <summary>
        ///     异步取消一个查看。
        /// </summary>
        /// <param name="viewId">查看的编号。</param>
        Task DeleteViewAsync(string viewId);

        #endregion
    }
}