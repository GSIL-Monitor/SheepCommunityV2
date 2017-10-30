using System.Net;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.Text;
using Sina.Weibo;

namespace Sheep.Tests.Social.Sina
{
    [TestFixture]
    public class WeiboClientTest : WeiboClientTestBase
    {
        [Test]
        public void GetCountires()
        {
            var response = WeiboClient.Get(new GetCountryRequest
                                           {
                                               AccessToken = "2.00WZqRJG3udz3D7452ba63660T_zbJ"
                                           });
            if (response.ErrorCode != 0)
            {
                throw new HttpError(HttpStatusCode.BadRequest, response.Error, response.ErrorDescription);
            }
            response.PrintDump();
        }

        [Test]
        public void GetProvinces()
        {
            var response = WeiboClient.Get(new GetProvinceRequest
                                           {
                                               AccessToken = "2.00WZqRJG3udz3D7452ba63660T_zbJ",
                                               Country = "001"
                                           });
            if (response.ErrorCode != 0)
            {
                throw new HttpError(HttpStatusCode.BadRequest, response.Error, response.ErrorDescription);
            }
            response.PrintDump();
        }

        [Test]
        public void GetCities()
        {
            var response = WeiboClient.Get(new GetCityRequest
                                           {
                                               AccessToken = "2.00WZqRJG3udz3D7452ba63660T_zbJ",
                                               Province = "001031"
                                           });
            if (response.ErrorCode != 0)
            {
                throw new HttpError(HttpStatusCode.BadRequest, response.Error, response.ErrorDescription);
            }
            response.PrintDump();
        }
    }
}