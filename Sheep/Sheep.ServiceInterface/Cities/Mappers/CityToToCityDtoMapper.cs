using Sheep.Model.Geo.Entities;
using Sheep.ServiceModel.Cities.Entities;

namespace Sheep.ServiceInterface.Cities.Mappers
{
    public static class CityToToCityDtoMapper
    {
        public static CityDto MapToCityDto(this City city)
        {
            var cityDto = new CityDto
                          {
                              Id = city.Id,
                              Name = city.Name
                          };
            return cityDto;
        }
    }
}