using NUnit.Framework;
using ServiceStack;
using ServiceStack.Configuration;

namespace Sheep.Tests.ServiceInterface.Books
{
    public class BookClientTestBase
    {
        #region 变量

        public IAppSettings AppSettings;
        public IServiceClient ServiceClient;

        #endregion

        #region 启动与结束设置

        [OneTimeSetUp]
        public virtual void OnBeforeTest()
        {
            AppSettings = new AppSettings();
            ServiceClient = new JsonServiceClient("http://apiv2.yangqungongshe.com");
            //ServiceClient = new JsonServiceClient("http://localhost:12345");
            ServiceClient.AddHeader("X-ss-opt", "perm");
            ServiceClient.AddHeader("X-ss-pid", "0GBQbTdaBoNY00RnFW4s");
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