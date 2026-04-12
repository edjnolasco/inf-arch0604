using INF._5120.Arch0604.Domain.Entities;
using INF._5120.Arch0604.Domain.Tests.Helpers;

namespace INF._5120.Arch0604.Domain.Tests.Entities
{
    public class CountryTests
    {
        [Fact]
        public void AddAndGetCountryTest()
        {
            // Arrange
            var context = TestHelper.GenerateContext();
            var countryExpected = new Country
            {
                Id = 1,
                Description = "TestCountry",
                IsoNum = 999,
                IsoA2 = "TC",
                IsoA3 = "TST",
                IsEnable = true,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.Now
            };

            context.Countries.Add(countryExpected);
            context.SaveChanges();

            // Act
            var countryActual = context.Countries.Find(1)!;

            // Assert
            Assert.Equal(1, countryActual.Id);
            Assert.Equal("TestCountry", countryActual.Description);
            Assert.Equal(999, countryActual.IsoNum);
            Assert.Equal("TC", countryActual.IsoA2);
            Assert.Equal("TST", countryActual.IsoA3);
            Assert.True(countryActual.IsEnable);
            Assert.NotEqual(DateTime.Now, countryActual.CreatedDate);
            Assert.NotEqual(DateTime.Now, countryActual.UpdatedDate);
        }
    }
}