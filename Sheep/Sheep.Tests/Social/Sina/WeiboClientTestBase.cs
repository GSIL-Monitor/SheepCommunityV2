using NUnit.Framework;
using ServiceStack.Configuration;
using Sheep.Common.Settings;
using Sina.Weibo;

namespace Sheep.Tests.Social.Sina
{
    public class WeiboClientTestBase
    {
        #region 变量

        public IWeiboClient WeiboClient;

        public IAppSettings AppSettings;

        #endregion

        #region 启动与结束设置

        [OneTimeSetUp]
        public virtual void OnBeforeTest()
        {
        }

        [OneTimeTearDown]
        public virtual void OnAfterTest()
        {
        }

        [SetUp]
        public virtual void OnBeforeEachTest()
        {
            AppSettings = new AppSettings();
            WeiboClient = new WeiboClient
                          {
                              AppKey = AppSettings.GetString(AppSettingsWeiboNames.AppKey),
                              AppSecret = AppSettings.GetString(AppSettingsWeiboNames.AppSecret),
                              AccessTokenUrl = AppSettings.GetString(AppSettingsWeiboNames.AccessTokenUrl),
                              GetTokenUrl = AppSettings.GetString(AppSettingsWeiboNames.GetTokenUrl),
                              ShowUserUrl = AppSettings.GetString(AppSettingsWeiboNames.ShowUserUrl),
                              GetCountryUrl = AppSettings.GetString(AppSettingsWeiboNames.GetCountryUrl),
                              GetProvinceUrl = AppSettings.GetString(AppSettingsWeiboNames.GetProvinceUrl),
                              GetCityUrl = AppSettings.GetString(AppSettingsWeiboNames.GetCityUrl),
                              RedirectUrl = AppSettings.GetString(AppSettingsWeiboNames.RedirectUrl)
                          };
        }

        [TearDown]
        public virtual void OnAfterEachTest()
        {
        }

        #endregion

    }
}