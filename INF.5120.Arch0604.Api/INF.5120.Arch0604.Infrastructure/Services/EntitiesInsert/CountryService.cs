using INF._5120.Arch0604.Application.Interfaces;

namespace INF._5120.Arch0604.Infrastructure.Services.EntitiesInsert
{
    public class CountryService(ICountryRepository countryRepository)
    {
        private readonly ICountryRepository CountryRepository = countryRepository;

        public static (bool isValid, string errorMessage) ValidateCountryData(string description, string isoA2, string isoA3)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                return (false, "Country name cannot be empty.");
            }
            if (string.IsNullOrWhiteSpace(isoA2) || isoA2.Length != 2)
            {
                return (false, "Country code must be a 2-letter code.");
            }
            if (string.IsNullOrWhiteSpace(isoA3) || isoA3.Length != 3)
            {
                return (false, "Country code must be a 3-letter code.");
            }
            return (true, string.Empty);
        }

        public (bool isValid, string message) InsertNewCountry(string description, string isoA2, string isoA3)
        {
            var (isValid, errorMessage) = ValidateCountryData(description, isoA2, isoA3);
            if (!isValid)
            {
                return (false, errorMessage);
            }
            try
            {
                var newCountry = new Domain.Entities.Country
                {
                    Description = description,
                    IsoA2 = isoA2,
                    IsoA3 = isoA3 + "X", // Placeholder for 3-letter code
                    IsoNum = 0, // Placeholder for ISO numeric code
                    IsEnable = true,
                    CreatedDate = DateTime.UtcNow
                };
                CountryRepository.AddAsync(newCountry);
                return (true, "Country inserted successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"Error inserting country: {ex.Message}");
            }
        }

        public (bool isValid, string) InsertNewCountry(string v)
        {
            throw new NotImplementedException();
        }
    }
}