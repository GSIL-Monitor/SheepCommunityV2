using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Model.Read;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceInterface.Volumes.Mappers;
using Sheep.ServiceModel.Volumes;

namespace Sheep.ServiceInterface.Volumes
{
    /// <summary>
    ///     列举一组卷服务接口。
    /// </summary>
    public class ListVolumeService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListVolumeService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置列举一组卷的校验器。
        /// </summary>
        public IValidator<VolumeList> VolumeListValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置书籍的存储库。
        /// </summary>
        public IBookRepository BookRepo { get; set; }

        /// <summary>
        ///     获取及设置卷的存储库。
        /// </summary>
        public IVolumeRepository VolumeRepo { get; set; }

        /// <summary>
        ///     获取及设置卷注释的存储库。
        /// </summary>
        public IVolumeAnnotationRepository VolumeAnnotationRepo { get; set; }

        #endregion

        #region 列举一组卷

        /// <summary>
        ///     列举一组卷。
        /// </summary>
        [CacheResponse(Duration = 600)]
        public async Task<object> Get(VolumeList request)
        {
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                VolumeListValidator.ValidateAndThrow(request, ApplyTo.Get);
            }
            var existingVolumes = await VolumeRepo.FindVolumesAsync(request.BookId, request.TitleFilter, request.OrderBy, request.Descending, request.Skip, request.Limit);
            if (existingVolumes == null)
            {
                throw HttpError.NotFound(string.Format(Resources.VolumesNotFound));
            }
            var volumeAnnotationsMap = (await VolumeAnnotationRepo.FindVolumeAnnotationsByVolumesAsync(existingVolumes.Select(volume => volume.Id), "VolumeId", null, null, null)).GroupBy(volumeAnnotation => volumeAnnotation.VolumeId, volumeAnnotation => volumeAnnotation).ToDictionary(grouping => grouping.Key, grouping => grouping.OrderBy(g => g.Number).ToList());
            var volumesDto = existingVolumes.Select(volume => volume.MapToVolumeDto(volumeAnnotationsMap.GetValueOrDefault(volume.Id))).ToList();
            return new VolumeListResponse
                   {
                       Volumes = volumesDto
                   };
        }

        #endregion
    }
}