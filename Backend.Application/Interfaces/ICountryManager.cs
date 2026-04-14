using Backend.Models;

namespace Backend.Interfaces
{
    public interface ICountryManager
    {
        public Task<ApiResult<CountryPopulationsResponse>> GetCountryPopulationsAsync();
    }
}
