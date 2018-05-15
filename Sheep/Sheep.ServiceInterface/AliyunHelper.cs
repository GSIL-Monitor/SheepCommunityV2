using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Aliyun.Green;
using Aliyun.OSS;
using Aliyun.OSS.Util;
using ServiceStack.Extensions;

namespace Sheep.ServiceInterface
{
    /// <summary>
    ///     阿里云帮助程序类。
    /// </summary>
    public static class AliyunHelper
    {
        #region 安全检测

        /// <summary>
        ///     检测指定图片是否合法。
        /// </summary>
        /// <param name="greenClient">阿里云内容安全服务客户端</param>
        /// <param name="url">要检测的图片地址</param>
        /// <param name="cancellationToken">取消标识</param>
        /// <returns>True 表示已通过检测，False 表示未通过。</returns>
        public static async Task<bool> IsImageValidAsync(IGreenClient greenClient, string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            Arguments.NotNull(greenClient, nameof(greenClient));
            Arguments.NotNullOrEmpty(url, nameof(url));
            var request = new ImageScanRequest
                          {
                              Scenes = new[]
                                       {
                                           "porn",
                                           "terrorism",
                                           "sface",
                                           "ad",
                                           "live"
                                       },
                              Tasks = new[]
                                      {
                                          new ImageScanRequestTask
                                          {
                                              DataId = Guid.NewGuid().ToString("D"),
                                              Url = url,
                                              Time = DateTimeOffset.Now.ToUnixTimeMilliseconds()
                                          }
                                      }
                          };
            var response = await greenClient.PostAsync(request, null, cancellationToken);
            if (response.Code != 200)
            {
                return false;
            }
            foreach (var data in response.Data)
            {
                if (data.Code != 200)
                {
                    return false;
                }
                foreach (var result in data.Results)
                {
                    if (result.Suggestion == "block")
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        ///     检测指定文本是否合法。
        /// </summary>
        /// <param name="greenClient">阿里云内容安全服务客户端</param>
        /// <param name="text">要检测的文本</param>
        /// <param name="category">内容类别</param>
        /// <param name="action">操作类型</param>
        /// <param name="cancellationToken">取消标识</param>
        /// <returns>True 表示已通过检测，False 表示未通过。</returns>
        public static async Task<bool> IsTextValidAsync(IGreenClient greenClient, string text, string category = null, string action = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Arguments.NotNull(greenClient, nameof(greenClient));
            Arguments.WithinLength(text, nameof(text), 1, 4000);
            var request = new TextScanRequest
                          {
                              Scenes = new[]
                                       {
                                           "antispam"
                                       },
                              Tasks = new[]
                                      {
                                          new TextScanRequestTask
                                          {
                                              DataId = Guid.NewGuid().ToString("D"),
                                              Content = text,
                                              Time = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                                              Category = category,
                                              Action = action
                                          }
                                      }
                          };
            var response = await greenClient.PostAsync(request, null, cancellationToken);
            if (response.Code != 200)
            {
                return false;
            }
            foreach (var data in response.Data)
            {
                if (data.Code != 200)
                {
                    return false;
                }
                foreach (var result in data.Results)
                {
                    if (result.Suggestion == "block")
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        #endregion

        #region 对象存储

        /// <summary>
        ///     上传图片。
        /// </summary>
        /// <param name="ossClient">阿里云对象存储服务客户端</param>
        /// <param name="ossBucket">阿里云对象存储的存储桶</param>
        /// <param name="ossPath">阿里云对象存储的的路径</param>
        /// <param name="ossDefaultPath">阿里云对象存储的的默认路径</param>
        /// <param name="contentType">文件的类型</param>
        /// <param name="content">文件的二进制内容</param>
        /// <param name="cancellationToken">取消标识</param>
        /// <returns>上传成功或失败的路径。</returns>
        public static async Task<string> UploadFileAsync(IOss ossClient, string ossBucket, string ossPath, string ossDefaultPath, string contentType, byte[] content, CancellationToken cancellationToken = default(CancellationToken))
        {
            Arguments.NotNull(ossClient, nameof(ossClient));
            Arguments.NotNullOrEmpty(ossBucket, nameof(ossBucket));
            Arguments.NotNullOrEmpty(ossPath, nameof(ossPath));
            Arguments.NotNullOrEmpty(ossDefaultPath, nameof(ossDefaultPath));
            Arguments.NotNullOrEmpty(contentType, nameof(contentType));
            Arguments.NotNull(content, nameof(content));
            string filePath;
            using (var stream = new MemoryStream(content))
            {
                var contentMd5 = OssUtils.ComputeContentMd5(stream, stream.Length);
                var metadata = new ObjectMetadata
                               {
                                   ContentMd5 = contentMd5,
                                   ContentType = contentType,
                                   ContentLength = content.Length,
                                   CacheControl = "max-age=604800"
                               };
                try
                {
                    await ossClient.PutObjectAsync(ossBucket, ossPath, stream, metadata, cancellationToken);
                    filePath = ossPath;
                }
                catch (Exception)
                {
                    filePath = ossDefaultPath;
                }
            }
            return filePath;
        }

        #endregion
    }
}