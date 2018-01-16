using System.Net;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.Text;
using Sheep.Common.Settings;
using Sheep.Model.Geo.Entities;
using Sheep.Model.Geo.Repositories;
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

        [Test]
        public void PopulateGeo()
        {
            var countryRepository = new RethinkDbCountryRepository(Conn, AppSettings.GetString(AppSettingsDbNames.RethinkDbShards).ToInt(), AppSettings.GetString(AppSettingsDbNames.RethinkDbReplicas).ToInt(), true);
            var stateRepository = new RethinkDbStateRepository(Conn, AppSettings.GetString(AppSettingsDbNames.RethinkDbShards).ToInt(), AppSettings.GetString(AppSettingsDbNames.RethinkDbReplicas).ToInt(), true);
            var cityRepository = new RethinkDbCityRepository(Conn, AppSettings.GetString(AppSettingsDbNames.RethinkDbShards).ToInt(), AppSettings.GetString(AppSettingsDbNames.RethinkDbReplicas).ToInt(), true);
            countryRepository.Clear();
            stateRepository.Clear();
            cityRepository.Clear();
            var countryResponse = WeiboClient.Get(new GetCountryRequest
                                                  {
                                                      AccessToken = "2.00WZqRJG3udz3D7452ba63660T_zbJ"
                                                  });
            foreach (var countryPair in countryResponse.Countries)
            {
                countryRepository.CreateCountry(new Country
                                                {
                                                    Id = countryPair.Key,
                                                    Name = countryPair.Value
                                                });
                var provinceResponse = WeiboClient.Get(new GetProvinceRequest
                                                       {
                                                           AccessToken = "2.00WZqRJG3udz3D7452ba63660T_zbJ",
                                                           Country = countryPair.Key
                                                       });
                foreach (var provincePair in provinceResponse.Provinces)
                {
                    stateRepository.CreateState(new State
                                                {
                                                    Id = provincePair.Key,
                                                    CountryId = countryPair.Key,
                                                    Name = provincePair.Value
                                                });
                    var cityResponse = WeiboClient.Get(new GetCityRequest
                                                       {
                                                           AccessToken = "2.00WZqRJG3udz3D7452ba63660T_zbJ",
                                                           Province = provincePair.Key
                                                       });
                    foreach (var cityPair in cityResponse.Cities)
                    {
                        cityRepository.CreateCity(new City
                                                  {
                                                      Id = cityPair.Key,
                                                      StateId = provincePair.Key,
                                                      Name = cityPair.Value
                                                  });
                    }
                }
            }
        }

        [Test]
        public void GetUser()
        {
            var response = WeiboClient.Get(new ShowUserRequest
                                           {
                                               AccessToken = "2.00WZqRJG3udz3D7452ba63660T_zbJ",
                                               UserId = "1905070401"
                                           });
            response.PrintDump();
        }
    }
}