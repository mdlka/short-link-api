using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShortLink.Infrastructure.Persistence;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;

namespace ShortLink.WebAPI.Tests
{
    public class ShortLinkWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:17.4")
            .WithDatabase("short_link_tests")
            .WithUsername("postgres")
            .WithPassword("mdlka")
            .Build();

        private readonly RedisContainer _cacheContainer = new RedisBuilder()
            .WithImage("redis:7.4.2")
            .Build();
        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbDescriptor = services.SingleOrDefault(s =>
                    s.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (dbDescriptor != null)
                    services.Remove(dbDescriptor);
                
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseNpgsql(_dbContainer.GetConnectionString());
                });

                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = _cacheContainer.GetConnectionString();
                });
            });
        }

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();
            await _cacheContainer.StartAsync();
        }

        public new async Task DisposeAsync()
        {
            await _dbContainer.StopAsync();
            await _cacheContainer.StopAsync();
        }
    }
}