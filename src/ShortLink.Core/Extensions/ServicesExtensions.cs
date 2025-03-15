using Microsoft.Extensions.DependencyInjection;
using ShortLink.Core.Services;

namespace ShortLink.Core.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddCoreServices(this IServiceCollection services)
        {
            services.AddTransient<IShortLinkService, ShortLinkService>();
        }
    }
}