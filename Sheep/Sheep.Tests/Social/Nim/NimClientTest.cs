using System.Collections.Generic;
using System.Net;
using Netease.Nim;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.Text;

namespace Sheep.Tests.Social.Nim
{
    [TestFixture]
    public class NimClientTest : NimClientTestBase
    {
        [Test]
        public void TeamQuery()
        {
            var response = NimClient.Post(new TeamQueryRequest
                                          {
                                              TeamIds = new List<string>
                                                        {
                                                            "192612409",
                                                            "191259679",
                                                            "204726623",
                                                            "263894346",
                                                            "263955117",
                                                            "237227573"
                                                        },
                                              Operation = 1
                                          });
            if (response.Code != 200)
            {
                throw new HttpError(HttpStatusCode.BadRequest, response.Description.ToString());
            }
            response.PrintDump();
        }

        [Test]
        public void TeamAdd()
        {
            var response = NimClient.Post(new TeamAddMemberRequest
                                          {
                                              TeamId = "400006157",
                                              OwnerAccountId = "1",
                                              MemberAccountIds = new List<string>
                                                                 {
                                                                     "2",
                                                                     "3"
                                                                 },
                                              MessageAgree = 0,
                                              Message = "欢迎加入羊群公社！"
                                          });
            if (response.Code != 200)
            {
                throw new HttpError(HttpStatusCode.BadRequest, response.Description.ToString());
            }
            response.PrintDump();
        }
    }
}