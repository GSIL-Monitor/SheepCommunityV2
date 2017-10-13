﻿using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Extensions;
using ServiceStack.Logging;
using Sheep.Model.SecurityStamps;
using Top.Api;
using Top.Api.Request;
using AsyncContext = Nito.AsyncEx.AsyncContext;

namespace Sheep.Model.SecurityTokens.Providers
{
    /// <summary>
    ///     基于手机发送的安全令牌提供程序。
    /// </summary>
    public class Rfc6238CodeMobileSecurityTokenProvider : ISecurityTokenProvider
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(Rfc6238CodeMobileSecurityTokenProvider));

        #endregion

        #region 属性

        /// <summary>
        ///     阿里大于客户端。
        /// </summary>
        public ITopClient TopClient { get; set; }

        /// <summary>
        ///     安全戳存储库。
        /// </summary>
        public ISecurityStampRepository SecurityStampRepository { get; set; }

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="Rfc6238CodeMobileSecurityTokenProvider" />对象。
        /// </summary>
        /// <param name="topClient">阿里大于客户端。</param>
        /// <param name="securityStampRepository">安全戳存储库。</param>
        public Rfc6238CodeMobileSecurityTokenProvider(ITopClient topClient, ISecurityStampRepository securityStampRepository)
        {
            TopClient = topClient;
            SecurityStampRepository = securityStampRepository;
        }

        #endregion

        #region ISecurityTokenProvider 接口实现

        /// <summary>
        ///     生成特定用途的令牌。
        /// </summary>
        /// <param name="target">手机号码或电子邮件地址。</param>
        /// <param name="purpose">特定的用途。</param>
        /// <returns>令牌。</returns>
        public string GenerateToken(string target, string purpose)
        {
            return AsyncContext.Run(() => GenerateTokenAsync(target, purpose));
        }

        /// <summary>
        ///     生成特定用途的令牌。
        /// </summary>
        /// <param name="target">手机号码或电子邮件地址。</param>
        /// <param name="purpose">特定的用途。</param>
        /// <returns>令牌。</returns>
        public async Task<string> GenerateTokenAsync(string target, string purpose)
        {
            target.ThrowIfNullOrEmpty(nameof(target));
            target.ThrowIfNotMatchPhoneNumber(nameof(target));
            purpose.ThrowIfNullOrEmpty(nameof(purpose));
            var securityStamp = await SecurityStampRepository.GetSecurityStampAsync(target);
            var tokenModifier = GetTokenModifier(target, purpose);
            var tokenCode = Rfc6238CodeService.GenerateCode(securityStamp.ToSecurityToken(), tokenModifier);
            return tokenCode.ToString("D6", CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     校验指定用途的用户令牌。
        /// </summary>
        /// <param name="target">手机号码或电子邮件地址。</param>
        /// <param name="purpose">特定的用途。</param>
        /// <param name="token">令牌。</param>
        /// <returns>true 表示校验成功，否则为 false。</returns>
        public bool VerifyToken(string target, string purpose, string token)
        {
            return AsyncContext.Run(() => VerifyTokenAsync(target, purpose, token));
        }

        /// <summary>
        ///     校验指定用途的用户令牌。
        /// </summary>
        /// <param name="target">手机号码或电子邮件地址。</param>
        /// <param name="purpose">特定的用途。</param>
        /// <param name="token">令牌。</param>
        /// <returns>true 表示校验成功，否则为 false。</returns>
        public async Task<bool> VerifyTokenAsync(string target, string purpose, string token)
        {
            target.ThrowIfNullOrEmpty(nameof(target));
            target.ThrowIfNotMatchPhoneNumber(nameof(target));
            purpose.ThrowIfNullOrEmpty(nameof(purpose));
            token.ThrowIfNullOrEmpty(nameof(token));
            var securityStamp = await SecurityStampRepository.GetSecurityStampAsync(target);
            var tokenModifier = GetTokenModifier(target, purpose);
            var tokenCode = token.ToInt();
            return Rfc6238CodeService.VerifyCode(securityStamp.ToSecurityToken(), tokenCode, tokenModifier);
        }

        /// <summary>
        ///     通知用户令牌已经生成，可以发送短信或邮件。
        /// </summary>
        /// <param name="target">手机号码或电子邮件地址。</param>
        /// <param name="signature">签名。</param>
        /// <param name="token">令牌。</param>
        /// <param name="template">模板的编号或内容。</param>
        /// <returns>true 表示发送成功，否则为 false。</returns>
        public bool SendToken(string target, string signature, string token, string template)
        {
            return AsyncContext.Run(() => SendTokenAsync(target, signature, token, template));
        }

        /// <summary>
        ///     通知用户令牌已经生成，可以发送短信或邮件。
        /// </summary>
        /// <param name="target">手机号码或电子邮件地址。</param>
        /// <param name="signature">签名。</param>
        /// <param name="token">令牌。</param>
        /// <param name="template">模板的编号或内容。</param>
        /// <returns>true 表示发送成功，否则为 false。</returns>
        public async Task<bool> SendTokenAsync(string target, string signature, string token, string template)
        {
            target.ThrowIfNullOrEmpty(nameof(target));
            target.ThrowIfNotMatchPhoneNumber(nameof(target));
            signature.ThrowIfNullOrEmpty(nameof(signature));
            token.ThrowIfNullOrEmpty(nameof(token));
            template.ThrowIfNullOrEmpty(nameof(template));
            var request = new AlibabaAliqinFcSmsNumSendRequest
                          {
                              SmsType = "normal",
                              SmsFreeSignName = signature,
                              SmsParam = "{" + "\"token\":\"{0}\"".Fmt(token) + "}",
                              RecNum = target,
                              SmsTemplateCode = template
                          };
            var response = await Task.Run(() => TopClient.Execute(request));
            if (response.IsError)
            {
                Log.ErrorFormat("{0} {1} Error: {2}-{3} {4}-{5}", GetType().Name, MethodBase.GetCurrentMethod().Name, response.ErrCode, response.ErrMsg, response.SubErrCode, response.SubErrMsg);
                return false;
            }
            if (response.Result != null && !response.Result.Success)
            {
                Log.WarnFormat("{0} {1} Failed: {0}", GetType().Name, MethodBase.GetCurrentMethod().Name, response.Result.Msg);
                return false;
            }
            return response.Result != null && response.Result.Success;
        }

        #endregion

        #region 获取修改器

        /// <summary>
        ///     获取令牌修改器。
        /// </summary>
        protected virtual string GetTokenModifier(string phoneNumber, string purpose)
        {
            return "MobileTokenModifier:{0}:{1}".Fmt(phoneNumber, purpose);
        }

        #endregion
    }
}