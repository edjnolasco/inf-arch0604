using INF._5120.Arch0604.Api.Controllers;
using INF._5120.Arch0604.Application.DTOs.CountryDTOs;
using INF._5120.Arch0604.Application.Services;
using INF._5120.Arch0604.Domain.Entities;
using INF._5120.Arch0604.Infrastructure.Persistence.Context;
using INF._5120.Arch0604.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace INF._5120.Arch0604.Api.Tests.Controllers
{
    public class CountryControllerTests
    {
        private readonly INF5120DbContext _context;

        public CountryControllerTests()
        {
            var options = new DbContextOptionsBuilder<INF5120DbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new INF5120DbContext(options);

            _context.Countries.Add(
                new Country
                {
                    Id = 1,
                    Description = "TestCountry1",
                    IsoNum = 999,
                    IsoA2 = "TC",
                    IsoA3 = "TST",
                    IsEnable = true,
                    CreatedDate = DateTime.UtcNow
                });

            _context.Countries.Add(
                new Country
                {
                    Id = 2,
                    Description = "TestCountry",
                    IsoNum = 444,
                    IsoA2 = "TS",
                    IsoA3 = "TCT",
                    IsEnable = true,
                    CreatedDate = DateTime.UtcNow
                });

            _context.SaveChanges();
        }

        [Fact]
        public async Task GetCountryTest()
        {
            // Arrange
            var repository = new CountryRepository(_context);
            var service = new CountryService(repository);
            var controller = new CountryController(service);

            // Act
            ActionResult<IEnumerable<CountryResponseDto>> actionResult = await controller.GetCountries();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var result = Assert.IsAssignableFrom<IEnumerable<CountryResponseDto>>(okResult.Value);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }
}