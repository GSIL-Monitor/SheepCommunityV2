using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Model.Bookstore;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceInterface.Subjects.Mappers;
using Sheep.ServiceModel.Subjects;

namespace Sheep.ServiceInterface.Subjects
{
    /// <summary>
    ///     列举一组主题服务接口。
    /// </summary>
    public class ListSubjectService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListSubjectService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置列举一组主题的校验器。
        /// </summary>
        public IValidator<SubjectList> SubjectListValidator { get; set; }

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

        #region 列举一组主题

        /// <summary>
        ///     列举一组主题。
        /// </summary>
        [CacheResponse(Duration = 31536000, MaxAge = 86400)]
        public async Task<object> Get(SubjectList request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    SubjectListValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var existingSubjects = await SubjectRepo.FindSubjectsAsync(request.BookId, request.VolumeNumber, request.TitleFilter, request.OrderBy, request.Descending, request.Skip, request.Limit);
            if (existingSubjects == null)
            {
                throw HttpError.NotFound(string.Format(Resources.SubjectsNotFound));
            }
            var subjectsDto = existingSubjects.Select(subject => subject.MapToSubjectDto()).ToList();
            return new SubjectListResponse
                   {
                       Subjects = subjectsDto
                   };
        }

        #endregion
    }
}