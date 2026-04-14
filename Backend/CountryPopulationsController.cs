using Backend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Backend
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryPopulationsController : ControllerBase
    {
        private readonly ILogger<CountryPopulationsController> _logger;
        private readonly ICountryManager _countryService;

        public CountryPopulationsController(ILogger<CountryPopulationsController> logger, ICountryManager countryService)
        {
            _logger = logger;
            _countryService = countryService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Received request to get country populations");
            Stopwatch sw = Stopwatch.StartNew();

            var result = await _countryService.GetCountryPopulationsAsync();
            sw.Stop();

            if (!result.IsSuccessful)
            {
                _logger.LogError("Failed to get country populations, Status Code: {StatusCode}, Errors: {Errors}, Elapsed MS: {ElapsedMS}", result.StatusCode, result.Errors, sw.ElapsedMilliseconds);
                return StatusCode(result.StatusCode, result.Errors);
            }

            _logger.LogInformation("Successfully retrieved country populations, Status Code: {StatusCode}, Elapsed MS: {ElapsedMS}", result.StatusCode, sw.ElapsedMilliseconds);
            return Ok(result);
        }
    }
}
