using ShortLink.Core.Extensions;
using ShortLink.Core.Services;
using ShortLink.WebAPI.Extensions;

namespace ShortLink.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddCoreServices();
            builder.Services.AddWebApiServices();
            
            var app = builder.Build();
            
            app.ConfigureSwagger();
            
            app.UseHttpsRedirection();
            
            app.MapPost("/shorten/{url}", (string url, IShortLinkService shortLinkService) 
                => shortLinkService.CreateShortUrl(url));
            
            app.MapGet("/{code}", (string code, IShortLinkService shortLinkService) 
                => shortLinkService.GetOriginalUrl(code));
            
            app.Run();
        }
    }
}