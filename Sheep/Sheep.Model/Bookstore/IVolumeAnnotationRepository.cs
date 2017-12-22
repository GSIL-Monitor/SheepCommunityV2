using System.Collections.Generic;
using System.Threading.Tasks;
using Sheep.Model.Bookstore.Entities;

namespace Sheep.Model.Bookstore
{
    /// <summary>
    ///     卷注释的存储库的接口定义。
    /// </summary>
    public interface IVolumeAnnotationRepository
    {
        #region 获取

        /// <summary>
        ///     获取卷注释。
        /// </summary>
        /// <param name="volumeAnnotationId">卷注释的编号。</param>
        /// <returns>卷注释。</returns>
        VolumeAnnotation GetVolumeAnnotation(string volumeAnnotationId);

        /// <summary>
        ///     异步获取卷注释。
        /// </summary>
        /// <param name="volumeAnnotationId">卷注释的编号。</param>
        /// <returns>卷注释。</returns>
        Task<VolumeAnnotation> GetVolumeAnnotationAsync(string volumeAnnotationId);

        /// <summary>
        ///     根据卷及序号获取卷注释。
        /// </summary>
        /// <param name="volumeId">卷的编号。</param>
        /// <param name="number">序号。</param>
        /// <returns>卷注释。</returns>
        VolumeAnnotation GetVolumeAnnotation(string volumeId, int number);

        /// <summary>
        ///     异步根据卷及序号获取卷注释。
        /// </summary>
        /// <param name="volumeId">卷的编号。</param>
        /// <param name="number">序号。</param>
        /// <returns>卷注释。</returns>
        Task<VolumeAnnotation> GetVolumeAnnotationAsync(string volumeId, int number);

        /// <summary>
        ///     根据书籍及卷序号及序号获取卷注释。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="volumeNumber">卷的编号。</param>
        /// <param name="number">序号。</param>
        /// <returns>卷注释。</returns>
        VolumeAnnotation GetVolumeAnnotation(string bookId, int volumeNumber, int number);

        /// <summary>
        ///     异步根据书籍及卷序号及序号获取卷注释。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="volumeNumber">卷的编号。</param>
        /// <param name="number">序号。</param>
        /// <returns>卷注释。</returns>
        Task<VolumeAnnotation> GetVolumeAnnotationAsync(string bookId, int volumeNumber, int number);

        /// <summary>
        ///     查找卷注释。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="volumeNumber">卷的编号。</param>
        /// <param name="annotationFilter">过滤注释的表达式。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>卷注释列表。</returns>
        List<VolumeAnnotation> FindVolumeAnnotations(string bookId, int volumeNumber, string annotationFilter, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步查找卷注释。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="volumeNumber">卷的编号。</param>
        /// <param name="annotationFilter">过滤注释的表达式。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>卷注释列表。</returns>
        Task<List<VolumeAnnotation>> FindVolumeAnnotationsAsync(string bookId, int volumeNumber, string annotationFilter, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     根据卷查找卷注释。
        /// </summary>
        /// <param name="volumeId">卷的编号。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>卷注释列表。</returns>
        List<VolumeAnnotation> FindVolumeAnnotationsByVolume(string volumeId, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据卷查找卷注释。
        /// </summary>
        /// <param name="volumeId">卷的编号。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>卷注释列表。</returns>
        Task<List<VolumeAnnotation>> FindVolumeAnnotationsByVolumeAsync(string volumeId, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     根据卷列表查找卷注释。
        /// </summary>
        /// <param name="volumeIds">卷的编号列表。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>卷注释列表。</returns>
        List<VolumeAnnotation> FindVolumeAnnotationsByVolumes(IEnumerable<string> volumeIds, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据卷列表查找卷注释。
        /// </summary>
        /// <param name="volumeIds">卷的编号列表。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>卷注释列表。</returns>
        Task<List<VolumeAnnotation>> FindVolumeAnnotationsByVolumesAsync(IEnumerable<string> volumeIds, string orderBy, bool? descending, int? skip, int? limit);

        #endregion

        #region 统计

        /// <summary>
        ///     查找获取卷注释数量。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="volumeNumber">卷的编号。</param>
        /// <param name="annotationFilter">过滤注释的表达式。</param>
        /// <returns>卷注释数量。</returns>
        int GetVolumeAnnotationsCount(string bookId, int volumeNumber, string annotationFilter);

        /// <summary>
        ///     异步获取卷注释数量。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="volumeNumber">卷的编号。</param>
        /// <param name="annotationFilter">过滤注释的表达式。</param>
        /// <returns>卷注释数量。</returns>
        Task<int> GetVolumeAnnotationsCountAsync(string bookId, int volumeNumber, string annotationFilter);

        /// <summary>
        ///     根据卷获取卷注释数量。
        /// </summary>
        /// <param name="volumeId">卷的编号。</param>
        /// <returns>卷注释数量。</returns>
        int GetVolumeAnnotationsCountByVolume(string volumeId);

        /// <summary>
        ///     异步根据卷获取卷注释数量。
        /// </summary>
        /// <param name="volumeId">卷的编号。</param>
        /// <returns>卷注释数量。</returns>
        Task<int> GetVolumeAnnotationsCountByVolumeAsync(string volumeId);

        #endregion

        #region 写入

        /// <summary>
        ///     创建一个新的卷注释。
        /// </summary>
        /// <param name="newVolumeAnnotation">新的卷注释。</param>
        /// <returns>创建后的卷注释。</returns>
        VolumeAnnotation CreateVolumeAnnotation(VolumeAnnotation newVolumeAnnotation);

        /// <summary>
        ///     异步创建一个新的卷注释。
        /// </summary>
        /// <param name="newVolumeAnnotation">新的卷注释。</param>
        /// <returns>创建后的卷注释。</returns>
        Task<VolumeAnnotation> CreateVolumeAnnotationAsync(VolumeAnnotation newVolumeAnnotation);

        /// <summary>
        ///     更新一条卷注释。
        /// </summary>
        /// <param name="existingVolumeAnnotation">原有的卷注释。</param>
        /// <param name="newVolumeAnnotation">新的卷注释。</param>
        /// <returns>更新后的卷注释。</returns>
        VolumeAnnotation UpdateVolumeAnnotation(VolumeAnnotation existingVolumeAnnotation, VolumeAnnotation newVolumeAnnotation);

        /// <summary>
        ///     异步更新一条卷注释。
        /// </summary>
        /// <param name="existingVolumeAnnotation">原有的卷注释。</param>
        /// <param name="newVolumeAnnotation">新的卷注释。</param>
        /// <returns>更新后的卷注释。</returns>
        Task<VolumeAnnotation> UpdateVolumeAnnotationAsync(VolumeAnnotation existingVolumeAnnotation, VolumeAnnotation newVolumeAnnotation);

        /// <summary>
        ///     删除一条卷注释。
        /// </summary>
        /// <param name="volumeAnnotationId">卷注释的编号。</param>
        void DeleteVolumeAnnotation(string volumeAnnotationId);

        /// <summary>
        ///     异步删除一条卷注释。
        /// </summary>
        /// <param name="volumeAnnotationId">卷注释的编号。</param>
        Task DeleteVolumeAnnotationAsync(string volumeAnnotationId);

        #endregion
    }
}