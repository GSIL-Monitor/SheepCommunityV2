namespace Sheep
{
    /// <summary>
    ///     新浪微博配置的名称。
    /// </summary>
    public static class AppSettingsWeiboNames
    {
        // 申请应用时分配的应用程序唯一标识。
        public const string AppKey = "oauth.weibo.AppKey";

        // 申请应用时分配的应用程序密钥。
        public const string AppSecret = "oauth.weibo.AppSecret";

        // 获取接口调用凭证的地址。
        public const string AccessTokenUrl = "oauth.weibo.AccessTokenUrl";

        // 重定向返回的地址。
        public const string RedirectUrl = "oauth.weibo.RedirectUrl";

        // 获取接口调用凭证的地址。
        public const string GetTokenUrl = "oauth.weibo.GetTokenUrl";

        // 根据用户编号获取用户信息的地址。
        public const string ShowUserUrl = "oauth.weibo.ShowUserUrl";
    }
}