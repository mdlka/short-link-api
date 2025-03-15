using ShortLink.Core.Extensions;
using ShortLink.Core.Services;
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
            builder.Services.AddInfrastructureServices();
            builder.Services.AddWebApiServices();
            
            var app = builder.Build();
            
            app.ConfigureSwagger();
            
            app.UseHttpsRedirection();
            
            app.MapPost("/shorten/{url}", async (string url, IShortLinkService shortLinkService) 
                => await shortLinkService.CreateShortUrl(url));
            
            app.MapGet("/{code}", async (string code, IShortLinkService shortLinkService) 
                => await shortLinkService.GetOriginalUrl(code));
            
            app.Run();
        }
    }
}