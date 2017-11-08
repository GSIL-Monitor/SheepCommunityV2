using System.IO;
using NUnit.Framework;
using ServiceStack.Extensions;
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
                                                            BizAttribute = "Fuck"
                                                        });
            response.PrintDump();
        }

        [Test]
        public void GetTestFolderStat()
        {
            var response = CosClient.Get("TestCustom", new GetFolderStatRequest());
            response.PrintDump();
        }

        [Test]
        public void DeleteTestFolder()
        {
            var response = CosClient.Post("TestCustom", new DeleteFolderRequest());
            response.PrintDump();
        }

        [Test]
        public void UploadFileToTestFolder()
        {
            var fileContent = File.ReadAllBytes(@"G:\头像—男\1130.jpg");
            var response = CosClient.Post("/test/ok.jpg", new UploadFileRequest
                                                          {
                                                              BizAttribute = "",
                                                              FileContent = fileContent,
                                                              Sha = fileContent.ToSha1HashString(),
                                                              InsertOnly = 0
                                                          });
            response.PrintDump();
        }
    }
}