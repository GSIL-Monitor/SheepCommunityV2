using Aliyun.Green;
using NUnit.Framework;
using ServiceStack.Configuration;
using Sheep.Common.Settings;

namespace Sheep.Tests.Social.Aliyun
{
    public class GreenClientTestBase
    {
        #region 变量

        public IGreenClient GreenClient;
        public IAppSettings AppSettings;

        #endregion

        #region 启动与结束设置

        [OneTimeSetUp]
        public virtual void OnBeforeTest()
        {
            AppSettings = new AppSettings();
            GreenClient = new GreenClient
                          {
                              AccessKeyId = AppSettings.GetString(AppSettingsGreenNames.GreenAccessKeyId),
                              AccessKeySecret = AppSettings.GetString(AppSettingsGreenNames.GreenAccessKeySecret),
                              BaseUrl = AppSettings.GetString(AppSettingsGreenNames.GreenBaseUrl),
                              ImageScanPath = AppSettings.GetString(AppSettingsGreenNames.GreenImageScanPath),
                              TextScanPath = AppSettings.GetString(AppSettingsGreenNames.GreenTextScanPath)
                          };
        }

        [OneTimeTearDown]
        public virtual void OnAfterTest()
        {
        }

        [SetUp]
        public virtual void OnBeforeEachTest()
        {
        }

        [TearDown]
        public virtual void OnAfterEachTest()
        {
        }

        #endregion
    }
}