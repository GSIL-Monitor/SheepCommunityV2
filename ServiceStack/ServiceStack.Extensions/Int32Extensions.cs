using System;
using ServiceStack.Extensions.Properties;

namespace ServiceStack.Extensions
{
    /// <summary>
    ///     整型数字的扩展。
    /// </summary>
    public static class Int32Extensions
    {
        #region 参数检测

        /// <summary>
        ///     如果指定值与要比较的数值不相等，则抛出异常。
        /// </summary>
        /// <param name="value">要检测的数值。</param>
        /// <param name="varName">数值参数的名称。</param>
        /// <param name="givenValue">指定要比较的数值。</param>
        /// <param name="errorMessage">抛出异常的错误信息。</param>
        public static void ThrowIfNotEqual(this int value, string varName, int givenValue, string errorMessage = null)
        {
            if (value != givenValue)
            {
                throw new ArgumentException(varName ?? nameof(value), errorMessage.IsNullOrEmpty() ? Resources.ValueIsNotEqual.Fmt(value, givenValue) : errorMessage);
            }
        }

        /// <summary>
        ///     如果指定值超出最大最小值的范围，则抛出异常。
        /// </summary>
        /// <param name="value">要检测的数值。</param>
        /// <param name="varName">数值参数的名称。</param>
        /// <param name="minValue">指定范围的最小值。</param>
        /// <param name="maxValue">指定范围的最大值。</param>
        /// <param name="errorMessage">抛出异常的错误信息。</param>
        public static void ThrowIfOutOfRange(this int value, string varName, int minValue, int maxValue, string errorMessage = null)
        {
            if (value < minValue)
            {
                throw new ArgumentOutOfRangeException(varName ?? nameof(value), errorMessage.IsNullOrEmpty() ? Resources.ValueLowLimitOutOfRange.Fmt(value, minValue) : errorMessage);
            }
            if (value > maxValue)
            {
                throw new ArgumentOutOfRangeException(varName ?? nameof(value), errorMessage.IsNullOrEmpty() ? Resources.ValueUpperLimitOutOfRange.Fmt(value, maxValue) : errorMessage);
            }
        }

        /// <summary>
        ///     如果指定值超出最大最小值的范围，则抛出异常。
        /// </summary>
        /// <param name="value">要检测的数值。</param>
        /// <param name="varName">数值参数的名称。</param>
        /// <param name="minValue">指定范围的最小值。</param>
        /// <param name="maxValue">指定范围的最大值。</param>
        /// <param name="errorMessage">抛出异常的错误信息。</param>
        public static void ThrowIfOutOfRange(this int? value, string varName, int minValue, int maxValue, string errorMessage = null)
        {
            if (value.HasValue)
            {
                if (value.Value < minValue)
                {
                    throw new ArgumentOutOfRangeException(varName ?? nameof(value), errorMessage.IsNullOrEmpty() ? Resources.ValueLowLimitOutOfRange.Fmt(value, minValue) : errorMessage);
                }
                if (value.Value > maxValue)
                {
                    throw new ArgumentOutOfRangeException(varName ?? nameof(value), errorMessage.IsNullOrEmpty() ? Resources.ValueUpperLimitOutOfRange.Fmt(value, maxValue) : errorMessage);
                }
            }
        }

        /// <summary>
        ///     如果其他字段开启状态下，指定值超出最大最小值的范围，则抛出异常。
        /// </summary>
        /// <param name="value">要检测的数值。</param>
        /// <param name="varName">数值参数的名称。</param>
        /// <param name="otherFieldEnabled">其他字段是否开启。</param>
        /// <param name="minValue">指定范围的最小值。</param>
        /// <param name="maxValue">指定范围的最大值。</param>
        /// <param name="errorMessage">抛出异常的错误信息。</param>
        public static void ThrowIfOutOfRangeWhenOtherFieldEnabled(this int value, string varName, bool otherFieldEnabled, int minValue, int maxValue, string errorMessage = null)
        {
            if (otherFieldEnabled)
            {
                if (value < minValue)
                {
                    throw new ArgumentOutOfRangeException(varName ?? nameof(value), errorMessage.IsNullOrEmpty() ? Resources.ValueLowLimitOutOfRange.Fmt(value, minValue) : errorMessage);
                }
                if (value > maxValue)
                {
                    throw new ArgumentOutOfRangeException(varName ?? nameof(value), errorMessage.IsNullOrEmpty() ? Resources.ValueUpperLimitOutOfRange.Fmt(value, maxValue) : errorMessage);
                }
            }
        }

        /// <summary>
        ///     如果其他字段开启状态下，指定值超出最大最小值的范围，则抛出异常。
        /// </summary>
        /// <param name="value">要检测的数值。</param>
        /// <param name="varName">数值参数的名称。</param>
        /// <param name="otherFieldEnabled">其他字段是否开启。</param>
        /// <param name="minValue">指定范围的最小值。</param>
        /// <param name="maxValue">指定范围的最大值。</param>
        /// <param name="errorMessage">抛出异常的错误信息。</param>
        public static void ThrowIfOutOfRangeWhenOtherFieldEnabled(this int? value, string varName, bool otherFieldEnabled, int minValue, int maxValue, string errorMessage = null)
        {
            if (otherFieldEnabled)
            {
                if (!value.HasValue)
                {
                    throw new ArgumentNullException(varName, errorMessage.IsNullOrEmpty() ? $"{varName} is null or empty." : errorMessage);
                }
                if (value < minValue)
                {
                    throw new ArgumentOutOfRangeException(varName ?? nameof(value), errorMessage.IsNullOrEmpty() ? Resources.ValueLowLimitOutOfRange.Fmt(value, minValue) : errorMessage);
                }
                if (value > maxValue)
                {
                    throw new ArgumentOutOfRangeException(varName ?? nameof(value), errorMessage.IsNullOrEmpty() ? Resources.ValueUpperLimitOutOfRange.Fmt(value, maxValue) : errorMessage);
                }
            }
        }

        #endregion
    }
}