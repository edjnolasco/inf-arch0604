using INF._5120.Arch0604.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace INF._5120.Arch0604.Domain.Tests.Helpers
{
    public static class TestHelper
    {
        public static INF5120DbContext GenerateContext()
        {
            var optionbuilder = new DbContextOptionsBuilder<INF5120DbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());

            return new INF5120DbContext(optionbuilder.Options);
        }
    }
}