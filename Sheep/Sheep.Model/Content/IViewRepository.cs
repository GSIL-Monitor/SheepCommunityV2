﻿using System;
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
        ///     根据编号获取查看。
        /// </summary>
        /// <param name="viewId">查看的编号。</param>
        /// <returns>查看。</returns>
        View GetView(string viewId);

        /// <summary>
        ///     异步根据编号获取查看。
        /// </summary>
        /// <param name="viewId">查看的编号。</param>
        /// <returns>查看。</returns>
        Task<View> GetViewAsync(string viewId);

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
        /// <param name="parentIdPrefix">上级的编号的前缀。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>查看列表。</returns>
        List<View> FindViewsByUser(int userId, string parentType, string parentIdPrefix, DateTime? createdSince, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据用户查找查看。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="parentIdPrefix">上级的编号的前缀。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>查看列表。</returns>
        Task<List<View>> FindViewsByUserAsync(int userId, string parentType, string parentIdPrefix, DateTime? createdSince, string orderBy, bool? descending, int? skip, int? limit);

        #endregion

        #region 统计

        /// <summary>
        ///     根据上级获取查看次数。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="userId">用户的编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <returns>查看次数。</returns>
        int GetViewsCountByParent(string parentId, int? userId, DateTime? createdSince);

        /// <summary>
        ///     异步根据上级获取查看次数。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="userId">用户的编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <returns>查看次数。</returns>
        Task<int> GetViewsCountByParentAsync(string parentId, int? userId, DateTime? createdSince);

        /// <summary>
        ///     根据上级获取查看次数。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="userId">用户的编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <returns>天数。</returns>
        int GetDaysCountByParent(string parentId, int? userId, DateTime? createdSince);

        /// <summary>
        ///     异步根据上级获取查看次数。
        /// </summary>
        /// <param name="parentId">上级的编号。（如帖子编号）</param>
        /// <param name="userId">用户的编号。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <returns>天数。</returns>
        Task<int> GetDaysCountByParentAsync(string parentId, int? userId, DateTime? createdSince);

        /// <summary>
        ///     根据用户获取查看次数。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="parentIdPrefix">上级的编号的前缀。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <returns>查看次数。</returns>
        int GetViewsCountByUser(int userId, string parentType, string parentIdPrefix, DateTime? createdSince);

        /// <summary>
        ///     异步根据用户获取查看次数。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="parentIdPrefix">上级的编号的前缀。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <returns>查看次数。</returns>
        Task<int> GetViewsCountByUserAsync(int userId, string parentType, string parentIdPrefix, DateTime? createdSince);

        /// <summary>
        ///     根据用户获取上级数量。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="parentIdPrefix">上级的编号的前缀。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <returns>上级数量。</returns>
        int GetParentsCountByUser(int userId, string parentType, string parentIdPrefix, DateTime? createdSince);

        /// <summary>
        ///     异步根据用户获取上级数量。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="parentIdPrefix">上级的编号的前缀。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <returns>上级数量。</returns>
        Task<int> GetParentsCountByUserAsync(int userId, string parentType, string parentIdPrefix, DateTime? createdSince);

        /// <summary>
        ///     根据用户获取天数。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="parentIdPrefix">上级的编号的前缀。</param>
        /// <param name="createdSince">过滤创建天在指定的时间之后。</param>
        /// <returns>天数。</returns>
        int GetDaysCountByUser(int userId, string parentType, string parentIdPrefix, DateTime? createdSince);

        /// <summary>
        ///     异步根据用户获取天数。
        /// </summary>
        /// <param name="userId">用户的编号。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="parentIdPrefix">上级的编号的前缀。</param>
        /// <param name="createdSince">过滤创建天在指定的时间之后。</param>
        /// <returns>天数。</returns>
        Task<int> GetDaysCountByUserAsync(int userId, string parentType, string parentIdPrefix, DateTime? createdSince);

        /// <summary>
        ///     根据用户列表获取查看次数。
        /// </summary>
        /// <param name="userIds">用户的编号列表。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="parentIdPrefix">上级的编号的前缀。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <returns>查看次数。</returns>
        List<KeyValuePair<int, int>> GetViewsCountByUsers(List<int> userIds, string parentType, string parentIdPrefix, DateTime? createdSince);

        /// <summary>
        ///     异步根据用户列表获取查看次数。
        /// </summary>
        /// <param name="userIds">用户的编号列表。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="parentIdPrefix">上级的编号的前缀。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <returns>查看次数。</returns>
        Task<List<KeyValuePair<int, int>>> GetViewsCountByUsersAsync(List<int> userIds, string parentType, string parentIdPrefix, DateTime? createdSince);

        /// <summary>
        ///     根据用户列表获取上级数量。
        /// </summary>
        /// <param name="userIds">用户的编号列表。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="parentIdPrefix">上级的编号的前缀。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <returns>上级数量。</returns>
        List<KeyValuePair<int, int>> GetParentsCountByUsers(List<int> userIds, string parentType, string parentIdPrefix, DateTime? createdSince);

        /// <summary>
        ///     异步根据用户列表获取上级数量。
        /// </summary>
        /// <param name="userIds">用户的编号列表。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="parentIdPrefix">上级的编号的前缀。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <returns>上级数量。</returns>
        Task<List<KeyValuePair<int, int>>> GetParentsCountByUsersAsync(List<int> userIds, string parentType, string parentIdPrefix, DateTime? createdSince);

        /// <summary>
        ///     根据用户列表获取天数。
        /// </summary>
        /// <param name="userIds">用户的编号列表。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="parentIdPrefix">上级的编号的前缀。</param>
        /// <param name="createdSince">过滤创建天在指定的时间之后。</param>
        /// <returns>天数。</returns>
        List<KeyValuePair<int, int>> GetDaysCountByUsers(List<int> userIds, string parentType, string parentIdPrefix, DateTime? createdSince);

        /// <summary>
        ///     异步根据用户列表获取天数。
        /// </summary>
        /// <param name="userIds">用户的编号列表。</param>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="parentIdPrefix">上级的编号的前缀。</param>
        /// <param name="createdSince">过滤创建天在指定的时间之后。</param>
        /// <returns>天数。</returns>
        Task<List<KeyValuePair<int, int>>> GetDaysCountByUsersAsync(List<int> userIds, string parentType, string parentIdPrefix, DateTime? createdSince);

        /// <summary>
        ///     根据所有用户列表获取查看次数。
        /// </summary>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="parentIdPrefix">上级的编号的前缀。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <returns>查看次数。</returns>
        List<KeyValuePair<int, int>> GetViewsCountByAllUsers(string parentType, string parentIdPrefix, DateTime? createdSince);

        /// <summary>
        ///     异步根据所有用户列表获取查看次数。
        /// </summary>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="parentIdPrefix">上级的编号的前缀。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <returns>查看次数。</returns>
        Task<List<KeyValuePair<int, int>>> GetViewsCountByAllUsersAsync(string parentType, string parentIdPrefix, DateTime? createdSince);

        /// <summary>
        ///     根据所有用户列表获取上级数量。
        /// </summary>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="parentIdPrefix">上级的编号的前缀。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <returns>上级数量。</returns>
        List<KeyValuePair<int, int>> GetParentsCountByAllUsers(string parentType, string parentIdPrefix, DateTime? createdSince);

        /// <summary>
        ///     异步根据所有用户列表获取上级数量。
        /// </summary>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="parentIdPrefix">上级的编号的前缀。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <returns>上级数量。</returns>
        Task<List<KeyValuePair<int, int>>> GetParentsCountByAllUsersAsync(string parentType, string parentIdPrefix, DateTime? createdSince);

        /// <summary>
        ///     根据所有用户列表获取天数。
        /// </summary>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="parentIdPrefix">上级的编号的前缀。</param>
        /// <param name="createdSince">过滤创建天在指定的时间之后。</param>
        /// <returns>天数。</returns>
        List<KeyValuePair<int, int>> GetDaysCountByAllUsers(string parentType, string parentIdPrefix, DateTime? createdSince);

        /// <summary>
        ///     异步根据所有用户列表获取天数。
        /// </summary>
        /// <param name="parentType">上级的类型。</param>
        /// <param name="parentIdPrefix">上级的编号的前缀。</param>
        /// <param name="createdSince">过滤创建天在指定的时间之后。</param>
        /// <returns>天数。</returns>
        Task<List<KeyValuePair<int, int>>> GetDaysCountByAllUsersAsync(string parentType, string parentIdPrefix, DateTime? createdSince);

        /// <summary>
        ///     根据所有群组列表获取帖子查看次数。
        /// </summary>
        /// <returns>查看次数。</returns>
        List<KeyValuePair<string, int>> GetPostViewsCountByAllGroups();

        /// <summary>
        ///     异步根据所有群组列表获取帖子查看次数。
        /// </summary>
        /// <returns>查看次数。</returns>
        Task<List<KeyValuePair<string, int>>> GetPostViewsCountByAllGroupsAsync();

        /// <summary>
        ///     根据所有群组列表获取节查看次数。
        /// </summary>
        /// <returns>查看次数。</returns>
        List<KeyValuePair<string, int>> GetParagraphViewsCountByAllGroups();

        /// <summary>
        ///     异步根据所有群组列表获取节查看次数。
        /// </summary>
        /// <returns>查看次数。</returns>
        Task<List<KeyValuePair<string, int>>> GetParagraphViewsCountByAllGroupsAsync();

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