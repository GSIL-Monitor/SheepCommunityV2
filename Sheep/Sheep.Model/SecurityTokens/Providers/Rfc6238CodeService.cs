using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using ServiceStack;

namespace Sheep.Model.SecurityTokens.Providers
{
    /// <summary>
    ///     基于时间的一次性代码生成与验证服务。
    /// </summary>
    public static class Rfc6238CodeService
    {
        #region 属性

        private static readonly DateTime s_UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly TimeSpan s_Timestep = TimeSpan.FromMinutes(30);
        private static readonly Encoding s_Encoding = new UTF8Encoding(false, true);

        #endregion

        #region 生成代码

        /// <summary>
        ///     生成安全代码。
        /// </summary>
        public static int GenerateCode(byte[] securityToken, string modifier = null)
        {
            securityToken.ThrowIfNull(nameof(securityToken));
            var timestepNumber = GetCurrentTimeStepNumber();
            using (var hashAlgorithm = new HMACSHA1(securityToken))
            {
                return ComputeTotp(hashAlgorithm, timestepNumber, modifier);
            }
        }

        #endregion

        #region 校验代码

        /// <summary>
        ///     校验安全代码。
        /// </summary>
        public static bool VerifyCode(byte[] securityToken, int code, string modifier = null)
        {
            securityToken.ThrowIfNull(nameof(securityToken));
            var timestepNumber = GetCurrentTimeStepNumber();
            using (var hashAlgorithm = new HMACSHA1(securityToken))
            {
                for (var index = -2; index <= 2; ++index)
                {
                    if (ComputeTotp(hashAlgorithm, timestepNumber + (ulong) index, modifier) == code)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion

        #region 辅助方法

        /// <summary>
        ///     获取时间步长。
        /// </summary>
        /// <returns></returns>
        private static ulong GetCurrentTimeStepNumber()
        {
            var utcNow = DateTime.UtcNow - s_UnixEpoch;
            return (ulong) (utcNow.Ticks / s_Timestep.Ticks);
        }

        /// <summary>
        ///     计算安全代码。
        ///     TOTP: Time-Based One-Time Password Algorithm
        /// </summary>
        private static int ComputeTotp(HashAlgorithm hashAlgorithm, ulong timestepNumber, string modifier)
        {
            var data = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((long) timestepNumber));
            var hashBuffer = hashAlgorithm.ComputeHash(ApplyModifier(data, modifier));
            var index = hashBuffer[hashBuffer.Length - 1] & 15;
            var code = ((hashBuffer[index] & 127) << 24) | ((hashBuffer[index + 1] & 255) << 16) | ((hashBuffer[index + 2] & 255) << 8) | (hashBuffer[index + 3] & 255);
            return code % 1000000;
        }

        /// <summary>
        ///     对输入的二进制数据使用修改器。
        /// </summary>
        private static byte[] ApplyModifier(byte[] data, string modifier)
        {
            if (modifier.IsNullOrEmpty())
            {
                return data;
            }
            var modifierBuffer = s_Encoding.GetBytes(modifier);
            var buffer = new byte[checked(data.Length + modifierBuffer.Length)];
            Buffer.BlockCopy(data, 0, buffer, 0, data.Length);
            Buffer.BlockCopy(modifierBuffer, 0, buffer, data.Length, modifierBuffer.Length);
            return buffer;
        }

        #endregion
    }
}