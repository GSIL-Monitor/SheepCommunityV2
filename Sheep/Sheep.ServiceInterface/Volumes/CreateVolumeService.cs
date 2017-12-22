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
    ///     新建一卷服务接口。
    /// </summary>
    public class CreateVolumeService : ChangeVolumeService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CreateVolumeService));

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
        ///     获取及设置新建一卷的校验器。
        /// </summary>
        public IValidator<VolumeCreate> VolumeCreateValidator { get; set; }

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

        #region 新建一卷

        /// <summary>
        ///     新建一卷。
        /// </summary>
        public async Task<object> Post(VolumeCreate request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                VolumeCreateValidator.ValidateAndThrow(request, ApplyTo.Post);
            }
            var existingVolume = await VolumeRepo.GetVolumeAsync(request.BookId, request.VolumeNumber);
            if (existingVolume != null)
            {
                var volumeAnnotations = await VolumeAnnotationRepo.FindVolumeAnnotationsByVolumeAsync(existingVolume.Id, null, null, null, null);
                return new VolumeCreateResponse
                       {
                           Volume = existingVolume.MapToVolumeDto(volumeAnnotations)
                       };
            }
            var newVolume = new Volume
                            {
                                Meta = new Dictionary<string, string>(),
                                BookId = request.BookId,
                                Number = request.VolumeNumber,
                                Title = request.Title?.Replace("\"", "'"),
                                Abbreviation = request.Abbreviation?.Replace("\"", "'")
                            };
            var volume = await VolumeRepo.CreateVolumeAsync(newVolume);
            await BookRepo.IncrementBookVolumesCountAsync(volume.BookId, 1);
            ResetCache(volume);
            return new VolumeCreateResponse
                   {
                       Volume = volume.MapToVolumeDto(new List<VolumeAnnotation>())
                   };
        }

        #endregion
    }
}