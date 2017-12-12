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
using Sheep.ServiceInterface.Chapters.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Chapters;

namespace Sheep.ServiceInterface.Chapters
{
    /// <summary>
    ///     新建一章服务接口。
    /// </summary>
    public class CreateChapterService : ChangeChapterService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CreateChapterService));

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
        ///     获取及设置新建一章的校验器。
        /// </summary>
        public IValidator<ChapterCreate> ChapterCreateValidator { get; set; }

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
        ///     获取及设置章的存储库。
        /// </summary>
        public IChapterRepository ChapterRepo { get; set; }

        /// <summary>
        ///     获取及设置章注释的存储库。
        /// </summary>
        public IChapterAnnotationRepository ChapterAnnotationRepo { get; set; }

        #endregion

        #region 新建一章

        /// <summary>
        ///     新建一章。
        /// </summary>
        public async Task<object> Post(ChapterCreate request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                ChapterCreateValidator.ValidateAndThrow(request, ApplyTo.Post);
            }
            var existingChapter = await ChapterRepo.GetChapterAsync(request.BookId, request.VolumeNumber, request.ChapterNumber);
            if (existingChapter != null)
            {
                var chapterAnnotations = await ChapterAnnotationRepo.FindChapterAnnotationsByChapterAsync(existingChapter.Id, null, null, null, null);
                return new ChapterCreateResponse
                       {
                           Chapter = existingChapter.MapToChapterDto(chapterAnnotations)
                       };
            }
            var existingVolume = await VolumeRepo.GetVolumeAsync(request.BookId, request.VolumeNumber);
            if (existingVolume == null)
            {
                throw HttpError.NotFound(string.Format(Resources.VolumeNotFound, string.Format("{0}-{1}", request.BookId, request.VolumeNumber)));
            }
            var newChapter = new Chapter
                             {
                                 Meta = new Dictionary<string, string>(),
                                 BookId = existingVolume.BookId,
                                 VolumeId = existingVolume.Id,
                                 VolumeNumber = existingVolume.Number,
                                 Number = request.ChapterNumber,
                                 Title = request.Title?.Replace("\"", "'"),
                                 Content = request.Content?.Replace("\"", "'")
                             };
            var chapter = await ChapterRepo.CreateChapterAsync(newChapter);
            await VolumeRepo.IncrementVolumeChaptersCountAsync(chapter.VolumeId, 1);
            ResetCache(chapter);
            return new ChapterCreateResponse
                   {
                       Chapter = chapter.MapToChapterDto(new List<ChapterAnnotation>())
                   };
        }

        #endregion
    }
}