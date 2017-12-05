using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Aliyun.OSS;
using Aliyun.OSS.Common;
using Aliyun.OSS.Util;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.Extensions;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Common.Settings;
using Sheep.Model.Read;
using Sheep.Model.Read.Entities;
using Sheep.ServiceInterface.Books.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Books;

namespace Sheep.ServiceInterface.Books
{
    /// <summary>
    ///     更新一个书籍服务接口。
    /// </summary>
    public class UpdateBookService : ChangeBookService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(UpdateBookService));

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
        ///     获取及设置更新一个书籍的校验器。
        /// </summary>
        public IValidator<BookUpdate> BookUpdateValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置书籍的存储库。
        /// </summary>
        public IBookRepository BookRepo { get; set; }

        #endregion

        #region 更新一个书籍

        /// <summary>
        ///     更新一个书籍。
        /// </summary>
        public async Task<object> Put(BookUpdate request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                BookUpdateValidator.ValidateAndThrow(request, ApplyTo.Put);
            }
            var existingBook = await BookRepo.GetBookAsync(request.BookId);
            if (existingBook == null)
            {
                throw HttpError.NotFound(string.Format(Resources.BookNotFound, request.BookId));
            }
            var newBook = new Book();
            newBook.PopulateWith(existingBook);
            newBook.Meta = existingBook.Meta == null ? new Dictionary<string, string>() : new Dictionary<string, string>(existingBook.Meta);
            newBook.Title = request.Title.Replace("\"", "'");
            newBook.Summary = request.Summary.Replace("\"", "'");
            newBook.Author = request.Author;
            newBook.Tags = request.Tags.IsNullOrEmpty() ? new List<string>() : request.Tags.Replace(",", ";").Replace("，", ";").Replace("；", ";").Split(';').Select(x => x.Replace("”", string.Empty).Replace("“", string.Empty).Replace("\"", string.Empty).Trim()).ToList();
            newBook.IsPublished = request.AutoPublish ?? false;
            string pictureUrl = null;
            if (!request.SourcePictureUrl.IsNullOrEmpty())
            {
                var imageBuffer = await request.SourcePictureUrl.GetBytesFromUrlAsync();
                if (imageBuffer != null && imageBuffer.Length > 0)
                {
                    using (var imageStream = new MemoryStream(imageBuffer))
                    {
                        var md5Hash = OssUtils.ComputeContentMd5(imageStream, imageStream.Length);
                        var path = $"books/{newBook.Id}/pictures/{Guid.NewGuid():N}.{request.SourcePictureUrl.GetImageUrlExtension()}";
                        var objectMetadata = new ObjectMetadata
                                             {
                                                 ContentMd5 = md5Hash,
                                                 ContentType = request.SourcePictureUrl.GetImageUrlExtension().GetImageContentType(),
                                                 ContentLength = imageBuffer.Length,
                                                 CacheControl = "max-age=604800"
                                             };
                        try
                        {
                            await OssClient.PutObjectAsync(AppSettings.GetString(AppSettingsOssNames.OssBucket), path, imageStream, objectMetadata);
                            pictureUrl = $"{AppSettings.GetString(AppSettingsOssNames.OssUrl)}/{path}";
                        }
                        catch (OssException ex)
                        {
                            Log.WarnFormat("Failed with error code: {0}; Error info: {1}. RequestID:{2}\tHostID:{3}", ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
                            throw new HttpError(HttpStatusCode.InternalServerError, ex.ErrorCode, ex.Message);
                        }
                        catch (Exception ex)
                        {
                            Log.WarnFormat("Failed with error info: {0}", ex.Message);
                            throw new HttpError(HttpStatusCode.InternalServerError, ex.Message);
                        }
                    }
                }
            }
            else
            {
                var imageFile = Request.Files.FirstOrDefault(file => file.ContentLength > 0);
                if (imageFile != null)
                {
                    using (var imageStream = imageFile.InputStream)
                    {
                        var md5Hash = OssUtils.ComputeContentMd5(imageStream, imageStream.Length);
                        var path = $"books/{newBook.Id}/pictures/{Guid.NewGuid():N}.{imageFile.FileName.GetImageFileExtension()}";
                        var objectMetadata = new ObjectMetadata
                                             {
                                                 ContentMd5 = md5Hash,
                                                 ContentType = imageFile.ContentType,
                                                 ContentLength = imageFile.ContentLength,
                                                 CacheControl = "max-age=604800"
                                             };
                        try
                        {
                            await OssClient.PutObjectAsync(AppSettings.GetString(AppSettingsOssNames.OssBucket), path, imageStream, objectMetadata);
                            pictureUrl = $"{AppSettings.GetString(AppSettingsOssNames.OssUrl)}/{path}";
                        }
                        catch (OssException ex)
                        {
                            Log.WarnFormat("Failed with error code: {0}; Error info: {1}. RequestID:{2}\tHostID:{3}", ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
                            throw new HttpError(HttpStatusCode.InternalServerError, ex.ErrorCode, ex.Message);
                        }
                        catch (Exception ex)
                        {
                            Log.WarnFormat("Failed with error info: {0}", ex.Message);
                            throw new HttpError(HttpStatusCode.InternalServerError, ex.Message);
                        }
                    }
                }
            }
            newBook.PictureUrl = pictureUrl.IsNullOrEmpty() ? existingBook.PictureUrl : pictureUrl;
            var book = await BookRepo.UpdateBookAsync(existingBook, newBook);
            ResetCache(book);
            return new BookUpdateResponse
                   {
                       Book = book.MapToBookDto()
                   };
        }

        #endregion
    }
}