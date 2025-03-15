using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShortLink.Core.Repositories;
using ShortLink.Infrastructure.Persistence;
using ShortLink.Infrastructure.Repositories;

namespace ShortLink.Infrastructure.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("short_link"));

            services.AddTransient<IShortUrlsRepository, ShortUrlsRepository>();
        }
    }
}