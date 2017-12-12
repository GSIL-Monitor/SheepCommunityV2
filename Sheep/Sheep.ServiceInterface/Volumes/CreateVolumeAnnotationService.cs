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
using Sheep.Model.Read;
using Sheep.Model.Read.Entities;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceInterface.Volumes.Mappers;
using Sheep.ServiceModel.Volumes;

namespace Sheep.ServiceInterface.Volumes
{
    /// <summary>
    ///     新建一条卷注释服务接口。
    /// </summary>
    public class CreateVolumeAnnotationService : ChangeVolumeAnnotationService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CreateVolumeAnnotationService));

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
        ///     获取及设置新建一条卷注释的校验器。
        /// </summary>
        public IValidator<VolumeAnnotationCreate> VolumeAnnotationCreateValidator { get; set; }

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

        #region 新建一条卷注释

        /// <summary>
        ///     新建一条卷注释。
        /// </summary>
        public async Task<object> Post(VolumeAnnotationCreate request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                VolumeAnnotationCreateValidator.ValidateAndThrow(request, ApplyTo.Post);
            }
            var existingVolumeAnnotation = await VolumeAnnotationRepo.GetVolumeAnnotationAsync(request.BookId, request.VolumeNumber, request.AnnotationNumber);
            if (existingVolumeAnnotation != null)
            {
                return new VolumeAnnotationCreateResponse
                       {
                           VolumeAnnotation = existingVolumeAnnotation.MapToVolumeAnnotationDto()
                       };
            }
            var existingVolume = await VolumeRepo.GetVolumeAsync(request.BookId, request.VolumeNumber);
            if (existingVolume == null)
            {
                throw HttpError.NotFound(string.Format(Resources.VolumeNotFound, string.Format("{0}-{1}", request.BookId, request.VolumeNumber)));
            }
            var newVolumeAnnotation = new VolumeAnnotation
                                      {
                                          Meta = new Dictionary<string, string>(),
                                          BookId = existingVolume.BookId,
                                          VolumeId = existingVolume.Id,
                                          VolumeNumber = existingVolume.Number,
                                          Number = request.AnnotationNumber,
                                          Title = request.Title?.Replace("\"", "'"),
                                          Annotation = request.Annotation?.Replace("\"", "'")
                                      };
            var volumeAnnotation = await VolumeAnnotationRepo.CreateVolumeAnnotationAsync(newVolumeAnnotation);
            ResetCache(volumeAnnotation);
            return new VolumeAnnotationCreateResponse
                   {
                       VolumeAnnotation = volumeAnnotation.MapToVolumeAnnotationDto()
                   };
        }

        #endregion
    }
}