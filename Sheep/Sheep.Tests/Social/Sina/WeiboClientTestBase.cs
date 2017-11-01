using NUnit.Framework;
using RethinkDb.Driver;
using RethinkDb.Driver.Net;
using ServiceStack;
using ServiceStack.Configuration;
using Sheep.Common.Settings;
using Sina.Weibo;

namespace Sheep.Tests.Social.Sina
{
    public class WeiboClientTestBase
    {
        #region 变量

        /// <summary>
        ///     RethinkDB 查询器.
        /// </summary>
        public static readonly RethinkDB R = RethinkDB.R;

        public IConnection Conn;
        public IWeiboClient WeiboClient;
        public IAppSettings AppSettings;

        #endregion

        #region 启动与结束设置

        [OneTimeSetUp]
        public virtual void OnBeforeTest()
        {
            AppSettings = new AppSettings();
            Conn = R.Connection().Hostname(AppSettings.GetString(AppSettingsDbNames.RethinkDbHostName)).Port(AppSettings.GetString(AppSettingsDbNames.RethinkDbPort).ToInt()).Timeout(AppSettings.GetString(AppSettingsDbNames.RethinkDbTimeout).ToInt()).Db(AppSettings.GetString(AppSettingsDbNames.RethinkDbDatabase)).Connect();
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