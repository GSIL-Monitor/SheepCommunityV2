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
    ///     更新一个卷服务接口。
    /// </summary>
    public class UpdateVolumeService : ChangeVolumeService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(UpdateVolumeService));

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
        ///     获取及设置更新一个卷的校验器。
        /// </summary>
        public IValidator<VolumeUpdate> VolumeUpdateValidator { get; set; }

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

        #region 更新一个卷

        /// <summary>
        ///     更新一个卷。
        /// </summary>
        public async Task<object> Put(VolumeUpdate request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                VolumeUpdateValidator.ValidateAndThrow(request, ApplyTo.Put);
            }
            var existingVolume = await VolumeRepo.GetVolumeAsync(request.BookId, request.VolumeNumber);
            if (existingVolume == null)
            {
                throw HttpError.NotFound(string.Format(Resources.VolumeNotFound, string.Format("{0}-{1}", request.BookId, request.VolumeNumber)));
            }
            var newVolume = new Volume();
            newVolume.PopulateWith(existingVolume);
            newVolume.Meta = existingVolume.Meta == null ? new Dictionary<string, string>() : new Dictionary<string, string>(existingVolume.Meta);
            newVolume.Title = request.Title.Replace("\"", "'");
            newVolume.Abbreviation = request.Abbreviation;
            var volume = await VolumeRepo.UpdateVolumeAsync(existingVolume, newVolume);
            var volumeAnnotations = await VolumeAnnotationRepo.FindVolumeAnnotationsByVolumeAsync(existingVolume.Id, null, null, null, null);
            ResetCache(volume);
            return new VolumeUpdateResponse
                   {
                       Volume = volume.MapToVolumeDto(volumeAnnotations)
                   };
        }

        #endregion
    }
}