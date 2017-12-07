using System.Threading.Tasks;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Model.Read;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Subjects;

namespace Sheep.ServiceInterface.Subjects
{
    /// <summary>
    ///     删除一个主题服务接口。
    /// </summary>
    public class DeleteSubjectService : ChangeSubjectService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(DeleteSubjectService));

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
        ///     获取及设置删除一个主题的校验器。
        /// </summary>
        public IValidator<SubjectDelete> SubjectDeleteValidator { get; set; }

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
        ///     获取及设置主题的存储库。
        /// </summary>
        public ISubjectRepository SubjectRepo { get; set; }

        #endregion

        #region 删除一个主题

        /// <summary>
        ///     删除一个主题。
        /// </summary>
        public async Task<object> Delete(SubjectDelete request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                SubjectDeleteValidator.ValidateAndThrow(request, ApplyTo.Delete);
            }
            var existingSubject = await SubjectRepo.GetSubjectAsync(request.BookId, request.VolumeNumber, request.SubjectNumber);
            if (existingSubject == null)
            {
                throw HttpError.NotFound(string.Format(Resources.SubjectNotFound, string.Format("{0}-{1}-{2}", request.BookId, request.VolumeNumber, request.SubjectNumber)));
            }
            await SubjectRepo.DeleteSubjectAsync(existingSubject.Id);
            await VolumeRepo.IncrementVolumeSubjectsCountAsync(existingSubject.VolumeId, -1);
            ResetCache(existingSubject);
            return new SubjectDeleteResponse();
        }

        #endregion
    }
}