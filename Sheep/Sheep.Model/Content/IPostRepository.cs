using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sheep.Model.Content.Entities;

namespace Sheep.Model.Content
{
    /// <summary>
    ///     帖子的存储库的接口定义。
    /// </summary>
    public interface IPostRepository
    {
        #region 获取

        /// <summary>
        ///     获取帖子。
        /// </summary>
        /// <param name="postId">帖子的编号。</param>
        /// <returns>帖子。</returns>
        Post GetPost(string postId);

        /// <summary>
        ///     异步获取帖子。
        /// </summary>
        /// <param name="postId">帖子的编号。</param>
        /// <returns>帖子。</returns>
        Task<Post> GetPostAsync(string postId);

        /// <summary>
        ///     获取帖子列表。
        /// </summary>
        /// <param name="postIds">帖子的编号列表。</param>
        /// <returns>帖子列表。</returns>
        List<Post> GetPosts(List<string> postIds);

        /// <summary>
        ///     异步获取帖子列表。
        /// </summary>
        /// <param name="postIds">帖子的编号列表。</param>
        /// <returns>帖子列表。</returns>
        Task<List<Post>> GetPostsAsync(List<string> postIds);

        /// <summary>
        ///     查找帖子列表。
        /// </summary>
        /// <param name="titleFilter">过滤标题及概要的表达式。</param>
        /// <param name="tag">分类的标签。</param>
        /// <param name="contentType">内容的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="publishedSince">过滤发布日期在指定的时间之后。</param>
        /// <param name="isPublished">是否已发布。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <param name="excludedAuthorIds">排除的作者编号列表。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>帖子列表。</returns>
        List<Post> FindPosts(string titleFilter, string tag, string contentType, DateTime? createdSince, DateTime? modifiedSince, DateTime? publishedSince, bool? isPublished, bool? isFeatured, string status, List<int> excludedAuthorIds, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步查找帖子列表。
        /// </summary>
        /// <param name="titleFilter">过滤标题及概要的表达式。</param>
        /// <param name="tag">分类的标签。</param>
        /// <param name="contentType">内容的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="publishedSince">过滤发布日期在指定的时间之后。</param>
        /// <param name="isPublished">是否已发布。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <param name="excludedAuthorIds">排除的作者编号列表。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>帖子列表。</returns>
        Task<List<Post>> FindPostsAsync(string titleFilter, string tag, string contentType, DateTime? createdSince, DateTime? modifiedSince, DateTime? publishedSince, bool? isPublished, bool? isFeatured, string status, List<int> excludedAuthorIds, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     根据作者查找帖子列表。
        /// </summary>
        /// <param name="authorId">作者的用户编号。</param>
        /// <param name="contentType">内容的类型。</param>
        /// <param name="tag">分类的标签。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="publishedSince">过滤发布日期在指定的时间之后。</param>
        /// <param name="isPublished">是否已发布。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>帖子列表。</returns>
        List<Post> FindPostsByAuthor(int authorId, string tag, string contentType, DateTime? createdSince, DateTime? modifiedSince, DateTime? publishedSince, bool? isPublished, bool? isFeatured, string status, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据作者查找帖子列表。
        /// </summary>
        /// <param name="authorId">作者的用户编号。</param>
        /// <param name="tag">分类的标签。</param>
        /// <param name="contentType">内容的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="publishedSince">过滤发布日期在指定的时间之后。</param>
        /// <param name="isPublished">是否已发布。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>帖子列表。</returns>
        Task<List<Post>> FindPostsByAuthorAsync(int authorId, string tag, string contentType, DateTime? createdSince, DateTime? modifiedSince, DateTime? publishedSince, bool? isPublished, bool? isFeatured, string status, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     根据作者列表查找帖子列表。
        /// </summary>
        /// <param name="authorIds">作者的用户编号列表。</param>
        /// <param name="contentType">内容的类型。</param>
        /// <param name="tag">分类的标签。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="publishedSince">过滤发布日期在指定的时间之后。</param>
        /// <param name="isPublished">是否已发布。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>帖子列表。</returns>
        List<Post> FindPostsByAuthors(List<int> authorIds, string tag, string contentType, DateTime? createdSince, DateTime? modifiedSince, DateTime? publishedSince, bool? isPublished, bool? isFeatured, string status, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据作者列表查找帖子列表。
        /// </summary>
        /// <param name="authorIds">作者的用户编号列表。</param>
        /// <param name="tag">分类的标签。</param>
        /// <param name="contentType">内容的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="publishedSince">过滤发布日期在指定的时间之后。</param>
        /// <param name="isPublished">是否已发布。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>帖子列表。</returns>
        Task<List<Post>> FindPostsByAuthorsAsync(List<int> authorIds, string tag, string contentType, DateTime? createdSince, DateTime? modifiedSince, DateTime? publishedSince, bool? isPublished, bool? isFeatured, string status, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     根据群组查找帖子列表。
        /// </summary>
        /// <param name="groupId">群组的编号。</param>
        /// <param name="tag">分类的标签。</param>
        /// <param name="contentType">内容的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="publishedSince">过滤发布日期在指定的时间之后。</param>
        /// <param name="isPublished">是否已发布。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <param name="excludedAuthorIds">排除的作者编号列表。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>帖子列表。</returns>
        List<Post> FindPostsByGroup(string groupId, string tag, string contentType, DateTime? createdSince, DateTime? modifiedSince, DateTime? publishedSince, bool? isPublished, bool? isFeatured, string status, List<int> excludedAuthorIds, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据群组查找帖子列表。
        /// </summary>
        /// <param name="groupId">群组的编号。</param>
        /// <param name="tag">分类的标签。</param>
        /// <param name="contentType">内容的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="publishedSince">过滤发布日期在指定的时间之后。</param>
        /// <param name="isPublished">是否已发布。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <param name="excludedAuthorIds">排除的作者编号列表。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>帖子列表。</returns>
        Task<List<Post>> FindPostsByGroupAsync(string groupId, string tag, string contentType, DateTime? createdSince, DateTime? modifiedSince, DateTime? publishedSince, bool? isPublished, bool? isFeatured, string status, List<int> excludedAuthorIds, string orderBy, bool? descending, int? skip, int? limit);

        #endregion

        #region 统计

        /// <summary>
        ///     查找获取帖子数量。
        /// </summary>
        /// <param name="titleFilter">过滤标题及概要的表达式。</param>
        /// <param name="tag">分类的标签。</param>
        /// <param name="contentType">内容的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="publishedSince">过滤发布日期在指定的时间之后。</param>
        /// <param name="isPublished">是否已发布。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <param name="excludedAuthorIds">排除的作者编号列表。</param>
        /// <returns>帖子数量。</returns>
        int GetPostsCount(string titleFilter, string tag, string contentType, DateTime? createdSince, DateTime? modifiedSince, DateTime? publishedSince, bool? isPublished, bool? isFeatured, string status, List<int> excludedAuthorIds);

        /// <summary>
        ///     异步获取帖子数量。
        /// </summary>
        /// <param name="titleFilter">过滤标题及概要的表达式。</param>
        /// <param name="tag">分类的标签。</param>
        /// <param name="contentType">内容的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="publishedSince">过滤发布日期在指定的时间之后。</param>
        /// <param name="isPublished">是否已发布。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <param name="excludedAuthorIds">排除的作者编号列表。</param>
        /// <returns>帖子数量。</returns>
        Task<int> GetPostsCountAsync(string titleFilter, string tag, string contentType, DateTime? createdSince, DateTime? modifiedSince, DateTime? publishedSince, bool? isPublished, bool? isFeatured, string status, List<int> excludedAuthorIds);

        /// <summary>
        ///     根据作者查找帖子数量。
        /// </summary>
        /// <param name="authorId">作者的用户编号。</param>
        /// <param name="contentType">内容的类型。</param>
        /// <param name="tag">分类的标签。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="publishedSince">过滤发布日期在指定的时间之后。</param>
        /// <param name="isPublished">是否已发布。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>帖子数量。</returns>
        int GetPostsCountByAuthor(int authorId, string tag, string contentType, DateTime? createdSince, DateTime? modifiedSince, DateTime? publishedSince, bool? isPublished, bool? isFeatured, string status);

        /// <summary>
        ///     异步根据作者获取帖子数量。
        /// </summary>
        /// <param name="authorId">作者的用户编号。</param>
        /// <param name="tag">分类的标签。</param>
        /// <param name="contentType">内容的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="publishedSince">过滤发布日期在指定的时间之后。</param>
        /// <param name="isPublished">是否已发布。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>帖子数量。</returns>
        Task<int> GetPostsCountByAuthorAsync(int authorId, string tag, string contentType, DateTime? createdSince, DateTime? modifiedSince, DateTime? publishedSince, bool? isPublished, bool? isFeatured, string status);

        /// <summary>
        ///     根据作者查找帖子平均内容质量的得分。
        /// </summary>
        /// <param name="authorId">作者的用户编号。</param>
        /// <param name="contentType">内容的类型。</param>
        /// <param name="tag">分类的标签。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="publishedSince">过滤发布日期在指定的时间之后。</param>
        /// <param name="isPublished">是否已发布。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>帖子平均内容质量的得分。</returns>
        float GetPostsAverageContentScoreByAuthor(int authorId, string tag, string contentType, DateTime? createdSince, DateTime? modifiedSince, DateTime? publishedSince, bool? isPublished, bool? isFeatured, string status);

        /// <summary>
        ///     异步根据作者获取帖子平均内容质量的得分。
        /// </summary>
        /// <param name="authorId">作者的用户编号。</param>
        /// <param name="tag">分类的标签。</param>
        /// <param name="contentType">内容的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="publishedSince">过滤发布日期在指定的时间之后。</param>
        /// <param name="isPublished">是否已发布。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>帖子平均内容质量的得分。</returns>
        Task<float> GetPostsAverageContentScoreByAuthorAsync(int authorId, string tag, string contentType, DateTime? createdSince, DateTime? modifiedSince, DateTime? publishedSince, bool? isPublished, bool? isFeatured, string status);

        /// <summary>
        ///     根据作者列表查找帖子数量。
        /// </summary>
        /// <param name="authorIds">作者的用户编号列表。</param>
        /// <param name="contentType">内容的类型。</param>
        /// <param name="tag">分类的标签。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="publishedSince">过滤发布日期在指定的时间之后。</param>
        /// <param name="isPublished">是否已发布。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>帖子数量。</returns>
        int GetPostsCountByAuthors(List<int> authorIds, string tag, string contentType, DateTime? createdSince, DateTime? modifiedSince, DateTime? publishedSince, bool? isPublished, bool? isFeatured, string status);

        /// <summary>
        ///     异步根据作者获取帖子数量。
        /// </summary>
        /// <param name="authorIds">作者的用户编号列表。</param>
        /// <param name="tag">分类的标签。</param>
        /// <param name="contentType">内容的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="publishedSince">过滤发布日期在指定的时间之后。</param>
        /// <param name="isPublished">是否已发布。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>帖子数量。</returns>
        Task<int> GetPostsCountByAuthorsAsync(List<int> authorIds, string tag, string contentType, DateTime? createdSince, DateTime? modifiedSince, DateTime? publishedSince, bool? isPublished, bool? isFeatured, string status);

        /// <summary>
        ///     根据群组获取帖子数量。
        /// </summary>
        /// <param name="groupId">群组的编号。</param>
        /// <param name="tag">分类的标签。</param>
        /// <param name="contentType">内容的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="publishedSince">过滤发布日期在指定的时间之后。</param>
        /// <param name="isPublished">是否已发布。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <param name="excludedAuthorIds">排除的作者编号列表。</param>
        /// <returns>帖子数量。</returns>
        int GetPostsCountByGroup(string groupId, string tag, string contentType, DateTime? createdSince, DateTime? modifiedSince, DateTime? publishedSince, bool? isPublished, bool? isFeatured, string status, List<int> excludedAuthorIds);

        /// <summary>
        ///     异步根据群组获取帖子数量。
        /// </summary>
        /// <param name="groupId">群组的编号。</param>
        /// <param name="tag">分类的标签。</param>
        /// <param name="contentType">内容的类型。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="publishedSince">过滤发布日期在指定的时间之后。</param>
        /// <param name="isPublished">是否已发布。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <param name="excludedAuthorIds">排除的作者编号列表。</param>
        /// <returns>帖子数量。</returns>
        Task<int> GetPostsCountByGroupAsync(string groupId, string tag, string contentType, DateTime? createdSince, DateTime? modifiedSince, DateTime? publishedSince, bool? isPublished, bool? isFeatured, string status, List<int> excludedAuthorIds);

        #endregion

        #region 计算

        /// <summary>
        ///     计算内容的得分。
        /// </summary>
        /// <param name="post">帖子。</param>
        /// <returns>得分。</returns>
        float CalculatePostContentScore(Post post);

        /// <summary>
        ///     异步计算内容的得分。
        /// </summary>
        /// <param name="post">帖子。</param>
        /// <returns>得分。</returns>
        Task<float> CalculatePostContentScoreAsync(Post post);

        /// <summary>
        ///     计算精选的得分。
        /// </summary>
        /// <param name="post">帖子。</param>
        /// <returns>得分。</returns>
        float CalculatePostFeaturedScore(Post post);

        /// <summary>
        ///     异步计算精选的得分。
        /// </summary>
        /// <param name="post">帖子。</param>
        /// <returns>得分。</returns>
        Task<float> CalculatePostFeaturedScoreAsync(Post post);

        /// <summary>
        ///     计算标签的得分。
        /// </summary>
        /// <returns>得分。</returns>
        float CalculatePostTagsScore(Post post);

        /// <summary>
        ///     异步计算标签的得分。
        /// </summary>
        /// <returns>得分。</returns>
        Task<float> CalculatePostTagsScoreAsync(Post post);

        /// <summary>
        ///     计算查看的得分。
        /// </summary>
        /// <param name="post">帖子。</param>
        /// <returns>得分。</returns>
        float CalculatePostViewsScore(Post post);

        /// <summary>
        ///     异步计算查看的得分。
        /// </summary>
        /// <param name="post">帖子。</param>
        /// <returns>得分。</returns>
        Task<float> CalculatePostViewsScoreAsync(Post post);

        /// <summary>
        ///     计算收藏的得分。
        /// </summary>
        /// <param name="post">帖子。</param>
        /// <returns>得分。</returns>
        float CalculatePostBookmarksScore(Post post);

        /// <summary>
        ///     异步计算收藏的得分。
        /// </summary>
        /// <param name="post">帖子。</param>
        /// <returns>得分。</returns>
        Task<float> CalculatePostBookmarksScoreAsync(Post post);

        /// <summary>
        ///     计算评论的得分。
        /// </summary>
        /// <param name="post">帖子。</param>
        /// <returns>得分。</returns>
        float CalculatePostCommentsScore(Post post);

        /// <summary>
        ///     异步计算评论的得分。
        /// </summary>
        /// <param name="post">帖子。</param>
        /// <returns>得分。</returns>
        Task<float> CalculatePostCommentsScoreAsync(Post post);

        /// <summary>
        ///     计算点赞的得分。
        /// </summary>
        /// <param name="post">帖子。</param>
        /// <returns>得分。</returns>
        float CalculatePostLikesScore(Post post);

        /// <summary>
        ///     异步计算点赞的得分。
        /// </summary>
        /// <param name="post">帖子。</param>
        /// <returns>得分。</returns>
        Task<float> CalculatePostLikesScoreAsync(Post post);

        /// <summary>
        ///     计算评分的得分。
        /// </summary>
        /// <param name="post">帖子。</param>
        /// <returns>得分。</returns>
        float CalculatePostRatingsScore(Post post);

        /// <summary>
        ///     异步计算评分的得分。
        /// </summary>
        /// <param name="post">帖子。</param>
        /// <returns>得分。</returns>
        Task<float> CalculatePostRatingsScoreAsync(Post post);

        /// <summary>
        ///     计算分享的得分。
        /// </summary>
        /// <param name="post">帖子。</param>
        /// <returns>得分。</returns>
        float CalculatePostSharesScore(Post post);

        /// <summary>
        ///     异步计算分享的得分。
        /// </summary>
        /// <param name="post">帖子。</param>
        /// <returns>得分。</returns>
        Task<float> CalculatePostSharesScoreAsync(Post post);

        /// <summary>
        ///     计算滥用举报的得分。
        /// </summary>
        /// <param name="post">帖子。</param>
        /// <returns>得分。</returns>
        float CalculatePostAbuseReportsScore(Post post);

        /// <summary>
        ///     异步计算滥用举报的得分。
        /// </summary>
        /// <param name="post">帖子。</param>
        /// <returns>得分。</returns>
        Task<float> CalculatePostAbuseReportsScoreAsync(Post post);

        /// <summary>
        ///     计算内容质量的得分。
        /// </summary>
        /// <param name="post">帖子。</param>
        /// <param name="contentWeight">内容的权重。</param>
        /// <param name="featuredWeight">精选的权重。</param>
        /// <param name="tagsWeight">标签的权重。</param>
        /// <param name="viewsWeight">查看的权重。</param>
        /// <param name="bookmarksWeight">收藏的权重。</param>
        /// <param name="commentsWeight">评论的权重。</param>
        /// <param name="likesWeight">点赞的权重。</param>
        /// <param name="ratingsWeight">评分的权重。</param>
        /// <param name="sharesWeight">分享的权重。</param>
        /// <param name="decayHalfLife">得分的半衰期。（天）</param>
        /// <returns>得分。</returns>
        float CalculatePostContentQuality(Post post, float contentWeight = 1.0f, float featuredWeight = 1.0f, float tagsWeight = 1.0f, float viewsWeight = 1.0f, float bookmarksWeight = 1.0f, float commentsWeight = 1.0f, float likesWeight = 1.0f, float ratingsWeight = 1.0f, float sharesWeight = 1.0f, int decayHalfLife = 30);

        /// <summary>
        ///     异步计算内容质量的得分。
        /// </summary>
        /// <param name="post">帖子。</param>
        /// <param name="contentWeight">内容的权重。</param>
        /// <param name="featuredWeight">精选的权重。</param>
        /// <param name="tagsWeight">标签的权重。</param>
        /// <param name="viewsWeight">查看的权重。</param>
        /// <param name="bookmarksWeight">收藏的权重。</param>
        /// <param name="commentsWeight">评论的权重。</param>
        /// <param name="likesWeight">点赞的权重。</param>
        /// <param name="ratingsWeight">评分的权重。</param>
        /// <param name="sharesWeight">分享的权重。</param>
        /// <param name="decayHalfLife">得分的半衰期。（天）</param>
        /// <returns>得分。</returns>
        Task<float> CalculatePostContentQualityAsync(Post post, float contentWeight = 1.0f, float featuredWeight = 1.0f, float tagsWeight = 1.0f, float viewsWeight = 1.0f, float bookmarksWeight = 1.0f, float commentsWeight = 1.0f, float likesWeight = 1.0f, float ratingsWeight = 1.0f, float sharesWeight = 1.0f, int decayHalfLife = 30);

        /// <summary>
        ///     计算用户帖子的得分。
        /// </summary>
        /// <param name="authorId">作者的用户编号。</param>
        /// <param name="contentType">内容的类型。</param>
        /// <param name="tag">分类的标签。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="publishedSince">过滤发布日期在指定的时间之后。</param>
        /// <param name="isPublished">是否已发布。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>得分。</returns>
        float CalculateAuthorPostsScore(int authorId, string tag, string contentType, DateTime? createdSince, DateTime? modifiedSince, DateTime? publishedSince, bool? isPublished, bool? isFeatured, string status);

        /// <summary>
        ///     异步计算用户帖子的得分。
        /// </summary>
        /// <param name="authorId">作者的用户编号。</param>
        /// <param name="contentType">内容的类型。</param>
        /// <param name="tag">分类的标签。</param>
        /// <param name="createdSince">过滤创建日期在指定的时间之后。</param>
        /// <param name="modifiedSince">过滤修改日期在指定的时间之后。</param>
        /// <param name="publishedSince">过滤发布日期在指定的时间之后。</param>
        /// <param name="isPublished">是否已发布。</param>
        /// <param name="isFeatured">是否标记为精选。</param>
        /// <param name="status"> 过滤状态。</param>
        /// <returns>得分。</returns>
        Task<float> CalculateAuthorPostsScoreAsync(int authorId, string tag, string contentType, DateTime? createdSince, DateTime? modifiedSince, DateTime? publishedSince, bool? isPublished, bool? isFeatured, string status);

        #endregion

        #region 写入

        /// <summary>
        ///     创建一个新的帖子。
        /// </summary>
        /// <param name="newPost">新的帖子。</param>
        /// <returns>创建后的帖子。</returns>
        Post CreatePost(Post newPost);

        /// <summary>
        ///     异步创建一个新的帖子。
        /// </summary>
        /// <param name="newPost">新的帖子。</param>
        /// <returns>创建后的帖子。</returns>
        Task<Post> CreatePostAsync(Post newPost);

        /// <summary>
        ///     更新一个帖子。
        /// </summary>
        /// <param name="existingPost">原有的帖子。</param>
        /// <param name="newPost">新的帖子。</param>
        /// <returns>更新后的帖子。</returns>
        Post UpdatePost(Post existingPost, Post newPost);

        /// <summary>
        ///     异步更新一个帖子。
        /// </summary>
        /// <param name="existingPost">原有的帖子。</param>
        /// <param name="newPost">新的帖子。</param>
        /// <returns>更新后的帖子。</returns>
        Task<Post> UpdatePostAsync(Post existingPost, Post newPost);

        /// <summary>
        ///     删除一个帖子。
        /// </summary>
        /// <param name="postId">帖子的编号。</param>
        void DeletePost(string postId);

        /// <summary>
        ///     异步删除一个帖子。
        /// </summary>
        /// <param name="postId">帖子的编号。</param>
        Task DeletePostAsync(string postId);

        /// <summary>
        ///     增加一个帖子的查看的次数。
        /// </summary>
        /// <param name="postId">帖子的编号。</param>
        /// <param name="count">增加的数量。</param>
        void IncrementPostViewsCount(string postId, int count);

        /// <summary>
        ///     异步增加一个帖子的查看的次数。
        /// </summary>
        /// <param name="postId">帖子的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task IncrementPostViewsCountAsync(string postId, int count);

        /// <summary>
        ///     增加一个帖子的收藏的次数。
        /// </summary>
        /// <param name="postId">帖子的编号。</param>
        /// <param name="count">增加的数量。</param>
        void IncrementPostBookmarksCount(string postId, int count);

        /// <summary>
        ///     异步增加一个帖子的收藏的次数。
        /// </summary>
        /// <param name="postId">帖子的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task IncrementPostBookmarksCountAsync(string postId, int count);

        /// <summary>
        ///     增加一个帖子的评论的次数。
        /// </summary>
        /// <param name="postId">帖子的编号。</param>
        /// <param name="count">增加的数量。</param>
        void IncrementPostCommentsCount(string postId, int count);

        /// <summary>
        ///     异步增加一个帖子的评论的次数。
        /// </summary>
        /// <param name="postId">帖子的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task IncrementPostCommentsCountAsync(string postId, int count);

        /// <summary>
        ///     增加一个帖子的点赞的次数。
        /// </summary>
        /// <param name="postId">帖子的编号。</param>
        /// <param name="count">增加的数量。</param>
        void IncrementPostLikesCount(string postId, int count);

        /// <summary>
        ///     异步增加一个帖子的点赞的次数。
        /// </summary>
        /// <param name="postId">帖子的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task IncrementPostLikesCountAsync(string postId, int count);

        /// <summary>
        ///     增加一个帖子的评分的次数。
        /// </summary>
        /// <param name="postId">帖子的编号。</param>
        /// <param name="count">增加的数量。</param>
        void IncrementPostRatingsCount(string postId, int count);

        /// <summary>
        ///     异步增加一个帖子的评分的次数。
        /// </summary>
        /// <param name="postId">帖子的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task IncrementPostRatingsCountAsync(string postId, int count);

        /// <summary>
        ///     增加一个帖子的分享的次数。
        /// </summary>
        /// <param name="postId">帖子的编号。</param>
        /// <param name="count">增加的数量。</param>
        void IncrementPostSharesCount(string postId, int count);

        /// <summary>
        ///     异步增加一个帖子的分享的次数。
        /// </summary>
        /// <param name="postId">帖子的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task IncrementPostSharesCountAsync(string postId, int count);

        /// <summary>
        ///     增加一个帖子的滥用举报的次数。
        /// </summary>
        /// <param name="postId">帖子的编号。</param>
        /// <param name="count">增加的数量。</param>
        void IncrementPostAbuseReportsCount(string postId, int count);

        /// <summary>
        ///     异步增加一个帖子的滥用举报的次数。
        /// </summary>
        /// <param name="postId">帖子的编号。</param>
        /// <param name="count">增加的数量。</param>
        Task IncrementPostAbuseReportsCountAsync(string postId, int count);

        /// <summary>
        ///     更新一个帖子的评分的平均值。
        /// </summary>
        /// <param name="postId">帖子的编号。</param>
        /// <param name="value">更新的数值。</param>
        void UpdatePostRatingsAverageValue(string postId, float value);

        /// <summary>
        ///     异步更新一个帖子的评分的平均值。
        /// </summary>
        /// <param name="postId">帖子的编号。</param>
        /// <param name="value">更新的数值。</param>
        Task UpdatePostRatingsAverageValueAsync(string postId, float value);

        /// <summary>
        ///     更新一个帖子的内容质量的评分。
        /// </summary>
        /// <param name="postId">帖子的编号。</param>
        /// <param name="value">更新的数值。</param>
        void UpdatePostContentQuality(string postId, float value);

        /// <summary>
        ///     异步更新一个帖子的内容质量的评分。
        /// </summary>
        /// <param name="postId">帖子的编号。</param>
        /// <param name="value">更新的数值。</param>
        Task UpdatePostContentQualityAsync(string postId, float value);

        #endregion
    }
}