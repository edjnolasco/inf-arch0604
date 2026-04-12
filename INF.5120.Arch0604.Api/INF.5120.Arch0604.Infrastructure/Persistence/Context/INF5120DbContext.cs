using INF._5120.Arch0604.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace INF._5120.Arch0604.Infrastructure.Persistence.Context
{
    public class INF5120DbContext(DbContextOptions<INF5120DbContext> options) : DbContext(options)
    {
        public DbSet<Country> Countries => Set<Country>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(INF5120DbContext).Assembly);
        }
    }
}
