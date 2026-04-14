using Backend.Models.DTOs;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace Backend.Tests
{
    public class CountryManagerTests
    {
        private readonly Mock<ILogger<CountryManager>> _loggerMock;
        private readonly Mock<ICountryRepository> _countryRepositoryMock;
        private readonly Mock<IStatService> _statServiceMock;

        public CountryManagerTests()
        {
            _loggerMock = new Mock<ILogger<CountryManager>>();
            _countryRepositoryMock = new Mock<ICountryRepository>();
            _statServiceMock = new Mock<IStatService>();
        }

        private CountryManager CreateService()
        {
            return new CountryManager(
                _loggerMock.Object,
                _countryRepositoryMock.Object,
                _statServiceMock.Object);
        }

        [Fact]
        public async Task GetCountryPopulationsAsync_WhenDatabaseReturnsNull_ReturnsInternalServerError()
        {
            _countryRepositoryMock
                .Setup(r => r.GetCountryPopulationsAsync())
                .ReturnsAsync((IEnumerable<CountryPopulationDTO>?)null);

            var service = CreateService();

            var result = await service.GetCountryPopulationsAsync();

            Assert.False(result.IsSuccessful);
            Assert.Equal((int)HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.NotNull(result.Errors);
            Assert.Single(result.Errors);

            var error = result.Errors.First();
            Assert.Equal("Database connection error", error.Title);
            Assert.Equal((int)HttpStatusCode.InternalServerError, error.Status);

            _statServiceMock.Verify(s => s.GetCountryPopulationsAsync(), Times.Never);
        }

        [Fact]
        public async Task GetCountryPopulationsAsync_WhenApiReturnsNull_ReturnsInternalServerError()
        {
            _countryRepositoryMock
            .Setup(r => r.GetCountryPopulationsAsync())
            .ReturnsAsync(new List<CountryPopulationDTO>
            {
                    new CountryPopulationDTO { CountryName = "Bulgaria", Population = 6800000 }
            });

            _statServiceMock
                .Setup(s => s.GetCountryPopulationsAsync())
                .ReturnsAsync((IEnumerable<CountryPopulationDTO>?)null);

            var service = CreateService();

            var result = await service.GetCountryPopulationsAsync();

            Assert.False(result.IsSuccessful);
            Assert.Equal((int)HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.NotNull(result.Errors);
            Assert.Single(result.Errors);

            var error = result.Errors.First();
            Assert.Equal("External API error", error.Title);
            Assert.Equal((int)HttpStatusCode.InternalServerError, error.Status);
        }

        [Fact]
        public async Task GetCountryPopulationsAsync_WhenBothSourcesReturnData_ReturnsOkResult()
        {
            _countryRepositoryMock
                .Setup(r => r.GetCountryPopulationsAsync())
                .ReturnsAsync(new List<CountryPopulationDTO>
                {
                    new CountryPopulationDTO { CountryName = "Bulgaria", Population = 6800000 }
                });

            _statServiceMock
                .Setup(s => s.GetCountryPopulationsAsync())
                .ReturnsAsync(new List<CountryPopulationDTO>
                {
                    new CountryPopulationDTO { CountryName = "Germany", Population = 83000000 }
                });

            var service = CreateService();

            var result = await service.GetCountryPopulationsAsync();

            Assert.True(result.IsSuccessful);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.NotNull(result.Data.CountryPopulations);
            Assert.Equal(2, result.Data.CountryPopulations.Count());
        }

    }
}