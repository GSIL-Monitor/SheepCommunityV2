using System.Linq;
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
    ///     列举一组卷注释服务接口。
    /// </summary>
    public class ListVolumeAnnotationService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListVolumeAnnotationService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置列举一组卷注释的校验器。
        /// </summary>
        public IValidator<VolumeAnnotationList> VolumeAnnotationListValidator { get; set; }

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

        #region 列举一组卷注释

        /// <summary>
        ///     列举一组卷注释。
        /// </summary>
        [CacheResponse(Duration = 3600, MaxAge = 1800)]
        public async Task<object> Get(VolumeAnnotationList request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    VolumeAnnotationListValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var existingVolumeAnnotations = await VolumeAnnotationRepo.FindVolumeAnnotationsAsync(request.BookId, request.VolumeNumber, request.AnnotationFilter, request.OrderBy, request.Descending, request.Skip, request.Limit);
            if (existingVolumeAnnotations == null)
            {
                throw HttpError.NotFound(string.Format(Resources.VolumeAnnotationsNotFound));
            }
            var volumeAnnotationsDto = existingVolumeAnnotations.Select(volumeAnnotation => volumeAnnotation.MapToVolumeAnnotationDto()).ToList();
            return new VolumeAnnotationListResponse
                   {
                       VolumeAnnotations = volumeAnnotationsDto
                   };
        }

        #endregion
    }
}