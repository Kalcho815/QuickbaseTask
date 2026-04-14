using Backend.Models.DTOs;

namespace Backend;

public interface IStatService
{
    public IEnumerable<CountryPopulationDTO> GetCountryPopulations();
    public Task<IEnumerable<CountryPopulationDTO>> GetCountryPopulationsAsync();
}
