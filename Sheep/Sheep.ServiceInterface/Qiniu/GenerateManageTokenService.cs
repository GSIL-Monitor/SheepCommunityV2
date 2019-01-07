using System.Net.Http;
using System.Threading.Tasks;
using Qiniu.Util;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.Logging;
using Sheep.Common.Settings;
using Sheep.ServiceModel.Qiniu;

namespace Sheep.ServiceInterface.Qiniu
{
    /// <summary>
    ///     生成一个管理凭证服务接口。
    /// </summary>
    [CompressResponse]
    public class GenerateManageTokenService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(GenerateManageTokenService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     申请应用时分配的应用程序唯一标识。
        /// </summary>
        public string AccessKey
        {
            get
            {
                return AppSettings.GetString(AppSettingsQiniuNames.AccessKey);
            }
        }

        /// <summary>
        ///     申请应用时分配的应用程序密钥。
        /// </summary>
        public string SecretKey
        {
            get
            {
                return AppSettings.GetString(AppSettingsQiniuNames.SecretKey);
            }
        }

        /// <summary>
        ///     存储桶
        /// </summary>
        public string Bucket
        {
            get
            {
                return AppSettings.GetString(AppSettingsQiniuNames.Bucket);
            }
        }

        #endregion

        #region 生成一个管理凭证

        /// <summary>
        ///     生成一个管理凭证。
        /// </summary>
        public async Task<object> Get(ManageTokenGenerate request)
        {
            var mac = new Mac(AccessKey, SecretKey);
            if (request.RequestBody == null)
            {
                var manageToken = Auth.CreateManageToken(mac, request.RequestUrl, null);
                return new ManageTokenGenerateResponse
                       {
                                       ManageToken = manageToken
                       };
            }

            using (var requestBody = new FormUrlEncodedContent(request.RequestBody))
            {
                var requestBodyBuffer = await requestBody.ReadAsByteArrayAsync();
                var manageToken = Auth.CreateManageToken(mac, request.RequestUrl, requestBodyBuffer);
                return new ManageTokenGenerateResponse
                       {
                                       ManageToken = manageToken
                       };
            }
        }

        #endregion
    }
}