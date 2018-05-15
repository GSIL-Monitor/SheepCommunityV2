using System;
using System.Text.RegularExpressions;

namespace ServiceStack.Extensions
{
    /// <summary>
    ///     参数。
    /// </summary>
    public static class Arguments
    {
        #region 对象检测

        /// <summary>
        ///     断言指定对象不能为空。
        /// </summary>
        /// <param name="obj">要检测的对象</param>
        /// <param name="paramName">参数的名称</param>
        /// <param name="errorMessage">抛出异常的错误信息</param>
        public static void NotNull(object obj, string paramName, string errorMessage = null)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(string.IsNullOrWhiteSpace(paramName) ? nameof(obj) : paramName, errorMessage);
            }
        }

        #endregion

        #region 字符串文本检测

        /// <summary>
        ///     断言指定文本不能为空。
        /// </summary>
        /// <param name="text">要检测的文本</param>
        /// <param name="paramName">参数的名称</param>
        /// <param name="errorMessage">抛出异常的错误信息</param>
        public static void NotNullOrEmpty(string text, string paramName, string errorMessage = null)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException(string.IsNullOrWhiteSpace(paramName) ? nameof(text) : paramName, errorMessage);
            }
        }

        /// <summary>
        ///     如果指定文本的长度超出最大最小值的范围，则抛出异常。
        /// </summary>
        /// <param name="text">要检测的文本</param>
        /// <param name="paramName">参数的名称</param>
        /// <param name="minLength">指定的最小长度</param>
        /// <param name="maxLength">指定的最大长度</param>
        /// <param name="errorMessage">抛出异常的错误信息</param>
        public static void WithinLength(string text, string paramName, int minLength, int maxLength, string errorMessage = null)
        {
            var length = string.IsNullOrWhiteSpace(text) ? 0 : text.Length;
            if (length < minLength)
            {
                throw new ArgumentOutOfRangeException(string.IsNullOrWhiteSpace(paramName) ? nameof(text) : paramName,
                                                      string.IsNullOrWhiteSpace(errorMessage) ? $"Insufficient text length. The minimum length required is {minLength}." : errorMessage);
            }
            if (length > maxLength)
            {
                throw new ArgumentOutOfRangeException(string.IsNullOrWhiteSpace(paramName) ? nameof(text) : paramName, string.IsNullOrWhiteSpace(errorMessage) ? $"Exceeded text length. The maximum length is {maxLength}." : errorMessage);
            }
        }

        /// <summary>
        ///     如果文本无法匹配指定的正则表达式，则抛出异常。
        /// </summary>
        /// <param name="text">要检测的文本</param>
        /// <param name="paramName">参数的名称</param>
        /// <param name="regexPattern">要匹配的正则表达式</param>
        /// <param name="errorMessage">抛出异常的错误信息</param>
        public static void ShouldMatch(string text, string paramName, string regexPattern, string errorMessage = null)
        {
            if (!Regex.IsMatch(text, regexPattern))
            {
                throw new ArgumentException(string.IsNullOrWhiteSpace(errorMessage) ? "Invalid text format." : errorMessage, string.IsNullOrWhiteSpace(paramName) ? nameof(text) : paramName);
            }
        }

        #endregion
    }
}