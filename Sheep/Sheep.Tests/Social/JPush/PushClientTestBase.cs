using JPush.Push;
using NUnit.Framework;
using ServiceStack.Configuration;
using Sheep.Common.Settings;

namespace Sheep.Tests.Social.JPush
{
    public class PushClientTestBase
    {
        #region 变量

        public IPushClient PushClient;
        public IAppSettings AppSettings;

        #endregion

        #region 启动与结束设置

        [OneTimeSetUp]
        public virtual void OnBeforeTest()
        {
            AppSettings = new AppSettings();
            PushClient = new PushClient
                         {
                             AppKey = AppSettings.GetString(AppSettingsJPushNames.AppKey),
                             MasterSecret = AppSettings.GetString(AppSettingsJPushNames.MasterSecret),
                             CidUrl = AppSettings.GetString(AppSettingsJPushNames.CidUrl),
                             PushUrl = AppSettings.GetString(AppSettingsJPushNames.PushUrl)
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