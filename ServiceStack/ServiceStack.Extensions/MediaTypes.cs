namespace ServiceStack.Extensions
{
    /// <summary>
    ///     媒体类型。
    /// </summary>
    public static class MediaTypes
    {
        #region 应用程序子类

        public static class Application
        {
            /// <summary>
            ///     JSON的媒体类型。
            /// </summary>
            public const string Json = "application/json";
        }

        #endregion

        #region 图像子类

        public static class Image
        {
            /// <summary>
            ///     BMP图像类型。
            /// </summary>
            public const string Bmp = "image/bmp";

            /// <summary>
            ///     PNG图像类型。
            /// </summary>
            public const string Png = "image/png";

            /// <summary>
            ///     WBMP图像类型。
            /// </summary>
            public const string WebBmp = "image/vnd.wap.wbmp";
        }

        #endregion
    }
}