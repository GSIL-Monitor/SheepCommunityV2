using System.Collections.Generic;
using System.Threading.Tasks;
using Sheep.Model.Read.Entities;

namespace Sheep.Model.Read
{
    /// <summary>
    ///     卷的存储库的接口定义。
    /// </summary>
    public interface IVolumeRepository
    {
        #region 获取

        /// <summary>
        ///     获取卷。
        /// </summary>
        /// <param name="volumeId">卷的编号。</param>
        /// <returns>卷。</returns>
        Volume GetVolume(string volumeId);

        /// <summary>
        ///     异步获取卷。
        /// </summary>
        /// <param name="volumeId">卷的编号。</param>
        /// <returns>卷。</returns>
        Task<Volume> GetVolumeAsync(string volumeId);

        /// <summary>
        ///     根据书籍及序号获取卷。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="number">序号。</param>
        /// <returns>卷。</returns>
        Volume GetVolume(string bookId, int number);

        /// <summary>
        ///     异步根据书籍及序号获取卷。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="number">序号。</param>
        /// <returns>卷。</returns>
        Task<Volume> GetVolumeAsync(string bookId, int number);

        /// <summary>
        ///     查找卷。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="titleFilter">过滤标题及概要的表达式。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>卷列表。</returns>
        List<Volume> FindVolumes(string bookId, string titleFilter, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步查找卷。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="titleFilter">过滤标题及概要的表达式。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>卷列表。</returns>
        Task<List<Volume>> FindVolumesAsync(string bookId, string titleFilter, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     根据书籍查找卷。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>卷列表。</returns>
        List<Volume> FindVolumesByBook(string bookId, string orderBy, bool? descending, int? skip, int? limit);

        /// <summary>
        ///     异步根据书籍查找卷。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="orderBy">排序的字段。</param>
        /// <param name="descending">是否按降序排序。</param>
        /// <param name="skip">忽略的行数。</param>
        /// <param name="limit">获取的行数。</param>
        /// <returns>卷列表。</returns>
        Task<List<Volume>> FindVolumesByBookAsync(string bookId, string orderBy, bool? descending, int? skip, int? limit);

        #endregion

        #region 统计

        /// <summary>
        ///     查找获取卷数量。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="titleFilter">过滤标题及概要的表达式。</param>
        /// <returns>卷数量。</returns>
        int GetVolumesCount(string bookId, string titleFilter);

        /// <summary>
        ///     异步获取卷数量。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <param name="titleFilter">过滤标题及概要的表达式。</param>
        /// <returns>卷数量。</returns>
        Task<int> GetVolumesCountAsync(string bookId, string titleFilter);

        /// <summary>
        ///     根据书籍获取卷数量。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <returns>卷数量。</returns>
        int GetVolumesCountByBook(string bookId);

        /// <summary>
        ///     异步根据书籍获取卷数量。
        /// </summary>
        /// <param name="bookId">书籍的编号。</param>
        /// <returns>卷数量。</returns>
        Task<int> GetVolumesCountByBookAsync(string bookId);

        #endregion

        #region 写入

        /// <summary>
        ///     创建一个新的卷。
        /// </summary>
        /// <param name="newVolume">新的卷。</param>
        /// <returns>创建后的卷。</returns>
        Volume CreateVolume(Volume newVolume);

        /// <summary>
        ///     异步创建一个新的卷。
        /// </summary>
        /// <param name="newVolume">新的卷。</param>
        /// <returns>创建后的卷。</returns>
        Task<Volume> CreateVolumeAsync(Volume newVolume);

        /// <summary>
        ///     更新一卷。
        /// </summary>
        /// <param name="existingVolume">原有的卷。</param>
        /// <param name="newVolume">新的卷。</param>
        /// <returns>更新后的卷。</returns>
        Volume UpdateVolume(Volume existingVolume, Volume newVolume);

        /// <summary>
        ///     异步更新一卷。
        /// </summary>
        /// <param name="existingVolume">原有的卷。</param>
        /// <param name="newVolume">新的卷。</param>
        /// <returns>更新后的卷。</returns>
        Task<Volume> UpdateVolumeAsync(Volume existingVolume, Volume newVolume);

        /// <summary>
        ///     删除一卷。
        /// </summary>
        /// <param name="volumeId">卷的编号。</param>
        void DeleteVolume(string volumeId);

        /// <summary>
        ///     异步删除一卷。
        /// </summary>
        /// <param name="volumeId">卷的编号。</param>
        Task DeleteVolumeAsync(string volumeId);

        #endregion
    }
}