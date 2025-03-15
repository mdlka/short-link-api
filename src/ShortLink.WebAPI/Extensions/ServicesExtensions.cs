using ShortLink.WebAPI.Filters;

namespace ShortLink.WebAPI.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddWebApiServices(this IServiceCollection services)
        {
            services.AddControllers(options => options.Filters.Add<CoreExceptionFilterAttribute>());
            services.AddSwaggerGen();
        }
    }
}