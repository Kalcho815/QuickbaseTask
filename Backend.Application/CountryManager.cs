using Backend.Helpers;
using Backend.Interfaces;
using Backend.Models;
using Backend.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Backend
{
    public class CountryManager : ICountryManager
    {
        private readonly ILogger _logger;
        private readonly ICountryRepository _countryRepository;
        private readonly IStatService _statService;

        public CountryManager(ILogger<CountryManager> logger, ICountryRepository countryRepository, IStatService statService) 
        {
            _logger = logger;
            _countryRepository = countryRepository;
            _statService = statService;
        }

        public async Task<ApiResult<CountryPopulationsResponse>> GetCountryPopulationsAsync()
        {
            var result = new ApiResult<CountryPopulationsResponse>();

            var dbResult = await _countryRepository.GetCountryPopulationsAsync();

            if (dbResult == null)
            {
                _logger.LogError("Failed to retrieve country populations from the database");

                result.IsSuccessful = false;
                result.StatusCode = (int)HttpStatusCode.InternalServerError;
                result.Errors = new List<ProblemDetails>
                {
                    new ProblemDetails
                    {
                        Title = "Database connection error",
                        Detail = "Failed to establish a connection to the database or execute the query",
                        Status = (int)HttpStatusCode.InternalServerError
                    }
                };
                return result;
            }

            var populationsApiResult = await _statService.GetCountryPopulationsAsync();

            if (populationsApiResult == null)
            {
                _logger.LogError("Failed to retrieve country populations from the external API");

                result.IsSuccessful = false;
                result.StatusCode = (int)HttpStatusCode.InternalServerError;
                result.Errors = new List<ProblemDetails>
                {
                    new ProblemDetails
                    {
                        Title = "External API error",
                        Detail = "Failed to retrieve data from the external statistics API",
                        Status = (int)HttpStatusCode.InternalServerError
                    }
                };
                return result;
            }

            Dictionary<string, CountryPopulationDTO>? mergedData = MergePopulationData(dbResult, populationsApiResult);
            if(mergedData == null)
            {
                _logger.LogError("Failed to merge population data from database and external API");
                result.IsSuccessful = false;
                result.StatusCode = (int)HttpStatusCode.InternalServerError;
                result.Errors = new List<ProblemDetails>
                {
                    new ProblemDetails
                    {
                        Title = "Data merging error",
                        Detail = "An error occurred while merging population data from the database and external API",
                        Status = (int)HttpStatusCode.InternalServerError
                    }
                };
                return result;
            }

            _logger.LogInformation("Mapping merged data, Merged data count {MergedDataCount}", mergedData.Count);
            var countryPopulations = Mapper.MapToCountryPopulations(mergedData.Values);

            result.Data = new CountryPopulationsResponse { CountryPopulations = countryPopulations };
            result.IsSuccessful = true;
            result.StatusCode = (int)HttpStatusCode.OK;

            return result;
        }

        private Dictionary<string, CountryPopulationDTO>? MergePopulationData(IEnumerable<CountryPopulationDTO> dbResult, IEnumerable<CountryPopulationDTO> populationsApiResult)
        {
            _logger.LogInformation("Merging population data from database and external API");
            var mergedData = new Dictionary<string, CountryPopulationDTO>(StringComparer.OrdinalIgnoreCase);

            //could also be done with linq
            try
            {
                foreach (var item in populationsApiResult)
                {
                    var countryName = NormaliseCountryName(item.CountryName);
                    mergedData[countryName] = item;
                }

                foreach (var item in dbResult)
                {
                    var countryName = NormaliseCountryName(item.CountryName);
                    mergedData[countryName] = item;
                }
                    
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error merging population data from database and external API");
                return null;
            }
            
            return mergedData;
        }

        private static string NormaliseCountryName(string countryName)
        {
            //normally this could be solved with country codes, but since we dont have them in the db im doing it manually
            if (string.IsNullOrWhiteSpace(countryName))
                return string.Empty;

            var normalized = countryName.Trim().ToLower();

            return normalized switch
            {
                "united states of america" => "usa",
                "u.s.a" => "usa",
                "u.s.a." => "usa",
                "us" => "usa",
                "u.s." => "usa",
                "united states" => "usa",
                _ => normalized
            };
        }
    }
}
