namespace Sheep.Common.Settings
{
    /// <summary>
    ///     腾讯微信配置的名称。
    /// </summary>
    public static class AppSettingsWeixinNames
    {
        // 申请应用时分配的应用程序唯一标识。
        public const string AppId = "oauth.weixin.AppKey";

        // 申请应用时分配的应用程序密钥。
        public const string AppSecret = "oauth.weixin.AppSecret";

        // 获取接口调用凭证的地址。
        public const string AccessTokenUrl = "oauth.weixin.AccessTokenUrl";

        // 刷新或续期接口调用凭证使用的地址。
        public const string RefreshTokenUrl = "oauth.weixin.RefreshTokenUrl";

        // 检验接口调用凭证是否有效的地址。
        public const string AuthenticateTokenUrl = "oauth.weixin.AuthenticateTokenUrl";

        // 获取用户个人信息的地址。
        public const string UserInfoUrl = "oauth.weixin.UserInfoUrl";
    }
}