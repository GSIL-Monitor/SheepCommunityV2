using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Model.Bookstore;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceInterface.Subjects.Mappers;
using Sheep.ServiceModel.Subjects;

namespace Sheep.ServiceInterface.Subjects
{
    /// <summary>
    ///     显示一个主题服务接口。
    /// </summary>
    public class ShowSubjectService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowSubjectService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置显示一个主题的校验器。
        /// </summary>
        public IValidator<SubjectShow> SubjectShowValidator { get; set; }

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

        #region 显示一个主题

        /// <summary>
        ///     显示一个主题。
        /// </summary>
        [CacheResponse(Duration = 600)]
        public async Task<object> Get(SubjectShow request)
        {
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                SubjectShowValidator.ValidateAndThrow(request, ApplyTo.Get);
            }
            var existingSubject = await SubjectRepo.GetSubjectAsync(request.BookId, request.VolumeNumber, request.SubjectNumber);
            if (existingSubject == null)
            {
                throw HttpError.NotFound(string.Format(Resources.SubjectNotFound, string.Format("{0}-{1}-{2}", request.BookId, request.VolumeNumber, request.SubjectNumber)));
            }
            var subjectDto = existingSubject.MapToSubjectDto();
            return new SubjectShowResponse
                   {
                       Subject = subjectDto
                   };
        }

        #endregion
    }
}