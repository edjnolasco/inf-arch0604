using INF._5120.Arch0604.Domain.Entities;

namespace INF._5120.Arch0604.Application.Interfaces
{
    public interface ICountryRepository
    {
        Task<IEnumerable<Country>> GetAllAsync();
        Task<Country?> GetByIdAsync(int id);
        Task<bool> ExistsDuplicateAsync(
            string description,
            int isoNum,
            string isoA2,
            string isoA3,
            int? excludeId = null);
        Task AddAsync(Country country);
        void Delete(Country country);
        Task<int> SaveChangesAsync();
    }
}