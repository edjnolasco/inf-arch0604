using INF._5120.Arch0604.Application.Interfaces;
using INF._5120.Arch0604.Domain.Entities;


namespace INF._5120.Arch0604.Application.Tests.Mocks
{
    public class CountryRepositoryMock : ICountryRepository
    {
        private readonly List<Country> Countries;
        public CountryRepositoryMock()
        {
            Countries = [];

            InitializeMockRepository();
        }

        private void InitializeMockRepository()
        {
            for (int i = 1; i <= 5; i++)
            {
                Countries.Add(new()
                {
                    Id = i,
                    Description = $"Country{i}",
                    IsoNum = 100 + i,
                    IsoA2 = $"C{i}",
                    IsoA3 = $"COU{i}",
                    IsEnable = true,
                    CreatedDate = DateTime.UtcNow
                });
            }
        }

        public void AddCountry(Country country)
        {
            Countries.Add(country);
        }

        public Country? GetCountryById(int id)
        {
            return Countries.FirstOrDefault(c => c.Id == id);
        }

        public void Update(Country country)
        {
            var existingCountry = GetCountryById(country.Id);
            if (existingCountry != null)
            {
                Countries.Remove(existingCountry);
                Countries.Add(country);
            }
        }

        public Country? GetCountryByIsoA2(string isoA2)
        {
            return Countries.FirstOrDefault(c => c.IsoA2 == isoA2);
        }

    }
}