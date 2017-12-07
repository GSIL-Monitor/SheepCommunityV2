using System.Collections.Generic;
using System.Threading.Tasks;
using Sheep.Model.Read.Entities;

namespace Sheep.Model.Read
{
    /// <summary>
    ///     主题的存储库的接口定义。
    /// </summary>
    public interface ISubjectRepository
    {
        #region 获取

        /// <summary>
        ///     获取主题。
        /// </summary>
        /// <param name="subjectId">主题的编号。</param>
        /// <returns>主题。</returns>
        Subject GetSubject(string subjectId);

        /// <summary>
        ///     异步获取主题。
        /// </summary>
        /// <param name="subjectId">主题的编号。</param>
        /// <returns>主题。</returns>
        Task<Subject> GetSubjectAsync(string subjectId);

        /// <summary>
        ///     根据卷及序号获取主题。
        /// </summary>
        /// <param name="volumeId">卷的编号。</param>
        /// <param name="number">序号。</param>
        /// <returns>主题。</returns>
        Subject GetSubject(string volumeId, int number);

        /// <summary>
        ///     异步根据卷及序号获取主题。
        /// </summary>
        /// <param name="volumeId">卷的编号。</param>
        /// <param name="number">序号。</param>
        /// <returns>主题。</returns>
        Task<Subject> GetSubjectAsync(string volumeId, int number);

        /// <summary>
        ///     根据书籍及卷序号及序号获取主题。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="volumeNumber">卷的编号。</param>
        /// <param name="number">序号。</param>
        /// <returns>主题。</returns>
        Subject GetSubject(string bookId, int volumeNumber, int number);

        /// <summary>
        ///     异步根据书籍及卷序号及序号获取主题。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="volumeNumber">卷的编号。</param>
        /// <param name="number">序号。</param>
        /// <returns>主题。</returns>
        Task<Subject> GetSubjectAsync(string bookId, int volumeNumber, int number);

        /// <summary>
        ///     查找主题。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="volumeNumber">卷的编号。</param>
        /// <param name="titleFilter">过滤标题的表达式。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>主题列表。</returns>
        List<Subject> FindSubjects(string bookId, int volumeNumber, string titleFilter, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步查找主题。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="volumeNumber">卷的编号。</param>
        /// <param name="titleFilter">过滤标题的表达式。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>主题列表。</returns>
        Task<List<Subject>> FindSubjectsAsync(string bookId, int volumeNumber, string titleFilter, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     根据卷查找主题。
        /// </summary>
        /// <param name="volumeId">卷的编号。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>主题列表。</returns>
        List<Subject> FindSubjectsByVolume(string volumeId, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据卷查找主题。
        /// </summary>
        /// <param name="volumeId">卷的编号。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>主题列表。</returns>
        Task<List<Subject>> FindSubjectsByVolumeAsync(string volumeId, string orderBy, bool? descending, int? skip, int? limit);

        #endregion

        #region 统计

        /// <summary>
        ///     查找获取主题数量。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="volumeNumber">卷的编号。</param>
        /// <param name="titleFilter">过滤标题的表达式。</param>
        /// <returns>主题数量。</returns>
        int GetSubjectsCount(string bookId, int volumeNumber, string titleFilter);

        /// <summary>
        ///     异步获取主题数量。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="volumeNumber">卷的编号。</param>
        /// <param name="titleFilter">过滤标题的表达式。</param>
        /// <returns>主题数量。</returns>
        Task<int> GetSubjectsCountAsync(string bookId, int volumeNumber, string titleFilter);

        /// <summary>
        ///     根据卷获取主题数量。
        /// </summary>
        /// <param name="volumeId">卷的编号。</param>
        /// <returns>主题数量。</returns>
        int GetSubjectsCountByVolume(string volumeId);

        /// <summary>
        ///     异步根据卷获取主题数量。
        /// </summary>
        /// <param name="volumeId">卷的编号。</param>
        /// <returns>主题数量。</returns>
        Task<int> GetSubjectsCountByVolumeAsync(string volumeId);

        #endregion

        #region 写入

        /// <summary>
        ///     创建一个新的主题。
        /// </summary>
        /// <param name="newSubject">新的主题。</param>
        /// <returns>创建后的主题。</returns>
        Subject CreateSubject(Subject newSubject);

        /// <summary>
        ///     异步创建一个新的主题。
        /// </summary>
        /// <param name="newSubject">新的主题。</param>
        /// <returns>创建后的主题。</returns>
        Task<Subject> CreateSubjectAsync(Subject newSubject);

        /// <summary>
        ///     更新一个主题。
        /// </summary>
        /// <param name="existingSubject">原有的主题。</param>
        /// <param name="newSubject">新的主题。</param>
        /// <returns>更新后的主题。</returns>
        Subject UpdateSubject(Subject existingSubject, Subject newSubject);

        /// <summary>
        ///     异步更新一个主题。
        /// </summary>
        /// <param name="existingSubject">原有的主题。</param>
        /// <param name="newSubject">新的主题。</param>
        /// <returns>更新后的主题。</returns>
        Task<Subject> UpdateSubjectAsync(Subject existingSubject, Subject newSubject);

        /// <summary>
        ///     删除一个主题。
        /// </summary>
        /// <param name="subjectId">主题的编号。</param>
        void DeleteSubject(string subjectId);

        /// <summary>
        ///     异步删除一个主题。
        /// </summary>
        /// <param name="subjectId">主题的编号。</param>
        Task DeleteSubjectAsync(string subjectId);

        #endregion
    }
}