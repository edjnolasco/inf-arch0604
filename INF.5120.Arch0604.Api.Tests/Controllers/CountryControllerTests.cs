using INF._5120.Arch0604.Api.Controllers;
using INF._5120.Arch0604.Application.DTOs.CountryDTOs;
using INF._5120.Arch0604.Application.Interfaces;
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
        }

        private CountryController BuildController()
        {
            ICountryRepository repository = new CountryRepository(_context);
            ICountryService service = new CountryService(repository);
            return new CountryController(service);
        }

        [Fact]
        public async Task GetCountries_ShouldReturnOkWithCountries()
        {
            // Arrange
            var controller = BuildController();

            // Act
            ActionResult<IEnumerable<CountryResponseDto>> actionResult = await controller.GetCountries();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var result = Assert.IsAssignableFrom<IEnumerable<CountryResponseDto>>(okResult.Value);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetCountryById_ShouldReturnOk_WhenCountryExists()
        {
            // Arrange
            var controller = BuildController();

            // Act
            ActionResult<CountryResponseDto> actionResult = await controller.GetCountryById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var result = Assert.IsType<CountryResponseDto>(okResult.Value);

            Assert.Equal(1, result.Id);
            Assert.Equal("TestCountry1", result.Description);
            Assert.Equal("TC", result.IsoA2);
            Assert.Equal("TST", result.IsoA3);
        }

        [Fact]
        public async Task GetCountryById_ShouldReturnNotFound_WhenCountryDoesNotExist()
        {
            // Arrange
            var controller = BuildController();

            // Act
            ActionResult<CountryResponseDto> actionResult = await controller.GetCountryById(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.NotNull(notFoundResult.Value);
        }

        [Fact]
        public async Task CreateCountry_ShouldReturnCreatedAtAction_WhenRequestIsValid()
        {
            // Arrange
            var controller = BuildController();

            var request = new CreateCountryRequestDto
            {
                Description = "Dominican Republic",
                IsoNum = 214,
                IsoA2 = "do",
                IsoA3 = "dom",
                IsEnable = true
            };

            // Act
            ActionResult<CountryResponseDto> actionResult = await controller.CreateCountry(request);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            Assert.Equal(nameof(CountryController.GetCountryById), createdResult.ActionName);

            var result = Assert.IsType<CountryResponseDto>(createdResult.Value);
            Assert.Equal("Dominican Republic", result.Description);
            Assert.Equal("DO", result.IsoA2);
            Assert.Equal("DOM", result.IsoA3);

            var entity = await _context.Countries.FirstOrDefaultAsync(c => c.Description == "Dominican Republic", cancellationToken: TestContext.Current.CancellationToken);
            Assert.NotNull(entity);
            Assert.Equal("DO", entity.IsoA2);
            Assert.Equal("DOM", entity.IsoA3);
        }

        [Fact]
        public async Task CreateCountry_ShouldReturnConflict_WhenCountryAlreadyExists()
        {
            // Arrange
            var controller = BuildController();

            var request = new CreateCountryRequestDto
            {
                Description = "TestCountry1",
                IsoNum = 999,
                IsoA2 = "TC",
                IsoA3 = "TST",
                IsEnable = true
            };

            // Act
            ActionResult<CountryResponseDto> actionResult = await controller.CreateCountry(request);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(actionResult.Result);
            Assert.NotNull(conflictResult.Value);
        }

        [Fact]
        public async Task CreateCountry_ShouldReturnValidationProblem_WhenModelStateIsInvalid()
        {
            // Arrange
            var controller = BuildController();
            controller.ModelState.AddModelError("Description", "The Description field is required.");

            var request = new CreateCountryRequestDto
            {
                Description = "Invalid",
                IsoNum = 214,
                IsoA2 = "DO",
                IsoA3 = "DOM",
                IsEnable = true
            };

            // Act
            ActionResult<CountryResponseDto> actionResult = await controller.CreateCountry(request);

            // Assert
            var validationResult = Assert.IsType<ObjectResult>(actionResult.Result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(validationResult.Value);

            Assert.NotNull(problemDetails.Errors);
            Assert.True(problemDetails.Errors.ContainsKey("Description"));
        }

        [Fact]
        public async Task UpdateCountry_ShouldReturnNoContent_WhenRequestIsValid()
        {
            // Arrange
            var controller = BuildController();

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
            IActionResult actionResult = await controller.UpdateCountry(1, request);

            // Assert
            Assert.IsType<NoContentResult>(actionResult);

            var entity = await _context.Countries.FindAsync([1], TestContext.Current.CancellationToken);
            Assert.NotNull(entity);
            Assert.Equal("CountryUpdated", entity.Description);
            Assert.Equal(840, entity.IsoNum);
            Assert.Equal("US", entity.IsoA2);
            Assert.Equal("USA", entity.IsoA3);
            Assert.NotNull(entity.UpdatedDate);
        }

        [Fact]
        public async Task UpdateCountry_ShouldReturnBadRequest_WhenRouteIdDoesNotMatchRequestId()
        {
            // Arrange
            var controller = BuildController();

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
            IActionResult actionResult = await controller.UpdateCountry(1, request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateCountry_ShouldReturnNotFound_WhenCountryDoesNotExist()
        {
            // Arrange
            var controller = BuildController();

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
            IActionResult actionResult = await controller.UpdateCountry(999, request);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult);
            Assert.NotNull(notFoundResult.Value);
        }

        [Fact]
        public async Task UpdateCountry_ShouldReturnConflict_WhenAnotherCountryAlreadyExistsWithSameUniqueFields()
        {
            // Arrange
            var controller = BuildController();

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
            IActionResult actionResult = await controller.UpdateCountry(1, request);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(actionResult);
            Assert.NotNull(conflictResult.Value);
        }

        [Fact]
        public async Task UpdateCountry_ShouldReturnValidationProblem_WhenModelStateIsInvalid()
        {
            // Arrange
            var controller = BuildController();
            controller.ModelState.AddModelError("Description", "The Description field is required.");

            var request = new UpdateCountryRequestDto
            {
                Id = 1,
                Description = "Invalid",
                IsoNum = 840,
                IsoA2 = "US",
                IsoA3 = "USA",
                IsEnable = true
            };

            // Act
            IActionResult actionResult = await controller.UpdateCountry(1, request);

            // Assert
            var validationResult = Assert.IsType<ObjectResult>(actionResult);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(validationResult.Value);

            Assert.NotNull(problemDetails.Errors);
            Assert.True(problemDetails.Errors.ContainsKey("Description"));
        }

        [Fact]
        public async Task DeleteCountry_ShouldReturnNoContent_WhenCountryExists()
        {
            // Arrange
            var controller = BuildController();

            // Act
            IActionResult actionResult = await controller.DeleteCountry(1);

            // Assert
            Assert.IsType<NoContentResult>(actionResult);

            var entity = await _context.Countries.FindAsync([1], TestContext.Current.CancellationToken);
            Assert.Null(entity);
        }

        [Fact]
        public async Task DeleteCountry_ShouldReturnNotFound_WhenCountryDoesNotExist()
        {
            // Arrange
            var controller = BuildController();

            // Act
            IActionResult actionResult = await controller.DeleteCountry(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult);
            Assert.NotNull(notFoundResult.Value);
        }
    }
}