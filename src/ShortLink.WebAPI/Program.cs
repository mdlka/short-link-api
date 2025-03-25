using ShortLink.Core.Extensions;
using ShortLink.Infrastructure.Extensions;
using ShortLink.WebAPI.Extensions;

namespace ShortLink.WebAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddCoreServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddWebApiServices();
            
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
                app.ConfigureSwagger();
            
            await app.ApplyMigrations();

            app.UseHttpsRedirection();
            app.MapControllers();
            
            await app.RunAsync();
        }
    }
}