using INF._5120.Arch0604.Api.Controllers;
using INF._5120.Arch0604.Domain.Entities;
using INF._5120.Arch0604.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace INF._5120.Arch0604.Api.Tests.Controllers
{
    public class CountryControllerTests
    {
        readonly INF5120DbContext _context;

        public CountryControllerTests()
        {
            var options = new DbContextOptionsBuilder<INF5120DbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new INF5120DbContext(options);
            _context.Countries.Add(
                new Country()
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
                new Country()
                {
                    Id = 2,
                    Description = "TestCountry",
                    IsoNum = 444,
                    IsoA2 = "TS",
                    IsoA3 = "TCT",
                    IsEnable = true,
                    CreatedDate = DateTime.UtcNow
                });
            _context.SaveChangesAsync();
        }

        [Fact]
        public async void GetCountryTest()
        {
            // Arrange
            var controller = new CountryController(_context);
            // Act
            var actionResult = await controller.GetCountries();
            // Assert
            var result = (actionResult as OkObjectResult)?.Value as IEnumerable<Country>;
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }


    }
}