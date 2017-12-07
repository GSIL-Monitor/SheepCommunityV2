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
using Sheep.ServiceInterface.Subjects.Mappers;
using Sheep.ServiceModel.Subjects;

namespace Sheep.ServiceInterface.Subjects
{
    /// <summary>
    ///     更新一个主题服务接口。
    /// </summary>
    public class UpdateSubjectService : ChangeSubjectService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(UpdateSubjectService));

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
        ///     获取及设置更新一个主题的校验器。
        /// </summary>
        public IValidator<SubjectUpdate> SubjectUpdateValidator { get; set; }

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

        #region 更新一个主题

        /// <summary>
        ///     更新一个主题。
        /// </summary>
        public async Task<object> Put(SubjectUpdate request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                SubjectUpdateValidator.ValidateAndThrow(request, ApplyTo.Put);
            }
            var existingSubject = await SubjectRepo.GetSubjectAsync(request.BookId, request.VolumeNumber, request.SubjectNumber);
            if (existingSubject == null)
            {
                throw HttpError.NotFound(string.Format(Resources.SubjectNotFound, string.Format("{0}-{1}-{2}", request.BookId, request.VolumeNumber, request.SubjectNumber)));
            }
            var newSubject = new Subject();
            newSubject.PopulateWith(existingSubject);
            newSubject.Meta = existingSubject.Meta == null ? new Dictionary<string, string>() : new Dictionary<string, string>(existingSubject.Meta);
            newSubject.Title = request.Title.Replace("\"", "'");
            var subject = await SubjectRepo.UpdateSubjectAsync(existingSubject, newSubject);
            ResetCache(subject);
            return new SubjectUpdateResponse
                   {
                       Subject = subject.MapToSubjectDto()
                   };
        }

        #endregion
    }
}