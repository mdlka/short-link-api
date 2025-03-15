namespace ShortLink.WebAPI.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddWebApiServices(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen();
        }
    }
}