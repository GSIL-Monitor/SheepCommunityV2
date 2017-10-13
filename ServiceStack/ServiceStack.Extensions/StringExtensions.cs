using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ServiceStack.Extensions.Properties;

namespace ServiceStack.Extensions
{
    /// <summary>
    ///     文本的扩展。
    /// </summary>
    public static class StringExtensions
    {
        #region 转换

        /// <summary>
        ///     如果文本为空，则将其转换为空字符串。
        /// </summary>
        /// <param name="text">要转换的文本。</param>
        /// <returns>空字符串或原始文本。</returns>
        public static string ToEmptyIfNull(this string text)
        {
            return string.IsNullOrEmpty(text) ? string.Empty : text;
        }

        /// <summary>
        ///     将GuidN格式的文本转换成Guid全局唯一标识。
        /// </summary>
        /// <param name="guidText">GuidN格式的文本。</param>
        /// <returns>Guid全局唯一标识。</returns>
        public static Guid FromGuidN(this string guidText)
        {
            if (string.IsNullOrEmpty(guidText))
            {
                return Guid.Empty;
            }
            Guid result;
            if (!Guid.TryParseExact(guidText, "N", out result))
            {
                return Guid.Empty;
            }
            return result;
        }

        /// <summary>
        ///     将Rfc3339日期格式的文本转换成日期时间。
        /// </summary>
        /// <param name="dateTimeText">Rfc3339日期格式的文本。</param>
        /// <returns>日期时间。</returns>
        public static DateTime FromRfc3339DateTime(this string dateTimeText)
        {
            DateTime finalDateTime;
            return DateTime.TryParseExact(dateTimeText.Replace(" Etc/GMT", string.Empty).Replace("\"", string.Empty).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out finalDateTime) ? finalDateTime : DateTime.MinValue;
        }

        #endregion

        #region 参数检测

        /// <summary>
        ///     如果指定文本的长度超出最大最小值的范围，则抛出异常。
        /// </summary>
        /// <param name="text">要检测的文本。</param>
        /// <param name="varName">文本参数的名称。</param>
        /// <param name="minLength">指定范围的最小长度。</param>
        /// <param name="maxLength">指定范围的最大长度。</param>
        /// <param name="errorMessage">抛出异常的错误信息。</param>
        public static void ThrowIfOutOfLength(this string text, string varName, int minLength, int maxLength, string errorMessage = null)
        {
            if (!string.IsNullOrEmpty(text))
            {
                if (text.Length < minLength)
                {
                    throw new ArgumentOutOfRangeException(varName ?? nameof(text), errorMessage.IsNullOrEmpty() ? Resources.TextMinLengthOutOfRange.Fmt(text.Length, minLength) : errorMessage);
                }
                if (text.Length > maxLength)
                {
                    throw new ArgumentOutOfRangeException(varName ?? nameof(text), errorMessage.IsNullOrEmpty() ? Resources.TextMaxLengthOutOfRange.Fmt(text.Length, maxLength) : errorMessage);
                }
            }
            else
            {
                if (minLength > 0)
                {
                    throw new ArgumentOutOfRangeException(varName ?? nameof(text), errorMessage.IsNullOrEmpty() ? Resources.TextMinLengthOutOfRange.Fmt(0, minLength) : errorMessage);
                }
            }
        }

        /// <summary>
        ///     如果其他字段开启状态下，指定文本为空，则抛出异常。
        /// </summary>
        /// <param name="text">要检测的文本。</param>
        /// <param name="varName">文本参数的名称。</param>
        /// <param name="otherFieldEnabled">其他字段是否开启。</param>
        /// <param name="errorMessage">抛出异常的错误信息。</param>
        public static void ThrowIfNullOrEmptyWhenOtherFieldEnabled(this string text, string varName, bool otherFieldEnabled, string errorMessage = null)
        {
            if (otherFieldEnabled && string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(varName ?? nameof(text), errorMessage.IsNullOrEmpty() ? Resources.TextIsNullOrEmpty : errorMessage);
            }
        }

        /// <summary>
        ///     如果文本无法匹配指定的正则表达式，则抛出异常。
        /// </summary>
        /// <param name="text">要检测的文本。</param>
        /// <param name="varName">文本参数的名称。</param>
        /// <param name="expression">要匹配的正则表达式。</param>
        /// <param name="errorMessage">抛出异常的错误信息。</param>
        public static void ThrowIfNotMatchRegex(this string text, string varName, string expression, string errorMessage = null)
        {
            if (!Regex.IsMatch(text, expression))
            {
                throw new ArgumentException(errorMessage.IsNullOrEmpty() ? Resources.TextInvalidFormat : errorMessage, varName ?? nameof(text));
            }
        }

        /// <summary>
        ///     如果文本无法匹配指定的手机号码格式，则抛出异常。
        /// </summary>
        /// <param name="text">要检测的文本。</param>
        /// <param name="varName">文本参数的名称。</param>
        /// <param name="errorMessage">抛出异常的错误信息。</param>
        public static void ThrowIfNotMatchPhoneNumber(this string text, string varName, string errorMessage = null)
        {
            if (!Regex.IsMatch(text, "^1[3|4|5|7|8][0-9]{9}$"))
            {
                throw new ArgumentException(errorMessage.IsNullOrEmpty() ? Resources.TextInvalidPhoneNumber : errorMessage, varName ?? nameof(text));
            }
        }

        /// <summary>
        ///     如果文本无法匹配指定的电子邮件格式，则抛出异常。
        /// </summary>
        /// <param name="text">要检测的文本。</param>
        /// <param name="varName">文本参数的名称。</param>
        /// <param name="errorMessage">抛出异常的错误信息。</param>
        public static void ThrowIfNotMatchEmail(this string text, string varName, string errorMessage = null)
        {
            if (!Regex.IsMatch(text, "^[a-zA-Z0-9_-]+@[a-zA-Z0-9_-]+(\\.[a-zA-Z0-9_-]+)+$"))
            {
                throw new ArgumentException(errorMessage.IsNullOrEmpty() ? Resources.TextInvalidEmail : errorMessage, varName ?? nameof(text));
            }
        }

        /// <summary>
        ///     如果文本无法匹配指定的颜色二进制格式，则抛出异常。
        /// </summary>
        /// <param name="text">要检测的文本。</param>
        /// <param name="varName">文本参数的名称。</param>
        /// <param name="errorMessage">抛出异常的错误信息。</param>
        public static void ThrowIfNotMatchColorHex(this string text, string varName, string errorMessage = null)
        {
            if (!Regex.IsMatch(text, "^[0-9A-F]{8}$"))
            {
                throw new ArgumentException(errorMessage.IsNullOrEmpty() ? Resources.TextInvalidColorHex : errorMessage, varName ?? nameof(text));
            }
        }

        #endregion

        #region 后缀

        public static string ImageFileSuffix(this string fileName)
        {
            if (fileName.IsNullOrEmpty())
            {
                return string.Empty;
            }
            var splittedFileNames = fileName.Split('.');
            return splittedFileNames.Length > 1 ? splittedFileNames[splittedFileNames.Length - 1] : "jpg";
        }

        public static string ImageUrlSuffix(this string fileUrl)
        {
            if (fileUrl.IsNullOrEmpty())
            {
                return string.Empty;
            }
            if (fileUrl.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase))
            {
                fileUrl = fileUrl.SafeSubstring(7);
            }
            else if (fileUrl.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
            {
                fileUrl = fileUrl.SafeSubstring(8);
            }
            else if (fileUrl.StartsWith("ftp://", StringComparison.InvariantCultureIgnoreCase))
            {
                fileUrl = fileUrl.SafeSubstring(6);
            }
            var splittedFileUrls = fileUrl.Split('/');
            return splittedFileUrls.Length > 1 ? splittedFileUrls[splittedFileUrls.Length - 1].ImageFileSuffix() : "jpg";
        }

        public static string TrimWithEllipse(this string text, int totalLength, int cutLength)
        {
            var remainLength = totalLength - cutLength;
            if (remainLength < 1)
            {
                remainLength = 1;
            }
            if (!text.IsNullOrEmpty())
            {
                if (text.Length < remainLength)
                {
                    return text;
                }
                return "{0}...".Fmt(text.SafeSubstring(0, remainLength - 1));
            }
            return string.Empty;
        }

        #endregion

        #region 网址请求

        private static readonly HttpClient s_HttpClient = new HttpClient();

        /// <summary>
        ///     使用GET方式发送请求并获取文本。
        /// </summary>
        /// <param name="url">要请求的网址。</param>
        /// <returns>Web服务器响应后获取的文本。</returns>
        public static Task<string> HttpGetAsync(this string url)
        {
            return s_HttpClient.GetStringAsync(url);
        }

        /// <summary>
        ///     使用DELETE方式发送请求并获取文本。
        /// </summary>
        /// <param name="url">要请求的网址。</param>
        /// <returns>Web服务器响应后获取的文本。</returns>
        public static Task<string> HttpDeleteAsync(this string url)
        {
            return s_HttpClient.DeleteAsync(url)
                               .ContinueWith(task =>
                                             {
                                                 var response = task.Result;
                                                 return response.Content == null ? string.Empty : response.Content.ReadAsStringAsync().Result;
                                             });
        }

        /// <summary>
        ///     使用POST方式发送JSON请求并获取文本。
        /// </summary>
        /// <param name="url">要请求的网址。</param>
        /// <param name="json">请求的JSON内容。</param>
        /// <returns>Web服务器响应后获取的文本。</returns>
        public static Task<string> HttpPostJsonAsync(this string url, string json)
        {
            return s_HttpClient.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"))
                               .ContinueWith(task =>
                                             {
                                                 var response = task.Result;
                                                 return response.Content == null ? string.Empty : response.Content.ReadAsStringAsync().Result;
                                             });
        }

        /// <summary>
        ///     使用POST方式发送JSON请求并获取文本。
        /// </summary>
        /// <param name="url">要请求的网址。</param>
        /// <param name="json">请求的JSON内容。</param>
        /// <param name="headers">请求的HTTP头。</param>
        /// <returns>Web服务器响应后获取的文本。</returns>
        public static Task<string> HttpPostJsonAsync(this string url, string json, Dictionary<string, string> headers)
        {
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            foreach (var header in headers)
            {
                httpContent.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
            return s_HttpClient.PostAsync(url, httpContent)
                               .ContinueWith(task =>
                                             {
                                                 var response = task.Result;
                                                 return response.Content == null ? string.Empty : response.Content.ReadAsStringAsync().Result;
                                             });
        }

        /// <summary>
        ///     使用POST方式发送文本请求并获取文本。
        /// </summary>
        /// <param name="url">要请求的网址。</param>
        /// <param name="text">请求的文本内容。</param>
        /// <param name="contentType">内容的MIME类型。</param>
        /// <returns>Web服务器响应后获取的文本。</returns>
        public static Task<string> HttpPostStringAsync(this string url, string text, string contentType = "text/plain")
        {
            return s_HttpClient.PostAsync(url, new StringContent(text, Encoding.UTF8, contentType))
                               .ContinueWith(task =>
                                             {
                                                 var response = task.Result;
                                                 return response.Content == null ? string.Empty : response.Content.ReadAsStringAsync().Result;
                                             });
        }

        /// <summary>
        ///     使用POST方式发送文本请求并获取文本。
        /// </summary>
        /// <param name="url">要请求的网址。</param>
        /// <param name="text">请求的文本内容。</param>
        /// <param name="contentType">内容的MIME类型。</param>
        /// <param name="headers">请求的HTTP头。</param>
        /// <returns>Web服务器响应后获取的文本。</returns>
        public static Task<string> HttpPostStringAsync(this string url, string text, string contentType, Dictionary<string, string> headers)
        {
            var httpContent = new StringContent(text, Encoding.UTF8, contentType);
            foreach (var header in headers)
            {
                httpContent.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
            return s_HttpClient.PostAsync(url, httpContent)
                               .ContinueWith(task =>
                                             {
                                                 var response = task.Result;
                                                 return response.Content == null ? string.Empty : response.Content.ReadAsStringAsync().Result;
                                             });
        }

        /// <summary>
        ///     使用PUT方式发送JSON请求并获取文本。
        /// </summary>
        /// <param name="url">要请求的网址。</param>
        /// <param name="json">请求的JSON内容。</param>
        /// <returns>Web服务器响应后获取的文本。</returns>
        public static Task<string> HttpPutJsonAsync(this string url, string json)
        {
            return s_HttpClient.PutAsync(url, new StringContent(json, Encoding.UTF8, "application/json"))
                               .ContinueWith(task =>
                                             {
                                                 var response = task.Result;
                                                 return response.Content == null ? string.Empty : response.Content.ReadAsStringAsync().Result;
                                             });
        }

        /// <summary>
        ///     使用PUT方式发送JSON请求并获取文本。
        /// </summary>
        /// <param name="url">要请求的网址。</param>
        /// <param name="json">请求的JSON内容。</param>
        /// <param name="headers">请求的HTTP头。</param>
        /// <returns>Web服务器响应后获取的文本。</returns>
        public static Task<string> HttpPutJsonAsync(this string url, string json, Dictionary<string, string> headers)
        {
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            foreach (var header in headers)
            {
                httpContent.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
            return s_HttpClient.PutAsync(url, httpContent)
                               .ContinueWith(task =>
                                             {
                                                 var response = task.Result;
                                                 return response.Content == null ? string.Empty : response.Content.ReadAsStringAsync().Result;
                                             });
        }

        /// <summary>
        ///     使用PUT方式发送文本请求并获取文本。
        /// </summary>
        /// <param name="url">要请求的网址。</param>
        /// <param name="text">请求的文本内容。</param>
        /// <param name="contentType">内容的MIME类型。</param>
        /// <returns>Web服务器响应后获取的文本。</returns>
        public static Task<string> HttpPutStringAsync(this string url, string text, string contentType)
        {
            return s_HttpClient.PutAsync(url, new StringContent(text, Encoding.UTF8, contentType))
                               .ContinueWith(task =>
                                             {
                                                 var response = task.Result;
                                                 return response.Content == null ? string.Empty : response.Content.ReadAsStringAsync().Result;
                                             });
        }

        /// <summary>
        ///     使用PUT方式发送文本请求并获取文本。
        /// </summary>
        /// <param name="url">要请求的网址。</param>
        /// <param name="text">请求的文本内容。</param>
        /// <param name="contentType">内容的MIME类型。</param>
        /// <param name="headers">请求的HTTP头。</param>
        /// <returns>Web服务器响应后获取的文本。</returns>
        public static Task<string> HttpPutStringAsync(this string url, string text, string contentType, Dictionary<string, string> headers)
        {
            var httpContent = new StringContent(text, Encoding.UTF8, contentType);
            foreach (var header in headers)
            {
                httpContent.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
            return s_HttpClient.PutAsync(url, httpContent)
                               .ContinueWith(task =>
                                             {
                                                 var response = task.Result;
                                                 return response.Content == null ? string.Empty : response.Content.ReadAsStringAsync().Result;
                                             });
        }

        #endregion
    }
}