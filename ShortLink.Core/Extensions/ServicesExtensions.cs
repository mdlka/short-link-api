using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShortLink.Core.Persistence;
using ShortLink.Core.Services;

namespace ShortLink.Core.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddCoreServices(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("short_link"));
            services.AddTransient<IShortLinkService, ShortLinkService>();
        }
    }
}