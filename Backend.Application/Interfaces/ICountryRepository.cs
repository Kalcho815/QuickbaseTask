using Backend.Models.DTOs;

namespace Backend
{
    public interface ICountryRepository
    {
        public Task<IEnumerable<CountryPopulationDTO>?> GetCountryPopulationsAsync();
    }
}