﻿using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Common.Auth;
using Sheep.Model.Bookstore;
using Sheep.ServiceInterface.ChapterReads.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.ChapterReads;

namespace Sheep.ServiceInterface.ChapterReads
{
    /// <summary>
    ///     根据章显示最后一个阅读服务接口。
    /// </summary>
    public class ShowLastChapterReadByChapterService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowLastChapterReadByChapterService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置根据章显示最后一个阅读的校验器。
        /// </summary>
        public IValidator<ChapterReadShowLastByChapter> ChapterReadShowLastByChapterValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置阅读的存储库。
        /// </summary>
        public IChapterReadRepository ChapterReadRepo { get; set; }

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

        #endregion

        #region 根据章显示最后一个阅读

        /// <summary>
        ///     根据章显示最后一个阅读。
        /// </summary>
        public async Task<object> Get(ChapterReadShowLastByChapter request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    ChapterReadShowLastByChapterValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var currentUserId = GetSession().UserAuthId.ToInt(0);
            var existingChapterReads = await ChapterReadRepo.FindChapterReadsByChapterAsync(request.ChapterId, request.IsMine.HasValue && request.IsMine.Value ? currentUserId : (int?) null, null, "CreatedDate", true, 0, 1);
            if (existingChapterReads == null || existingChapterReads.Count == 0)
            {
                throw HttpError.NotFound(string.Format(Resources.ChapterReadsNotFound));
            }
            var existingChapterRead = existingChapterReads[0];
            var user = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(existingChapterRead.UserId.ToString());
            if (user == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, existingChapterRead.UserId));
            }
            var chapter = await ChapterRepo.GetChapterAsync(existingChapterRead.ChapterId);
            if (chapter == null)
            {
                throw HttpError.NotFound(string.Format(Resources.ChapterNotFound, existingChapterRead.ChapterId));
            }
            var book = await BookRepo.GetBookAsync(existingChapterRead.BookId);
            if (book == null)
            {
                throw HttpError.NotFound(string.Format(Resources.BookNotFound, existingChapterRead.BookId));
            }
            var volume = await VolumeRepo.GetVolumeAsync(existingChapterRead.VolumeId);
            if (volume == null)
            {
                throw HttpError.NotFound(string.Format(Resources.VolumeNotFound, existingChapterRead.VolumeId));
            }
            var chapterReadDto = existingChapterRead.MapToChapterReadDto(book, volume, chapter, user);
            return new ChapterReadShowResponse
                   {
                       ChapterRead = chapterReadDto
                   };
        }

        #endregion
    }
}