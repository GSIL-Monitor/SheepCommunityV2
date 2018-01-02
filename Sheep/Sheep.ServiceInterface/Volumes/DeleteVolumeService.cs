using System.Threading.Tasks;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Model.Bookstore;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Volumes;

namespace Sheep.ServiceInterface.Volumes
{
    /// <summary>
    ///     删除一卷服务接口。
    /// </summary>
    public class DeleteVolumeService : ChangeVolumeService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(DeleteVolumeService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     网易云通信服务客户端。
        /// </summary>
        public INimClient NimClient { get; set; }

        /// <summary>
        ///     获取及设置删除一卷的校验器。
        /// </summary>
        public IValidator<VolumeDelete> VolumeDeleteValidator { get; set; }

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

        #region 删除一卷

        /// <summary>
        ///     删除一卷。
        /// </summary>
        public async Task<object> Delete(VolumeDelete request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    VolumeDeleteValidator.ValidateAndThrow(request, ApplyTo.Delete);
            //}
            var existingVolume = await VolumeRepo.GetVolumeAsync(request.BookId, request.VolumeNumber);
            if (existingVolume == null)
            {
                throw HttpError.NotFound(string.Format(Resources.VolumeNotFound, string.Format("{0}-{1}", request.BookId, request.VolumeNumber)));
            }
            await VolumeRepo.DeleteVolumeAsync(existingVolume.Id);
            await BookRepo.IncrementBookVolumesCountAsync(existingVolume.BookId, -1);
            ResetCache(existingVolume);
            return new VolumeDeleteResponse();
        }

        #endregion
    }
}