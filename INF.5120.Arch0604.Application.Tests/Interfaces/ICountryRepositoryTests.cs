using INF._5120.Arch0604.Application.Interfaces;
using INF._5120.Arch0604.Application.Tests.Mocks;

namespace INF._5120.Arch0604.Application.Tests.Interfaces
{
    public class ICountryRepositoryTests
    {
        [Fact]
        public void OnInsertCountryShouldReturnIsNotValid()
        {
            //Arrange
            ICountryRepository countryRepository = new CountryRepositoryMock();

            var countryService = new CountryService(countryRepository);

            //Act
            var (isValid, _) = countryService.InsertNewCountry(
                description: "Description",
                isoA2: "null",
                isoA3: "isoA3"
            );

            //Assert
            Assert.False(isValid);
        }
    }
}