using System.Collections.Generic;
using System.Threading.Tasks;
using Sheep.Model.Bookstore.Entities;

namespace Sheep.Model.Bookstore
{
    /// <summary>
    ///     节注释的存储库的接口定义。
    /// </summary>
    public interface IParagraphAnnotationRepository
    {
        #region 获取

        /// <summary>
        ///     获取节注释。
        /// </summary>
        /// <param name="paragraphAnnotationId">节注释的编号。</param>
        /// <returns>节注释。</returns>
        ParagraphAnnotation GetParagraphAnnotation(string paragraphAnnotationId);

        /// <summary>
        ///     异步获取节注释。
        /// </summary>
        /// <param name="paragraphAnnotationId">节注释的编号。</param>
        /// <returns>节注释。</returns>
        Task<ParagraphAnnotation> GetParagraphAnnotationAsync(string paragraphAnnotationId);

        /// <summary>
        ///     根据节及序号获取节注释。
        /// </summary>
        /// <param name="paragraphId">节的编号。</param>
        /// <param name="number">序号。</param>
        /// <returns>节注释。</returns>
        ParagraphAnnotation GetParagraphAnnotation(string paragraphId, int number);

        /// <summary>
        ///     异步根据节及序号获取节注释。
        /// </summary>
        /// <param name="paragraphId">节的编号。</param>
        /// <param name="number">序号。</param>
        /// <returns>节注释。</returns>
        Task<ParagraphAnnotation> GetParagraphAnnotationAsync(string paragraphId, int number);

        /// <summary>
        ///     根据书籍及节序号及序号获取节注释。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="volumeNumber">卷的编号。</param>
        /// <param name="chapterNumber">章的编号。</param>
        /// <param name="paragraphNumber">节的编号。</param>
        /// <param name="number">序号。</param>
        /// <returns>节注释。</returns>
        ParagraphAnnotation GetParagraphAnnotation(string bookId, int volumeNumber, int chapterNumber, int paragraphNumber, int number);

        /// <summary>
        ///     异步根据书籍及节序号及序号获取节注释。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="volumeNumber">卷的编号。</param>
        /// <param name="chapterNumber">章的编号。</param>
        /// <param name="paragraphNumber">节的编号。</param>
        /// <param name="number">序号。</param>
        /// <returns>节注释。</returns>
        Task<ParagraphAnnotation> GetParagraphAnnotationAsync(string bookId, int volumeNumber, int chapterNumber, int paragraphNumber, int number);

        /// <summary>
        ///     查找节注释。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="volumeNumber">卷的编号。</param>
        /// <param name="chapterNumber">章的编号。</param>
        /// <param name="paragraphNumber">节的编号。</param>
        /// <param name="annotationFilter">过滤注释的表达式。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>节注释列表。</returns>
        List<ParagraphAnnotation> FindParagraphAnnotations(string bookId, int volumeNumber, int chapterNumber, int paragraphNumber, string annotationFilter, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步查找节注释。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="volumeNumber">卷的编号。</param>
        /// <param name="chapterNumber">章的编号。</param>
        /// <param name="paragraphNumber">节的编号。</param>
        /// <param name="annotationFilter">过滤注释的表达式。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>节注释列表。</returns>
        Task<List<ParagraphAnnotation>> FindParagraphAnnotationsAsync(string bookId, int volumeNumber, int chapterNumber, int paragraphNumber, string annotationFilter, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     根据节查找节注释。
        /// </summary>
        /// <param name="paragraphId">节的编号。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>节注释列表。</returns>
        List<ParagraphAnnotation> FindParagraphAnnotationsByParagraph(string paragraphId, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据节查找节注释。
        /// </summary>
        /// <param name="paragraphId">节的编号。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>节注释列表。</returns>
        Task<List<ParagraphAnnotation>> FindParagraphAnnotationsByParagraphAsync(string paragraphId, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     根据节列表查找节注释。
        /// </summary>
        /// <param name="paragraphIds">节的编号列表。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>节注释列表。</returns>
        List<ParagraphAnnotation> FindParagraphAnnotationsByParagraphs(List<string> paragraphIds, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据节列表查找节注释。
        /// </summary>
        /// <param name="paragraphIds">节的编号列表。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>节注释列表。</returns>
        Task<List<ParagraphAnnotation>> FindParagraphAnnotationsByParagraphsAsync(List<string> paragraphIds, string orderBy, bool? descending, int? skip, int? limit);

        #endregion

        #region 统计

        /// <summary>
        ///     查找获取节注释数量。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="volumeNumber">卷的编号。</param>
        /// <param name="chapterNumber">章的编号。</param>
        /// <param name="paragraphNumber">节的编号。</param>
        /// <param name="annotationFilter">过滤注释的表达式。</param>
        /// <returns>节注释数量。</returns>
        int GetParagraphAnnotationsCount(string bookId, int volumeNumber, int chapterNumber, int paragraphNumber, string annotationFilter);

        /// <summary>
        ///     异步获取节注释数量。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="volumeNumber">卷的编号。</param>
        /// <param name="chapterNumber">章的编号。</param>
        /// <param name="paragraphNumber">节的编号。</param>
        /// <param name="annotationFilter">过滤注释的表达式。</param>
        /// <returns>节注释数量。</returns>
        Task<int> GetParagraphAnnotationsCountAsync(string bookId, int volumeNumber, int chapterNumber, int paragraphNumber, string annotationFilter);

        /// <summary>
        ///     根据节获取节注释数量。
        /// </summary>
        /// <param name="paragraphId">节的编号。</param>
        /// <returns>节注释数量。</returns>
        int GetParagraphAnnotationsCountByParagraph(string paragraphId);

        /// <summary>
        ///     异步根据节获取节注释数量。
        /// </summary>
        /// <param name="paragraphId">节的编号。</param>
        /// <returns>节注释数量。</returns>
        Task<int> GetParagraphAnnotationsCountByParagraphAsync(string paragraphId);

        #endregion

        #region 写入

        /// <summary>
        ///     创建一个新的节注释。
        /// </summary>
        /// <param name="newParagraphAnnotation">新的节注释。</param>
        /// <returns>创建后的节注释。</returns>
        ParagraphAnnotation CreateParagraphAnnotation(ParagraphAnnotation newParagraphAnnotation);

        /// <summary>
        ///     异步创建一个新的节注释。
        /// </summary>
        /// <param name="newParagraphAnnotation">新的节注释。</param>
        /// <returns>创建后的节注释。</returns>
        Task<ParagraphAnnotation> CreateParagraphAnnotationAsync(ParagraphAnnotation newParagraphAnnotation);

        /// <summary>
        ///     更新一条节注释。
        /// </summary>
        /// <param name="existingParagraphAnnotation">原有的节注释。</param>
        /// <param name="newParagraphAnnotation">新的节注释。</param>
        /// <returns>更新后的节注释。</returns>
        ParagraphAnnotation UpdateParagraphAnnotation(ParagraphAnnotation existingParagraphAnnotation, ParagraphAnnotation newParagraphAnnotation);

        /// <summary>
        ///     异步更新一条节注释。
        /// </summary>
        /// <param name="existingParagraphAnnotation">原有的节注释。</param>
        /// <param name="newParagraphAnnotation">新的节注释。</param>
        /// <returns>更新后的节注释。</returns>
        Task<ParagraphAnnotation> UpdateParagraphAnnotationAsync(ParagraphAnnotation existingParagraphAnnotation, ParagraphAnnotation newParagraphAnnotation);

        /// <summary>
        ///     删除一条节注释。
        /// </summary>
        /// <param name="paragraphAnnotationId">节注释的编号。</param>
        void DeleteParagraphAnnotation(string paragraphAnnotationId);

        /// <summary>
        ///     异步删除一条节注释。
        /// </summary>
        /// <param name="paragraphAnnotationId">节注释的编号。</param>
        Task DeleteParagraphAnnotationAsync(string paragraphAnnotationId);

        #endregion
    }
}