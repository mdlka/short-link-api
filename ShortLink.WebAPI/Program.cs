using ShortLink.Core.Extensions;
using ShortLink.Core.Services;

namespace ShortLink.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddCoreServices();
            
            var app = builder.Build();

            app.UseHttpsRedirection();
            
            app.MapPost("/", (string url, IShortLinkService shortLinkService) 
                => shortLinkService.CreateShortUrl(url));
            
            app.MapGet("/{code}", (string code, IShortLinkService shortLinkService) 
                => shortLinkService.GetOriginalUrl(code));
            
            app.Run();
        }
    }
}