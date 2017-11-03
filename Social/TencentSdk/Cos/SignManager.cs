using System;
using System.Security.Cryptography;
using System.Text;
using ServiceStack.Extensions;
using ServiceStack.Logging;
using ServiceStack.Text;

namespace Tencent.Cos
{
    /// <summary>
    ///     签名管理器。
    ///     腾讯移动服务通过签名来验证请求的合法性。开发者通过将签名授权给客户端，使其具备上传下载及管理指定资源的能力。
    /// </summary>
    public class SignManager
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(SignManager));

        /// <summary>
        ///     随机数生成器。
        /// </summary>
        protected static readonly Random Rnd = new Random(Guid.NewGuid().GetHashCode());

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
        ///     存储桶的名称。
        /// </summary>
        public string Bucket { get; set; }

        #endregion

        #region 签名

        /// <summary>
        ///     多次有效签名，签名中不绑定文件。
        /// </summary>
        /// <param name="expired">大于当前时间的有效期。</param>
        /// <returns>多次有效签名。</returns>
        public string Signature(long expired)
        {
            return Signature(expired, string.Empty);
        }

        /// <summary>
        ///     多次有效签名，签名中绑定或者不绑定文件，需要设置大于当前时间的有效期，有效期内此签名可多次使用，有效期最长可设置三个月。
        /// </summary>
        /// <param name="expired">大于当前时间的有效期。</param>
        /// <param name="fileId"></param>
        /// <returns>多次有效签名。</returns>
        public string Signature(long expired, string fileId)
        {
            var now = DateTime.Now.ToUnixTime();
            var number = Rnd.Next(int.MaxValue);
            var text = $"a={AppId}&k={SecretId}&e={expired}&t={now}&r={number}&f={fileId}&b={Bucket}";
            using (var mac = new HMACSHA1(Encoding.UTF8.GetBytes(SecretKey)))
            {
                var hash = mac.ComputeHash(Encoding.UTF8.GetBytes(text));
                var buffer = Encoding.UTF8.GetBytes(text);
                var all = new byte[hash.Length + buffer.Length];
                Array.Copy(hash, 0, all, 0, hash.Length);
                Array.Copy(buffer, 0, all, hash.Length, buffer.Length);
                return Convert.ToBase64String(all);
            }
        }

        /// <summary>
        ///     单次有效签名，签名中绑定文件，有效期必须设置为 0，此签名只可使用一次，且只能应用于被绑定的文件。
        /// </summary>
        /// <param name="remotePath">指定的远程文件的路径。</param>
        /// <returns>单次有效签名。</returns>
        public string SignatureOnce(string remotePath)
        {
            var fileId = $"/{AppId}/{Bucket}{remotePath.EncodeRemotePath()}";
            return Signature(0, fileId);
        }

        #endregion
    }
}