using ShortLink.Core.Models;

namespace ShortLink.Core.Services
{
    public interface IShortLinkService
    {
        Task<string> GetOriginalUrl(string code);
        Task<ShortUrl> CreateShortUrl(string url);
    }
}