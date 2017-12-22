using System.Collections.Generic;
using System.Threading.Tasks;
using Aliyun.OSS;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Model.Bookstore;
using Sheep.Model.Bookstore.Entities;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceInterface.Volumes.Mappers;
using Sheep.ServiceModel.Volumes;

namespace Sheep.ServiceInterface.Volumes
{
    /// <summary>
    ///     更新一条卷注释服务接口。
    /// </summary>
    public class UpdateVolumeAnnotationService : ChangeVolumeAnnotationService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(UpdateVolumeAnnotationService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置阿里云对象存储客户端。
        /// </summary>
        public IOss OssClient { get; set; }

        /// <summary>
        ///     网易云通信服务客户端。
        /// </summary>
        public INimClient NimClient { get; set; }

        /// <summary>
        ///     获取及设置更新一条卷注释的校验器。
        /// </summary>
        public IValidator<VolumeAnnotationUpdate> VolumeAnnotationUpdateValidator { get; set; }

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

        #region 更新一条卷注释

        /// <summary>
        ///     更新一条卷注释。
        /// </summary>
        public async Task<object> Put(VolumeAnnotationUpdate request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                VolumeAnnotationUpdateValidator.ValidateAndThrow(request, ApplyTo.Put);
            }
            var existingVolumeAnnotation = await VolumeAnnotationRepo.GetVolumeAnnotationAsync(request.BookId, request.VolumeNumber, request.AnnotationNumber);
            if (existingVolumeAnnotation == null)
            {
                throw HttpError.NotFound(string.Format(Resources.VolumeAnnotationNotFound, string.Format("{0}-{1}-{2}", request.BookId, request.VolumeNumber, request.AnnotationNumber)));
            }
            var newVolumeAnnotation = new VolumeAnnotation();
            newVolumeAnnotation.PopulateWith(existingVolumeAnnotation);
            newVolumeAnnotation.Meta = existingVolumeAnnotation.Meta == null ? new Dictionary<string, string>() : new Dictionary<string, string>(existingVolumeAnnotation.Meta);
            newVolumeAnnotation.Title = request.Title?.Replace("\"", "'");
            newVolumeAnnotation.Annotation = request.Annotation?.Replace("\"", "'");
            var volumeAnnotation = await VolumeAnnotationRepo.UpdateVolumeAnnotationAsync(existingVolumeAnnotation, newVolumeAnnotation);
            ResetCache(volumeAnnotation);
            return new VolumeAnnotationUpdateResponse
                   {
                       VolumeAnnotation = volumeAnnotation.MapToVolumeAnnotationDto()
                   };
        }

        #endregion
    }
}