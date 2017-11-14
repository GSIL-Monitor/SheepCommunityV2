using Sheep.Model.Geo.Entities;
using Sheep.ServiceModel.Countries.Entities;

namespace Sheep.ServiceInterface.Countries.Mappers
{
    public static class CountryToCountryDtoMapper
    {
        public static CountryDto MapToCountryDto(this Country country)
        {
            var countryDto = new CountryDto
                             {
                                 Id = country.Id,
                                 Name = country.Name
                             };
            return countryDto;
        }
    }
}