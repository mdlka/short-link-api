using ShortLink.Core.Models;

namespace ShortLink.Core.Repositories
{
    public interface IShortUrlsRepository
    {
        Task<ShortUrl?> GetAsync(string shortCode);
        Task AddAsync(ShortUrl shortUrl);
    }
}