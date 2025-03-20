using ShortLink.Infrastructure.Extensions;

namespace ShortLink.WebAPI.Extensions
{
    public static class AppExtensions
    {
        public static void ConfigureSwagger(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "ShortLink API V1");
            });
        }
        
        public static async Task ApplyMigrations(this IApplicationBuilder app)
        {
            await app.ApplicationServices.ApplyMigrations();
        }
    }
}