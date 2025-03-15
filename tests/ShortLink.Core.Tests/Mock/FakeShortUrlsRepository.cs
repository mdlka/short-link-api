using ShortLink.Core.Models;
using ShortLink.Core.Repositories;

namespace ShortLink.Core.Tests.Mock
{
    public class FakeShortUrlsRepository : IShortUrlsRepository
    {
        private readonly Dictionary<string, ShortUrl> _shortUrls = new();
        
        public Task<ShortUrl?> GetAsync(string shortCode)
        {
            return Task.FromResult(_shortUrls.GetValueOrDefault(shortCode));
        }

        public Task AddAsync(ShortUrl shortUrl)
        {
            _shortUrls.Add(shortUrl.ShortCode, shortUrl);
            return Task.CompletedTask;
        }
    }
}