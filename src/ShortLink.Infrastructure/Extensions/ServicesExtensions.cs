using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShortLink.Core.Repositories;
using ShortLink.Infrastructure.Persistence;
using ShortLink.Infrastructure.Repositories;

namespace ShortLink.Infrastructure.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("ApplicationContext")));

            services.AddStackExchangeRedisCache(options =>
                options.Configuration = configuration.GetConnectionString("Redis"));

            services.AddTransient<IShortUrlsRepository, ShortUrlsRepository>();
        }
    }
}