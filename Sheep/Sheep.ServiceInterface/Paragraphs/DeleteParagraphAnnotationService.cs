﻿using System.Threading.Tasks;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Model.Bookstore;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Paragraphs;

namespace Sheep.ServiceInterface.Paragraphs
{
    /// <summary>
    ///     删除一条节注释服务接口。
    /// </summary>
    public class DeleteParagraphAnnotationService : ChangeParagraphAnnotationService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(DeleteParagraphAnnotationService));

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
        ///     获取及设置删除一条节注释的校验器。
        /// </summary>
        public IValidator<ParagraphAnnotationDelete> ParagraphAnnotationDeleteValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置书籍的存储库。
        /// </summary>
        public IBookRepository BookRepo { get; set; }

        /// <summary>
        ///     获取及设置节的存储库。
        /// </summary>
        public IParagraphRepository ParagraphRepo { get; set; }

        /// <summary>
        ///     获取及设置节注释的存储库。
        /// </summary>
        public IParagraphAnnotationRepository ParagraphAnnotationRepo { get; set; }

        #endregion

        #region 删除一条节注释

        /// <summary>
        ///     删除一条节注释。
        /// </summary>
        public async Task<object> Delete(ParagraphAnnotationDelete request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    ParagraphAnnotationDeleteValidator.ValidateAndThrow(request, ApplyTo.Delete);
            //}
            var existingParagraphAnnotation = await ParagraphAnnotationRepo.GetParagraphAnnotationAsync(request.BookId, request.VolumeNumber, request.ChapterNumber, request.ParagraphNumber, request.AnnotationNumber);
            if (existingParagraphAnnotation == null)
            {
                throw HttpError.NotFound(string.Format(Resources.ParagraphAnnotationNotFound, string.Format("{0}-{1}-{2}-{3}-{4}", request.BookId, request.VolumeNumber, request.ChapterNumber, request.ParagraphNumber, request.AnnotationNumber)));
            }
            await ParagraphAnnotationRepo.DeleteParagraphAnnotationAsync(existingParagraphAnnotation.Id);
            ResetCache(existingParagraphAnnotation);
            return new ParagraphAnnotationDeleteResponse();
        }

        #endregion
    }
}