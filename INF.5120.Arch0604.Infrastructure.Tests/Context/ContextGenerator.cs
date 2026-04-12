using INF._5120.Arch0604.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace INF._5120.Arch0604.Infrastructure.Tests.Context
{
    public static class ContextGenerator
    {
        public static INF5120DbContext Generate()
        {
            var optionsBuilder = new DbContextOptionsBuilder<INF5120DbContext>()
                .UseInMemoryDatabase(databaseName: "INF5120");
            return new INF5120DbContext(optionsBuilder.Options);
        }
    }
}
