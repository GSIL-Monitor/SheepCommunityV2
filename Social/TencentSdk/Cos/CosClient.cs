using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Extensions;
using ServiceStack.Logging;
using ServiceStack.Text;

namespace Tencent.Cos
{
    /// <summary>
    ///     腾讯云对象存储服务客户端封装库。
    /// </summary>
    public class CosClient : ICosClient
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CosClient));

        #endregion

        #region 属性

        /// <summary>
        ///     项目的编号。
        /// </summary>
        public int AppId { get; set; }

        /// <summary>
        ///     签名的编号。
        /// </summary>
        public string SecretId { get; set; }

        /// <summary>
        ///     签名的密钥。
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        ///     接口的地址。
        /// </summary>
        public string ApiUrl { get; set; }

        /// <summary>
        ///     存储桶的名称。
        /// </summary>
        public string Bucket { get; set; }

        #endregion

        #region 构造器

        #endregion

        #region ICosClient 接口实现

        /// <summary>
        ///     创建文件夹。
        /// </summary>
        public CreateFolderResponse Post(string remotePath, CreateFolderRequest request)
        {
            return PostAsync(remotePath, request).GetAwaiter().GetResult();
        }

        /// <summary>
        ///     异步创建文件夹。
        /// </summary>
        public async Task<CreateFolderResponse> PostAsync(string remotePath, CreateFolderRequest request)
        {
            try
            {
                remotePath = remotePath.StandardizeRemotePath();
                if (remotePath.IsNullOrEmpty())
                {
                    return new CreateFolderResponse
                           {
                               Code = -3,
                               Message = "Remote path illegal."
                           };
                }
                var headers = new Dictionary<string, string>
                              {
                                  {
                                      "Authorization", Signer.Signature(AppId, SecretId, SecretKey, DateTime.Now.AddSeconds(300).ToUnixTime(), Bucket)
                                  }
                              };
                var responseJson = await string.Format("{0}/{1}/{2}{3}", ApiUrl, AppId, Bucket, remotePath.EncodeRemotePath()).HttpPostJsonAsync(request.ToJson(), headers);
                var response = responseJson.FromJson<CreateFolderResponse>();
                if (response != null && response.Code != 0)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}", GetType().Name, request.GetType().Name, response.Code, response.Message);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new CreateFolderResponse
                       {
                           Code = -4,
                           Message = errorMessage
                       };
            }
        }

        /// <summary>
        ///     查询文件夹的属性信息。
        /// </summary>
        public GetFolderStatResponse Get(string remotePath, GetFolderStatRequest request)
        {
            return GetAsync(remotePath, request).GetAwaiter().GetResult();
        }

        /// <summary>
        ///     异步查询文件夹的属性信息。
        /// </summary>
        public async Task<GetFolderStatResponse> GetAsync(string remotePath, GetFolderStatRequest request)
        {
            try
            {
                remotePath = remotePath.StandardizeRemotePath();
                if (remotePath.IsNullOrEmpty())
                {
                    return new GetFolderStatResponse
                           {
                               Code = -3,
                               Message = "Remote path illegal."
                           };
                }
                var headers = new Dictionary<string, string>
                              {
                                  {
                                      "Authorization", Signer.Signature(AppId, SecretId, SecretKey, DateTime.Now.AddSeconds(300).ToUnixTime(), Bucket)
                                  }
                              };
                var responseJson = await string.Format("{0}/{1}/{2}{3}?{4}", ApiUrl, AppId, Bucket, remotePath.EncodeRemotePath(), request.ToQueryString()).HttpGetAsync(headers);
                var response = responseJson.FromJson<GetFolderStatResponse>();
                if (response != null && response.Code != 0)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}", GetType().Name, request.GetType().Name, response.Code, response.Message);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new GetFolderStatResponse
                       {
                           Code = -4,
                           Message = errorMessage
                       };
            }
        }

        /// <summary>
        ///     删除文件夹。
        /// </summary>
        public DeleteFolderResponse Post(string remotePath, DeleteFolderRequest request)
        {
            return PostAsync(remotePath, request).GetAwaiter().GetResult();
        }

        /// <summary>
        ///     异步删除文件夹。
        /// </summary>
        public async Task<DeleteFolderResponse> PostAsync(string remotePath, DeleteFolderRequest request)
        {
            try
            {
                remotePath = remotePath.StandardizeRemotePath();
                if (remotePath.IsNullOrEmpty())
                {
                    return new DeleteFolderResponse
                           {
                               Code = -3,
                               Message = "Remote path illegal."
                           };
                }
                var headers = new Dictionary<string, string>
                              {
                                  {
                                      "Authorization", Signer.SignatureOnce(AppId, SecretId, SecretKey, remotePath, Bucket)
                                  }
                              };
                var responseJson = await string.Format("{0}/{1}/{2}{3}", ApiUrl, AppId, Bucket, remotePath.EncodeRemotePath()).HttpPostJsonAsync(request.ToJson(), headers);
                var response = responseJson.FromJson<DeleteFolderResponse>();
                if (response != null && response.Code != 0)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}", GetType().Name, request.GetType().Name, response.Code, response.Message);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new DeleteFolderResponse
                       {
                           Code = -4,
                           Message = errorMessage
                       };
            }
        }

        /// <summary>
        ///     上传文件。
        /// </summary>
        public UploadFileResponse Post(string remoteFilePath, UploadFileRequest request)
        {
            return PostAsync(remoteFilePath, request).GetAwaiter().GetResult();
        }

        /// <summary>
        ///     异步上传文件。
        /// </summary>
        public async Task<UploadFileResponse> PostAsync(string remoteFilePath, UploadFileRequest request)
        {
            try
            {
                if (remoteFilePath.EndsWith("/"))
                {
                    return new UploadFileResponse
                           {
                               Code = -3,
                               Message = "Remote file path cannot end with '/'."
                           };
                }
                var headers = new Dictionary<string, string>
                              {
                                  {
                                      "Authorization", Signer.Signature(AppId, SecretId, SecretKey, DateTime.Now.AddSeconds(300).ToUnixTime(), Bucket)
                                  }
                              };
                var responseJson = await string.Format("{0}/{1}/{2}{3}", ApiUrl, AppId, Bucket, remoteFilePath.EncodeRemotePath()).HttpPostFileAsync(request.ToDictionary(), request.FileContent, remoteFilePath.GetFileName(), headers);
                var response = responseJson.FromJson<UploadFileResponse>();
                if (response != null && response.Code != 0)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}", GetType().Name, request.GetType().Name, response.Code, response.Message);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new UploadFileResponse
                       {
                           Code = -4,
                           Message = errorMessage
                       };
            }
        }

        #endregion
    }
}