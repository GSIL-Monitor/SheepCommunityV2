using System.Threading.Tasks;

namespace Tencent.Cos
{
    /// <summary>
    ///     腾讯云对象存储服务客户端封装库的接口定义。
    /// </summary>
    public interface ICosClient
    {
        #region 文件夹

        /// <summary>
        ///     创建文件夹。
        /// </summary>
        CreateFolderResponse Post(string remotePath, CreateFolderRequest request);

        /// <summary>
        ///     异步创建文件夹。
        /// </summary>
        Task<CreateFolderResponse> PostAsync(string remotePath, CreateFolderRequest request);

        /// <summary>
        ///     查询文件夹的属性信息。
        /// </summary>
        GetFolderStatResponse Get(string remotePath, GetFolderStatRequest request);

        /// <summary>
        ///     异步查询文件夹的属性信息。
        /// </summary>
        Task<GetFolderStatResponse> GetAsync(string remotePath, GetFolderStatRequest request);

        /// <summary>
        ///     删除文件夹。
        /// </summary>
        DeleteFolderResponse Post(string remotePath, DeleteFolderRequest request);

        /// <summary>
        ///     异步删除文件夹。
        /// </summary>
        Task<DeleteFolderResponse> PostAsync(string remotePath, DeleteFolderRequest request);

        #endregion

        #region 文件

        /// <summary>
        ///     上传文件。
        /// </summary>
        UploadFileResponse Post(string remoteFilePath, UploadFileRequest request);

        /// <summary>
        ///     异步上传文件。
        /// </summary>
        Task<UploadFileResponse> PostAsync(string remoteFilePath, UploadFileRequest request);

        #endregion
    }
}