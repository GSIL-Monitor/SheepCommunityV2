using System;
using System.Security.Cryptography;
using ServiceStack.Text;

namespace ServiceStack.Extensions
{
    /// <summary>
    ///     哈希算法的扩展。
    /// </summary>
    public static class HashExtensions
    {
        #region SHA1 加密

        public static string ToSha1HashString(this string value)
        {
            var builder = StringBuilderCache.Allocate();
            using (var sha1 = SHA1.Create())
            {
                foreach (var num in sha1.ComputeHash(value.ToUtf8Bytes()))
                {
                    builder.Append(num.ToString("x2"));
                }
            }
            return StringBuilderCache.ReturnAndFree(builder);
        }

        public static string ToSha1HashString(this byte[] bytes)
        {
            using (var sha1 = SHA1.Create())
            {
                var hashBytes = sha1.ComputeHash(bytes);
                var hashData = BitConverter.ToString(hashBytes);
                return hashData.Replace("-", string.Empty).ToLower();
            }
        }

        public static byte[] ToSha1HashBytes(this byte[] bytes)
        {
            using (var sha1 = SHA1.Create())
            {
                return sha1.ComputeHash(bytes);
            }
        }

        #endregion

        #region MD5 加密

        public static string ToMd5HashString(this string value)
        {
            var builder = StringBuilderCache.Allocate();
            using (var md5 = MD5.Create())
            {
                foreach (var num in md5.ComputeHash(value.ToUtf8Bytes()))
                {
                    builder.Append(num.ToString("x2"));
                }
            }
            return StringBuilderCache.ReturnAndFree(builder);
        }

        public static byte[] ToMd5HashBytes(this string value)
        {
            using (var md5 = MD5.Create())
            {
                return md5.ComputeHash(value.ToUtf8Bytes());
            }
        }

        public static byte[] ToMd5HashBytes(this byte[] bytes)
        {
            using (var md5 = MD5.Create())
            {
                return md5.ComputeHash(bytes);
            }
        }

        #endregion

        #region HMACSHA1 加密

        public static string ToHmacSha1HashString(this string value, string key)
        {
            var builder = StringBuilderCache.Allocate();
            using (var sha1 = new HMACSHA1(key.ToUtf8Bytes()))
            {
                foreach (var num in sha1.ComputeHash(value.ToUtf8Bytes()))
                {
                    builder.Append(num.ToString("x2"));
                }
            }
            return StringBuilderCache.ReturnAndFree(builder);
        }

        public static byte[] ToHmacSha1HashBytes(this string value, string key)
        {
            using (var sha1 = new HMACSHA1(key.ToUtf8Bytes()))
            {
                return sha1.ComputeHash(value.ToUtf8Bytes());
            }
        }

        public static byte[] ToHmacSha1HashBytes(this byte[] bytes, string key)
        {
            using (var sha1 = new HMACSHA1(key.ToUtf8Bytes()))
            {
                return sha1.ComputeHash(bytes);
            }
        }

        #endregion
    }
}