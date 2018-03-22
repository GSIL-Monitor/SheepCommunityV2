using System.Collections.Generic;
using System.Net;
using JPush.Push;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.Text;

namespace Sheep.Tests.Social.JPush
{
    [TestFixture]
    public class PushClientTest : PushClientTestBase
    {
        [Test]
        public void Push()
        {
            var cidResponse = PushClient.Get(new CidRequest
                                             {
                                                 Count = 2
                                             });
            if (cidResponse.Cids == null)
            {
                throw new HttpError(HttpStatusCode.BadRequest);
            }
            cidResponse.PrintDump();
            var response = PushClient.Post(new PushRequest
                                           {
                                               CId = cidResponse.Cids.FirstNonDefault(),
                                               Platform = new[]
                                                          {
                                                              "android",
                                                              "ios"
                                                          },
                                               Audience = "all",
                                               Notification = new Notification
                                                              {
                                                                  Android = new NotificationAndroid
                                                                            {
                                                                                Alert = "我是黄假菌，我是一篇帖子，一点要点开看看哦！！",
                                                                                AlertType = 2 | 4,
                                                                                Extras = new Dictionary<string, object>
                                                                                         {
                                                                                             {
                                                                                                 "PostId", "123456"
                                                                                             }
                                                                                         }
                                                                            },
                                                                  IOS = new NotificationIOS
                                                                        {
                                                                            Alert = "我是黄假菌，我是一篇帖子，一点要点开看看哦！！",
                                                                            Extras = new Dictionary<string, object>
                                                                                     {
                                                                                         {
                                                                                             "PostId", "123456"
                                                                                         }
                                                                                     }
                                                                        }
                                                              }
                                           });
            if (response.Error != null)
            {
                throw new HttpError(HttpStatusCode.BadRequest, response.Error.Message);
            }
            response.PrintDump();
        }
    }
}