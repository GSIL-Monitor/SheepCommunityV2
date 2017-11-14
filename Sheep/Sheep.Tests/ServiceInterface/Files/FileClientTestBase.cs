using NUnit.Framework;
using ServiceStack;
using ServiceStack.Configuration;

namespace Sheep.Tests.ServiceInterface.Files
{
    public class FileClientTestBase
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
            ServiceClient = new JsonServiceClient("http://localhost:12345");
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