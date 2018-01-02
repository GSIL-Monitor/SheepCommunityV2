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
    ///     显示一条卷注释服务接口。
    /// </summary>
    public class ShowVolumeAnnotationService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowVolumeAnnotationService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置显示一条卷注释的校验器。
        /// </summary>
        public IValidator<VolumeAnnotationShow> VolumeAnnotationShowValidator { get; set; }

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

        #region 显示一条卷注释

        /// <summary>
        ///     显示一条卷注释。
        /// </summary>
        [CacheResponse(Duration = 31536000, MaxAge = 86400)]
        public async Task<object> Get(VolumeAnnotationShow request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    VolumeAnnotationShowValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var existingVolumeAnnotation = await VolumeAnnotationRepo.GetVolumeAnnotationAsync(request.BookId, request.VolumeNumber, request.AnnotationNumber);
            if (existingVolumeAnnotation == null)
            {
                throw HttpError.NotFound(string.Format(Resources.VolumeAnnotationNotFound, string.Format("{0}-{1}-{2}", request.BookId, request.VolumeNumber, request.AnnotationNumber)));
            }
            var volumeAnnotationDto = existingVolumeAnnotation.MapToVolumeAnnotationDto();
            return new VolumeAnnotationShowResponse
                   {
                       VolumeAnnotation = volumeAnnotationDto
                   };
        }

        #endregion
    }
}