using System;
using System.Linq;
using System.Threading.Tasks;
using Aliyun.OSS;
using Aliyun.OSS.Common;
using Aliyun.OSS.Util;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.Extensions;
using ServiceStack.Logging;
using Sheep.Common.Settings;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Files;

namespace Sheep.ServiceInterface.Files
{
    /// <summary>
    ///     上传图像服务接口。
    /// </summary>
    public class UploadFileImageService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(UploadFileImageService));

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

        #endregion

        #region 上传图像

        /// <summary>
        ///     上传图像。
        /// </summary>
        public async Task<object> Any(FileUploadImage request)
        {
            var imageFile = Request.Files.FirstOrDefault(file => file.ContentLength > 0);
            if (imageFile != null)
            {
                using (var imageStream = imageFile.InputStream)
                {
                    var md5Hash = OssUtils.ComputeContentMd5(imageStream, imageStream.Length);
                    var path = $"files/images/{Guid.NewGuid():N}.{imageFile.FileName.GetImageFileExtension()}";
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
                        var imageUrl = $"{AppSettings.GetString(AppSettingsOssNames.OssUrl)}/{path}";
                        return new FileUploadImageResponse
                               {
                                   Status = 1,
                                   Url = imageUrl
                               };
                    }
                    catch (OssException ex)
                    {
                        Log.WarnFormat("Failed with error code: {0}; Error info: {1}. RequestID:{2}\tHostID:{3}", ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
                        return new FileUploadImageResponse
                               {
                                   Status = 0,
                                   Message = ex.Message
                               };
                    }
                    catch (Exception ex)
                    {
                        Log.WarnFormat("Failed with error info: {0}", ex.Message);
                        return new FileUploadImageResponse
                               {
                                   Status = 0,
                                   Message = ex.Message
                               };
                    }
                }
            }
            return new FileUploadImageResponse
                   {
                       Status = 0,
                       Message = Resources.ImageFileNotFound
                   };
        }
    }

    #endregion
}