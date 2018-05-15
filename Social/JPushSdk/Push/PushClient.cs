using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Logging;

namespace JPush.Push
{
    /// <summary>
    ///     极光推送服务客户端封装库。
    /// </summary>
    public class PushClient : IPushClient
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(PushClient));

        #endregion

        #region 属性

        /// <summary>
        ///     申请应用时分配的应用程序唯一标识。
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        ///     申请应用时分配的应用程序密钥。
        /// </summary>
        public string MasterSecret { get; set; }

        /// <summary>
        ///     获取CID列表的地址。
        /// </summary>
        public string CidUrl { get; set; }

        /// <summary>
        ///     获取接口推送的地址。
        /// </summary>
        public string PushUrl { get; set; }

        #endregion

        #region IPushClient 接口实现

        /// <inheritdoc />
        public CidResponse Get(CidRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => GetAsync(request));
        }

        /// <inheritdoc />
        public async Task<CidResponse> GetAsync(CidRequest request)
        {
            try
            {
                var httpHandler = new HttpClientHandler();
                using (var httpClient = new HttpClient(httpHandler))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(string.Format("{0}:{1}", AppKey, MasterSecret).ToUtf8Bytes()));
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var responseMessage = await httpClient.GetAsync(string.Format("{0}?{1}", CidUrl, request.ToQueryString()));
                    var responseContent = responseMessage.Content == null ? string.Empty : await responseMessage.Content.ReadAsStringAsync();
                    var response = responseContent.FromJson<CidResponse>();
                    return response;
                }
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new CidResponse();
            }
        }

        /// <summary>
        ///     推送消息。
        /// </summary>
        public PushResponse Post(PushRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步推送消息。
        /// </summary>
        public async Task<PushResponse> PostAsync(PushRequest request)
        {
            try
            {
                var httpHandler = new HttpClientHandler();
                using (var httpClient = new HttpClient(httpHandler))
                {
                    var requestContent = new StringContent(request.ToJson(), Encoding.UTF8);
                    requestContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(string.Format("{0}:{1}", AppKey, MasterSecret).ToUtf8Bytes()));
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var responseMessage = await httpClient.PostAsync(PushUrl, requestContent);
                    var responseContent = responseMessage.Content == null ? string.Empty : await responseMessage.Content.ReadAsStringAsync();
                    var response = responseContent.FromJson<PushResponse>();
                    if (response?.Error != null && response.Error.Code != 0)
                    {
                        Log.ErrorFormat("{0} {1} Error: {2}-{3}-{4}", GetType().Name, request.GetType().Name, response.Error, response.Error.Code, response.Error.Message);
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new PushResponse
                       {
                           Error = new Error
                                   {
                                       Code = -1,
                                       Message = errorMessage
                                   }
                       };
            }
        }

        #endregion
    }
}