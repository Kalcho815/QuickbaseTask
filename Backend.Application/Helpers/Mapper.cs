using Backend.Models;
using Backend.Models.DTOs;

namespace Backend.Helpers
{
    public static class Mapper
    {
        public static CountryPopulation MapToCountryPopulation(CountryPopulationDTO dto)
        {
            return new CountryPopulation
            {
                CountryName = dto.CountryName,
                Population = dto.Population
            };
        }

        public static IEnumerable<CountryPopulation>? MapToCountryPopulations(IEnumerable<CountryPopulationDTO> dtos)
        {
            return dtos.Select(MapToCountryPopulation);
        }
    }
}
