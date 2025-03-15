using ShortLink.Core.Extensions;
using ShortLink.Infrastructure.Extensions;
using ShortLink.WebAPI.Extensions;

namespace ShortLink.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddCoreServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddWebApiServices();
            
            var app = builder.Build();
            
            app.ConfigureSwagger();
            
            app.UseHttpsRedirection();
            app.MapControllers();
            
            app.Run();
        }
    }
}