using System;
using System.Net;
using Aliyun.Green;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.Text;

namespace Sheep.Tests.Social.Aliyun
{
    [TestFixture]
    public class GreenClientTest : GreenClientTestBase
    {
        [Test]
        public void TextScan()
        {
            var response = GreenClient.Post(new TextScanRequest
                                            {
                                                BizType = "Text",
                                                Scenes = new[]
                                                         {
                                                             "antispam"
                                                         },
                                                Tasks = new[]
                                                        {
                                                            new TextScanTask
                                                            {
                                                                DataId = Guid.NewGuid().ToString("N"),
                                                                Content = "非常爽歪歪，真的好爽！超屌的",
                                                                Time = DateTime.Now.Millisecond
                                                            }
                                                        }
                                            });
            if (response.Code != 200)
            {
                throw new HttpError(HttpStatusCode.BadRequest, response.Message);
            }
            response.PrintDump();
        }
    }
}