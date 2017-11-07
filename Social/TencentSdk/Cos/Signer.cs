using System;
using System.Security.Cryptography;
using System.Text;
using ServiceStack;
using ServiceStack.Extensions;
using ServiceStack.Logging;
using ServiceStack.Text;

namespace Tencent.Cos
{
    /// <summary>
    ///     签名器。腾讯移动服务通过签名来验证请求的合法性。开发者通过将签名授权给客户端，使其具备上传下载及管理指定资源的能力。
    /// </summary>
    public class Signer
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(Signer));

        /// <summary>
        ///     随机数生成器。
        /// </summary>
        protected static readonly Random Rnd = new Random(Guid.NewGuid().GetHashCode());

        #endregion

        #region 签名

        /// <summary>
        ///     多次有效签名，签名中不绑定文件。
        /// </summary>
        /// <param name="appId">项目的编号。</param>
        /// <param name="secretId">签名的编号。</param>
        /// <param name="secretKey">签名的密钥。</param>
        /// <param name="expired">大于当前时间的有效期。</param>
        /// <param name="bucket">存储桶的名称。</param>
        /// <returns>多次有效签名。</returns>
        public static string Signature(int appId, string secretId, string secretKey, long expired, string bucket)
        {
            return Signature(appId, secretId, secretKey, expired, string.Empty, bucket);
        }

        /// <summary>
        ///     多次有效签名，签名中绑定或者不绑定文件，需要设置大于当前时间的有效期，有效期内此签名可多次使用，有效期最长可设置三个月。
        /// </summary>
        /// <param name="appId">项目的编号。</param>
        /// <param name="secretId">签名的编号。</param>
        /// <param name="secretKey">签名的密钥。</param>
        /// <param name="expired">大于当前时间的有效期。</param>
        /// <param name="fileId">文件的编号。</param>
        /// <param name="bucket">存储桶的名称。</param>
        /// <returns>多次有效签名。</returns>
        public static string Signature(int appId, string secretId, string secretKey, long expired, string fileId, string bucket)
        {
            if (secretId.IsNullOrEmpty() || secretKey.IsNullOrEmpty())
            {
                return "-1";
            }
            var now = DateTime.Now.ToUnixTime();
            var number = Rnd.Next(int.MaxValue);
            var text = $"a={appId}&k={secretId}&e={expired}&t={now}&r={number}&f={fileId}&b={bucket}";
            using (var mac = new HMACSHA1(Encoding.UTF8.GetBytes(secretKey)))
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
        /// <param name="appId">项目的编号。</param>
        /// <param name="secretId">签名的编号。</param>
        /// <param name="secretKey">签名的密钥。</param>
        /// <param name="remotePath">指定的远程文件的路径。</param>
        /// <param name="bucket">存储桶的名称。</param>
        /// <returns>单次有效签名。</returns>
        public static string SignatureOnce(int appId, string secretId, string secretKey, string remotePath, string bucket)
        {
            var fileId = $"/{appId}/{bucket}{remotePath.EncodeRemotePath()}";
            return Signature(appId, secretId, secretKey, 0, fileId, bucket);
        }

        #endregion
    }
}