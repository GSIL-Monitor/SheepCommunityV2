namespace Sheep.Common.Settings
{
    /// <summary>
    ///     阿里云对象存储配置的名称。
    /// </summary>
    public static class AppSettingsGreenNames
    {
        // 申请应用时分配的应用程序唯一标识。
        public const string GreenAccessKeyId = "green.AccessKeyId";

        // 申请应用时分配的应用程序密钥。
        public const string GreenAccessKeySecret = "green.AccessKeySecret";

        // 检测的基本地址。
        public const string GreenBaseUrl = "green.BaseUrl";

        // 检测图片的地址路径。
        public const string GreenImageScanPath = "green.ImageScanPath";

        // 检测文本的地址路径。
        public const string GreenTextScanPath = "green.TextScanPath";
    }
}