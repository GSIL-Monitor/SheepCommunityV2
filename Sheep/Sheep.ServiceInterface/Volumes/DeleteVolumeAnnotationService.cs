using System.Threading.Tasks;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Model.Bookstore;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Volumes;

namespace Sheep.ServiceInterface.Volumes
{
    /// <summary>
    ///     删除一条卷注释服务接口。
    /// </summary>
    public class DeleteVolumeAnnotationService : ChangeVolumeAnnotationService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(DeleteVolumeAnnotationService));

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
        ///     获取及设置删除一条卷注释的校验器。
        /// </summary>
        public IValidator<VolumeAnnotationDelete> VolumeAnnotationDeleteValidator { get; set; }

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

        #region 删除一条卷注释

        /// <summary>
        ///     删除一条卷注释。
        /// </summary>
        public async Task<object> Delete(VolumeAnnotationDelete request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                VolumeAnnotationDeleteValidator.ValidateAndThrow(request, ApplyTo.Delete);
            }
            var existingVolumeAnnotation = await VolumeAnnotationRepo.GetVolumeAnnotationAsync(request.BookId, request.VolumeNumber, request.AnnotationNumber);
            if (existingVolumeAnnotation == null)
            {
                throw HttpError.NotFound(string.Format(Resources.VolumeAnnotationNotFound, string.Format("{0}-{1}-{2}", request.BookId, request.VolumeNumber, request.AnnotationNumber)));
            }
            await VolumeAnnotationRepo.DeleteVolumeAnnotationAsync(existingVolumeAnnotation.Id);
            ResetCache(existingVolumeAnnotation);
            return new VolumeAnnotationDeleteResponse();
        }

        #endregion
    }
}