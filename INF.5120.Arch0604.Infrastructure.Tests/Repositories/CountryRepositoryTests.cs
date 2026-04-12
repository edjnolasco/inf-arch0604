using INF._5120.Arch0604.Domain.Entities;
using INF._5120.Arch0604.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace INF._5120.Arch0604.Infrastructure.Tests.Repositories
{
    public class CountryRepositoryTests
    {
        [Fact]

        public void AddCountryTest()
        {
            var options = new DbContextOptionsBuilder<INF5120DbContext>()
                .UseInMemoryDatabase(databaseName: "INF5120")
                .Options;


            using var context = new INF5120DbContext(options);
            context.Countries.Add(new Country
            {
                Id = 1,
                Description = "TestCountry",
                IsoNum = 999,
                IsoA2 = "TC",
                IsoA3 = "TST",
                IsEnable = true,
                CreatedDate = DateTime.UtcNow
            });

            context.Countries.Add(new Country
            {
                Id = 2,
                Description = "TestCountry",
                IsoNum = 444,
                IsoA2 = "TS",
                IsoA3 = "TCT",
                IsEnable = true,
                CreatedDate = DateTime.UtcNow
            });
            context.SaveChanges();


        }


    }
}