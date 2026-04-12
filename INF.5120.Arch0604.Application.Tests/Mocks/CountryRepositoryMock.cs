using INF._5120.Arch0604.Application.Interfaces;
using INF._5120.Arch0604.Domain.Entities;

namespace INF._5120.Arch0604.Application.Tests.Mocks
{
    public class CountryRepositoryMock : ICountryRepository
    {
        private readonly List<Country> Countries;

        public CountryRepositoryMock()
        {
            Countries = new List<Country>();
            InitializeMockRepository();
        }

        private void InitializeMockRepository()
        {
            for (int i = 1; i <= 5; i++)
            {
                Countries.Add(new Country
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

        public Task AddAsync(Country country)
        {
            Countries.Add(country);
            return Task.CompletedTask;
        }

        public void AddCountry(Country country)
        {
            Countries.Add(country);
        }

        public void Delete(Country country)
        {
            var existingCountry = Countries.FirstOrDefault(c => c.Id == country.Id);
            if (existingCountry != null)
            {
                Countries.Remove(existingCountry);
            }
        }

        public Task<bool> ExistsDuplicateAsync(
            string description,
            int isoNum,
            string isoA2,
            string isoA3,
            int? excludeId)
        {
            var exists = Countries.Any(c =>
                (!excludeId.HasValue || c.Id != excludeId.Value) &&
                (
                    c.Description == description ||
                    c.IsoNum == isoNum ||
                    c.IsoA2 == isoA2 ||
                    c.IsoA3 == isoA3
                ));

            return Task.FromResult(exists);
        }

        public Task<IEnumerable<Country>> GetAllAsync()
        {
            return Task.FromResult(Countries.AsEnumerable());
        }

        public Task<Country?> GetByIdAsync(int id)
        {
            var country = Countries.FirstOrDefault(c => c.Id == id);
            return Task.FromResult(country);
        }

        public Country? GetCountryById(int id)
        {
            return Countries.FirstOrDefault(c => c.Id == id);
        }

        public Country? GetCountryByIsoA2(string isoA2)
        {
            return Countries.FirstOrDefault(c => c.IsoA2 == isoA2);
        }

        public Task<int> SaveChangesAsync()
        {
            return Task.FromResult(1);
        }

        public void Update(Country country)
        {
            var existingCountry = Countries.FirstOrDefault(c => c.Id == country.Id);
            if (existingCountry != null)
            {
                Countries.Remove(existingCountry);
                Countries.Add(country);
            }
        }
    }
}