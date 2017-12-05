using System.Collections.Generic;
using System.Threading.Tasks;
using Sheep.Model.Read.Entities;

namespace Sheep.Model.Read
{
    /// <summary>
    ///     章注释的存储库的接口定义。
    /// </summary>
    public interface IChapterAnnotationRepository
    {
        #region 获取

        /// <summary>
        ///     获取章注释。
        /// </summary>
        /// <param name="chapterAnnotationId">章注释的编号。</param>
        /// <returns>章注释。</returns>
        ChapterAnnotation GetChapterAnnotation(string chapterAnnotationId);

        /// <summary>
        ///     异步获取章注释。
        /// </summary>
        /// <param name="chapterAnnotationId">章注释的编号。</param>
        /// <returns>章注释。</returns>
        Task<ChapterAnnotation> GetChapterAnnotationAsync(string chapterAnnotationId);

        /// <summary>
        ///     查找章注释。
        /// </summary>
        /// <param name="annotationFilter">过滤注释的表达式。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>章注释列表。</returns>
        List<ChapterAnnotation> FindChapterAnnotations(string annotationFilter, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步查找章注释。
        /// </summary>
        /// <param name="annotationFilter">过滤注释的表达式。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>章注释列表。</returns>
        Task<List<ChapterAnnotation>> FindChapterAnnotationsAsync(string annotationFilter, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     根据章查找章注释。
        /// </summary>
        /// <param name="chapterId">章的编号。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>章注释列表。</returns>
        List<ChapterAnnotation> FindChapterAnnotationsByChapter(string chapterId, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据章查找章注释。
        /// </summary>
        /// <param name="chapterId">章的编号。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>章注释列表。</returns>
        Task<List<ChapterAnnotation>> FindChapterAnnotationsByChapterAsync(string chapterId, string orderBy, bool? descending, int? skip, int? limit);

        #endregion

        #region 统计

        /// <summary>
        ///     查找获取章注释数量。
        /// </summary>
        /// <param name="annotationFilter">过滤注释的表达式。</param>
        /// <returns>章注释数量。</returns>
        int GetChapterAnnotationsCount(string annotationFilter);

        /// <summary>
        ///     异步获取章注释数量。
        /// </summary>
        /// <param name="annotationFilter">过滤注释的表达式。</param>
        /// <returns>章注释数量。</returns>
        Task<int> GetChapterAnnotationsCountAsync(string annotationFilter);

        /// <summary>
        ///     根据章获取章注释数量。
        /// </summary>
        /// <param name="chapterId">章的编号。</param>
        /// <returns>章注释数量。</returns>
        int GetChapterAnnotationsCountByChapter(string chapterId);

        /// <summary>
        ///     异步根据章获取章注释数量。
        /// </summary>
        /// <param name="chapterId">章的编号。</param>
        /// <returns>章注释数量。</returns>
        Task<int> GetChapterAnnotationsCountByChapterAsync(string chapterId);

        #endregion

        #region 写入

        /// <summary>
        ///     创建一个新的章注释。
        /// </summary>
        /// <param name="newChapterAnnotation">新的章注释。</param>
        /// <returns>创建后的章注释。</returns>
        ChapterAnnotation CreateChapterAnnotation(ChapterAnnotation newChapterAnnotation);

        /// <summary>
        ///     异步创建一个新的章注释。
        /// </summary>
        /// <param name="newChapterAnnotation">新的章注释。</param>
        /// <returns>创建后的章注释。</returns>
        Task<ChapterAnnotation> CreateChapterAnnotationAsync(ChapterAnnotation newChapterAnnotation);

        /// <summary>
        ///     更新一个章注释。
        /// </summary>
        /// <param name="existingChapterAnnotation">原有的章注释。</param>
        /// <param name="newChapterAnnotation">新的章注释。</param>
        /// <returns>更新后的章注释。</returns>
        ChapterAnnotation UpdateChapterAnnotation(ChapterAnnotation existingChapterAnnotation, ChapterAnnotation newChapterAnnotation);

        /// <summary>
        ///     异步更新一个章注释。
        /// </summary>
        /// <param name="existingChapterAnnotation">原有的章注释。</param>
        /// <param name="newChapterAnnotation">新的章注释。</param>
        /// <returns>更新后的章注释。</returns>
        Task<ChapterAnnotation> UpdateChapterAnnotationAsync(ChapterAnnotation existingChapterAnnotation, ChapterAnnotation newChapterAnnotation);

        /// <summary>
        ///     删除一个章注释。
        /// </summary>
        /// <param name="chapterAnnotationId">章注释的编号。</param>
        void DeleteChapterAnnotation(string chapterAnnotationId);

        /// <summary>
        ///     异步删除一个章注释。
        /// </summary>
        /// <param name="chapterAnnotationId">章注释的编号。</param>
        Task DeleteChapterAnnotationAsync(string chapterAnnotationId);

        #endregion
    }
}