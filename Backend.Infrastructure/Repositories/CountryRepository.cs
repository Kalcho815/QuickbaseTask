using Backend.Models.DTOs;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Backend.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly IDbManager _dbManager;
        private readonly ILogger<CountryRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly string? _countryConnectionString;

        public CountryRepository(IDbManager dbManager, ILogger<CountryRepository> logger, IConfiguration configuration)
        {
            _dbManager = dbManager;
            _logger = logger;
            _configuration = configuration;
            if (!string.IsNullOrEmpty(_configuration["ConnectionString"]))
            {
                _countryConnectionString = _configuration["ConnectionString"]!;
            }
            
        }

        public async Task<IEnumerable<CountryPopulationDTO>?> GetCountryPopulationsAsync() //not sure if i was supposed to use ef core, realised too late :/
        {
            if(string.IsNullOrEmpty(_countryConnectionString))
            {
                _logger.LogError("Database connection string is not configured");
                return null;
            }

            using (var connection = _dbManager.GetConnection(_countryConnectionString))
            {
                IEnumerable<CountryPopulationDTO> result;
                if (connection == null)
                {
                    _logger.LogError("Failed to establish a connection to the database, Connection string: {ConnectionString}", _countryConnectionString);
                    return null;
                }

                try
                {
                    Stopwatch sw = Stopwatch.StartNew();

                    CommandDefinition command = new CommandDefinition(@"SELECT co.CountryName, SUM(CAST(ct.Population AS bigint)) AS Population from country co
	                                                                    JOIN [State] st ON st.CountryId = co.CountryId
	                                                                    JOIN City ct ON ct.StateId = st.StateId
	                                                                    GROUP BY co.CountryName ", commandType: System.Data.CommandType.Text); //normally this would be a stored procedure
                    result = await connection.QueryAsync<CountryPopulationDTO>(command);

                    sw.Stop();
                    _logger.LogInformation("Finished country populations from the database", [sw.ElapsedMilliseconds]);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error executing query to get country populations");
                    return null;
                }

                return result;
            }
        }
    }
}
