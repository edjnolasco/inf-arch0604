using INF._5120.Arch0604.Application.Common;
using INF._5120.Arch0604.Application.DTOs.CountryDTOs;
using INF._5120.Arch0604.Application.Services;
using INF._5120.Arch0604.Domain.Entities;
using INF._5120.Arch0604.Infrastructure.Persistence.Context;
using INF._5120.Arch0604.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace INF._5120.Arch0604.Application.Tests.Services
{
    public class CountryServiceTests
    {
        private readonly INF5120DbContext _context;
        private readonly CountryService _service;

        public CountryServiceTests()
        {
            var options = new DbContextOptionsBuilder<INF5120DbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new INF5120DbContext(options);

            _context.Countries.AddRange(
                new Country
                {
                    Id = 1,
                    Description = "TestCountry1",
                    IsoNum = 999,
                    IsoA2 = "TC",
                    IsoA3 = "TST",
                    IsEnable = true,
                    CreatedDate = DateTime.UtcNow
                },
                new Country
                {
                    Id = 2,
                    Description = "TestCountry2",
                    IsoNum = 444,
                    IsoA2 = "TS",
                    IsoA3 = "TCT",
                    IsEnable = true,
                    CreatedDate = DateTime.UtcNow
                });

            _context.SaveChanges();

            var repository = new CountryRepository(_context);
            _service = new CountryService(repository);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnOkWithCountries()
        {
            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.True(result.Success);
            Assert.Equal(ServiceErrorType.None, result.ErrorType);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnOk_WhenCountryExists()
        {
            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(ServiceErrorType.None, result.ErrorType);
            Assert.NotNull(result.Data);
            Assert.Equal(1, result.Data.Id);
            Assert.Equal("TestCountry1", result.Data.Description);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNotFound_WhenCountryDoesNotExist()
        {
            // Act
            var result = await _service.GetByIdAsync(999);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(ServiceErrorType.NotFound, result.ErrorType);
            Assert.Null(result.Data);
            Assert.False(string.IsNullOrWhiteSpace(result.Message));
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnOk_WhenRequestIsValid()
        {
            // Arrange
            var request = new CreateCountryRequestDto
            {
                Description = "Dominican Republic",
                IsoNum = 214,
                IsoA2 = "do",
                IsoA3 = "dom",
                IsEnable = true
            };

            // Act
            var result = await _service.CreateAsync(request);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(ServiceErrorType.None, result.ErrorType);
            Assert.NotNull(result.Data);
            Assert.Equal("Dominican Republic", result.Data.Description);
            Assert.Equal("DO", result.Data.IsoA2);
            Assert.Equal("DOM", result.Data.IsoA3);

            var entity = await _context.Countries.FirstOrDefaultAsync(c => c.Description == "Dominican Republic", cancellationToken: TestContext.Current.CancellationToken);
            Assert.NotNull(entity);
            Assert.Equal(214, entity.IsoNum);
            Assert.Equal("DO", entity.IsoA2);
            Assert.Equal("DOM", entity.IsoA3);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnConflict_WhenDuplicateExists()
        {
            // Arrange
            var request = new CreateCountryRequestDto
            {
                Description = "TestCountry1",
                IsoNum = 999,
                IsoA2 = "TC",
                IsoA3 = "TST",
                IsEnable = true
            };

            // Act
            var result = await _service.CreateAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(ServiceErrorType.Conflict, result.ErrorType);
            Assert.Null(result.Data);
            Assert.False(string.IsNullOrWhiteSpace(result.Message));
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnOk_WhenRequestIsValid()
        {
            // Arrange
            var request = new UpdateCountryRequestDto
            {
                Id = 1,
                Description = "CountryUpdated",
                IsoNum = 840,
                IsoA2 = "us",
                IsoA3 = "usa",
                IsEnable = true
            };

            // Act
            var result = await _service.UpdateAsync(1, request);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(ServiceErrorType.None, result.ErrorType);
            Assert.True(result.Data);

            var entity = await _context.Countries.FindAsync([1], TestContext.Current.CancellationToken);
            Assert.NotNull(entity);
            Assert.Equal("CountryUpdated", entity.Description);
            Assert.Equal(840, entity.IsoNum);
            Assert.Equal("US", entity.IsoA2);
            Assert.Equal("USA", entity.IsoA3);
            Assert.NotNull(entity.UpdatedDate);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnValidation_WhenRouteIdDoesNotMatchRequestId()
        {
            // Arrange
            var request = new UpdateCountryRequestDto
            {
                Id = 2,
                Description = "CountryUpdated",
                IsoNum = 840,
                IsoA2 = "US",
                IsoA3 = "USA",
                IsEnable = true
            };

            // Act
            var result = await _service.UpdateAsync(1, request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(ServiceErrorType.Validation, result.ErrorType);
            Assert.False(result.Data);
            Assert.False(string.IsNullOrWhiteSpace(result.Message));
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNotFound_WhenCountryDoesNotExist()
        {
            // Arrange
            var request = new UpdateCountryRequestDto
            {
                Id = 999,
                Description = "CountryUpdated",
                IsoNum = 840,
                IsoA2 = "US",
                IsoA3 = "USA",
                IsEnable = true
            };

            // Act
            var result = await _service.UpdateAsync(999, request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(ServiceErrorType.NotFound, result.ErrorType);
            Assert.False(result.Data);
            Assert.False(string.IsNullOrWhiteSpace(result.Message));
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnConflict_WhenDuplicateExists()
        {
            // Arrange
            var request = new UpdateCountryRequestDto
            {
                Id = 1,
                Description = "TestCountry2",
                IsoNum = 444,
                IsoA2 = "TS",
                IsoA3 = "TCT",
                IsEnable = true
            };

            // Act
            var result = await _service.UpdateAsync(1, request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(ServiceErrorType.Conflict, result.ErrorType);
            Assert.False(result.Data);
            Assert.False(string.IsNullOrWhiteSpace(result.Message));
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnOk_WhenCountryExists()
        {
            // Act
            var result = await _service.DeleteAsync(1);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(ServiceErrorType.None, result.ErrorType);
            Assert.True(result.Data);

            var entity = await _context.Countries.FindAsync([1], TestContext.Current.CancellationToken);
            Assert.Null(entity);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnNotFound_WhenCountryDoesNotExist()
        {
            // Act
            var result = await _service.DeleteAsync(999);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(ServiceErrorType.NotFound, result.ErrorType);
            Assert.False(result.Data);
            Assert.False(string.IsNullOrWhiteSpace(result.Message));
        }
    }
}