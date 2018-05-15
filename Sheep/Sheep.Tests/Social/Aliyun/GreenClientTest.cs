using System;
using System.Threading;
using System.Threading.Tasks;
using Aliyun.Green;
using NUnit.Framework;
using ServiceStack.Text;

namespace Sheep.Tests.Social.Aliyun
{
    [TestFixture]
    public class GreenClientTest : GreenClientTestBase
    {
        [Test]
        public async Task TextScan()
        {
            var currentTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var request = new TextScanRequest
                          {
                              BizType = "test",
                              Scenes = new[]
                                       {
                                           "antispam"
                                       },
                              Tasks = new[]
                                      {
                                          new TextScanRequestTask
                                          {
                                              DataId = Guid.NewGuid().ToString("D"),
                                              Content = "习近平总书记永垂不朽！！",
                                              Time = currentTime
                                          },
                                          new TextScanRequestTask
                                          {
                                              DataId = Guid.NewGuid().ToString("D"),
                                              Content = "AK47及炸药",
                                              Time = currentTime
                                          },
                                          new TextScanRequestTask
                                          {
                                              DataId = Guid.NewGuid().ToString("D"),
                                              Content = "男女同同，集片，各种货，，[稀饭]。。avb▲10▲20   去掉  ▲",
                                              Category = "title",
                                              Time = currentTime
                                          },
                                          new TextScanRequestTask
                                          {
                                              DataId = Guid.NewGuid().ToString("D"),
                                              Content = "硬长直——简称——污,A 日韩 云盘 超轻午马 魏鑫",
                                              Category = "title",
                                              Time = currentTime
                                          },
                                          new TextScanRequestTask
                                          {
                                              DataId = Guid.NewGuid().ToString("D"),
                                              Content = "真的猛士，敢于直面惨淡的人生，敢于正视淋漓的鲜血。生活不只眼前的苟且 还有诗和远方！",
                                              Category = "title",
                                              Time = currentTime
                                          },
                                          new TextScanRequestTask
                                          {
                                              DataId = Guid.NewGuid().ToString("D"),
                                              Content = "本校小额贷款，安全、快捷、方便、无抵押，随机随贷，当天放款，上门服务。联系weixin 946932",
                                              Category = "title",
                                              Time = currentTime
                                          }
                                      }
                          };
            var response = await GreenClient.PostAsync(request, null, CancellationToken.None);
            response.PrintDump();
        }
    }
}