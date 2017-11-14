using System.IO;
using NUnit.Framework;
using ServiceStack.Text;
using Sheep.ServiceModel.Files;

namespace Sheep.Tests.ServiceInterface.Files
{
    [TestFixture]
    public class FileClientTest : FileClientTestBase
    {
        [Test]
        public void CreateTestFolder()
        {
            var response = ServiceClient.PostFile<FileUploadImageResponse>("/files/image", File.OpenRead(@"G:\头像—男\1100.jpg"), "1100.jpg", "image/jpeg");
            response.PrintDump();
        }
    }
}