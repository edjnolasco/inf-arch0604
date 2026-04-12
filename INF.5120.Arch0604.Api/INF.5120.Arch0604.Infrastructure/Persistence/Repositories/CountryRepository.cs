using INF._5120.Arch0604.Application.Interfaces;
using INF._5120.Arch0604.Domain.Entities;
using INF._5120.Arch0604.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace INF._5120.Arch0604.Infrastructure.Persistence.Repositories
{
    public class CountryRepository(INF5120DbContext context) : ICountryRepository
    {
        public async Task<IEnumerable<Country>> GetAllAsync()
        {
            return await context.Countries
                .AsNoTracking()
                .OrderBy(c => c.Description)
                .ToListAsync();
        }

        public async Task<Country?> GetByIdAsync(int id)
        {
            return await context.Countries
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> ExistsDuplicateAsync(
            string description,
            int isoNum,
            string isoA2,
            string isoA3,
            int? excludeId = null)
        {
            description = description.Trim().ToUpper();
            isoA2 = isoA2.Trim().ToUpper();
            isoA3 = isoA3.Trim().ToUpper();

            return await context.Countries.AnyAsync(c =>
                (!excludeId.HasValue || c.Id != excludeId.Value) &&
                (
                    c.Description.ToUpper() == description ||
                    c.IsoNum == isoNum ||
                    c.IsoA2 == isoA2 ||
                    c.IsoA3 == isoA3
                ));
        }

        public async Task AddAsync(Country country)
        {
            await context.Countries.AddAsync(country);
        }

        public void Delete(Country country)
        {
            context.Countries.Remove(country);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}