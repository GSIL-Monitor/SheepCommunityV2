using NUnit.Framework;
using ServiceStack.Text;
using Tencent.Cos;

namespace Sheep.Tests.Social.Tencent
{
    [TestFixture]
    public class CosClientTest : CosClientTestBase
    {
        [Test]
        public void CreateTestFolder()
        {
            var response = CosClient.Post("TestCustom", new CreateFolderRequest
                                                        {
                                                            Operation = "create",
                                                            BizAttribute = "Fuck"
                                                        });
            response.PrintDump();
        }

        [Test]
        public void GetTestFolderStat()
        {
            var response = CosClient.Get("TestCustom", new GetFolderStatRequest
                                                       {
                                                           Operation = "stat"
                                                       });
            response.PrintDump();
        }

        [Test]
        public void GetCities()
        {
            var response = CosClient.Post("TestCustom", new DeleteFolderRequest
                                                        {
                                                            Operation = "delete"
                                                        });
            response.PrintDump();
        }
    }
}