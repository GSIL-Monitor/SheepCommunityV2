using System.Collections.Generic;
using System.Threading.Tasks;
using Aliyun.OSS;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Model.Bookstore;
using Sheep.Model.Bookstore.Entities;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceInterface.Subjects.Mappers;
using Sheep.ServiceModel.Subjects;

namespace Sheep.ServiceInterface.Subjects
{
    /// <summary>
    ///     新建一个主题服务接口。
    /// </summary>
    public class CreateSubjectService : ChangeSubjectService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CreateSubjectService));

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
        ///     获取及设置新建一个主题的校验器。
        /// </summary>
        public IValidator<SubjectCreate> SubjectCreateValidator { get; set; }

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

        #region 新建一个主题

        /// <summary>
        ///     新建一个主题。
        /// </summary>
        public async Task<object> Post(SubjectCreate request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    SubjectCreateValidator.ValidateAndThrow(request, ApplyTo.Post);
            //}
            var existingSubject = await SubjectRepo.GetSubjectAsync(request.BookId, request.VolumeNumber, request.SubjectNumber);
            if (existingSubject != null)
            {
                return new SubjectCreateResponse
                       {
                           Subject = existingSubject.MapToSubjectDto()
                       };
            }
            var existingVolume = await VolumeRepo.GetVolumeAsync(request.BookId, request.VolumeNumber);
            if (existingVolume == null)
            {
                throw HttpError.NotFound(string.Format(Resources.VolumeNotFound, string.Format("{0}-{1}", request.BookId, request.VolumeNumber)));
            }
            var newSubject = new Subject
                             {
                                 Meta = new Dictionary<string, string>(),
                                 BookId = existingVolume.BookId,
                                 VolumeId = existingVolume.Id,
                                 VolumeNumber = existingVolume.Number,
                                 Number = request.SubjectNumber,
                                 Title = request.Title?.Replace("\"", "'")
                             };
            var subject = await SubjectRepo.CreateSubjectAsync(newSubject);
            await VolumeRepo.IncrementVolumeSubjectsCountAsync(subject.VolumeId, 1);
            ResetCache(subject);
            return new SubjectCreateResponse
                   {
                       Subject = subject.MapToSubjectDto()
                   };
        }

        #endregion
    }
}