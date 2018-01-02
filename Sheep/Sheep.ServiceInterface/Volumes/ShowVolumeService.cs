using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Model.Bookstore;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceInterface.Volumes.Mappers;
using Sheep.ServiceModel.Volumes;

namespace Sheep.ServiceInterface.Volumes
{
    /// <summary>
    ///     显示一卷服务接口。
    /// </summary>
    public class ShowVolumeService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowVolumeService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置显示一卷的校验器。
        /// </summary>
        public IValidator<VolumeShow> VolumeShowValidator { get; set; }

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

        #region 显示一卷

        /// <summary>
        ///     显示一卷。
        /// </summary>
        [CacheResponse(Duration = 3600, MaxAge = 1800)]
        public async Task<object> Get(VolumeShow request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    VolumeShowValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var existingVolume = await VolumeRepo.GetVolumeAsync(request.BookId, request.VolumeNumber);
            if (existingVolume == null)
            {
                throw HttpError.NotFound(string.Format(Resources.VolumeNotFound, string.Format("{0}-{1}", request.BookId, request.VolumeNumber)));
            }
            var volumeAnnotations = await VolumeAnnotationRepo.FindVolumeAnnotationsByVolumeAsync(existingVolume.Id, null, null, null, null);
            var volumeDto = existingVolume.MapToVolumeDto(volumeAnnotations);
            return new VolumeShowResponse
                   {
                       Volume = volumeDto
                   };
        }

        #endregion
    }
}