namespace Sheep.Common.Settings
{
    /// <summary>
    ///     腾讯云对象存储配置的名称。
    /// </summary>
    public static class AppSettingsCosNames
    {
        // 申请应用时分配的项目的编号。
        public const string AppId = "oauth.cos.AppId";

        // 申请应用时分配的签名的编号。
        public const string SecretId = "oauth.cos.SecretId";

        // 获取接口调用凭证的签名的密钥。
        public const string SecretKey = "oauth.cos.SecretKey";

        // 接口的地址。。
        public const string ApiUrl = "oauth.cos.ApiUrl";

        // 存储桶的名称。
        public const string Bucket = "oauth.cos.Bucket";
    }
}