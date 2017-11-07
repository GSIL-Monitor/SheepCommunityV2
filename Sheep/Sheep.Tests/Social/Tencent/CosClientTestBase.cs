using NUnit.Framework;
using RethinkDb.Driver;
using RethinkDb.Driver.Net;
using ServiceStack;
using ServiceStack.Configuration;
using Sheep.Common.Settings;
using Tencent.Cos;

namespace Sheep.Tests.Social.Tencent
{
    public class CosClientTestBase
    {
        #region 变量

        /// <summary>
        ///     RethinkDB 查询器.
        /// </summary>
        public static readonly RethinkDB R = RethinkDB.R;

        public IConnection Conn;
        public ICosClient CosClient;
        public IAppSettings AppSettings;

        #endregion

        #region 启动与结束设置

        [OneTimeSetUp]
        public virtual void OnBeforeTest()
        {
            AppSettings = new AppSettings();
            Conn = R.Connection().Hostname(AppSettings.GetString(AppSettingsDbNames.RethinkDbHostName)).Port(AppSettings.GetString(AppSettingsDbNames.RethinkDbPort).ToInt()).Timeout(AppSettings.GetString(AppSettingsDbNames.RethinkDbTimeout).ToInt()).Db(AppSettings.GetString(AppSettingsDbNames.RethinkDbDatabase)).Connect();
            CosClient = new CosClient
                        {
                            AppId = AppSettings.Get<int>(AppSettingsCosNames.AppId),
                            SecretId = AppSettings.GetString(AppSettingsCosNames.SecretId),
                            SecretKey = AppSettings.GetString(AppSettingsCosNames.SecretKey),
                            ApiUrl = AppSettings.GetString(AppSettingsCosNames.ApiUrl),
                            Bucket = AppSettings.GetString(AppSettingsCosNames.Bucket)
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