using Microsoft.EntityFrameworkCore;
using ShortLink.Core.Persistence;

namespace ShortLink.Core.Tests.Mock
{
    public class MockDbContextFactory : IDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}