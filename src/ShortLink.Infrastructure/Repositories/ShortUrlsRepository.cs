using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using ShortLink.Core.Models;
using ShortLink.Core.Repositories;
using ShortLink.Infrastructure.Extensions;
using ShortLink.Infrastructure.Persistence;

namespace ShortLink.Infrastructure.Repositories
{
    internal class ShortUrlsRepository(ApplicationDbContext dbContext, IDistributedCache cache) : IShortUrlsRepository
    {
        private readonly TimeSpan _cacheAbsoluteExpiration = TimeSpan.FromMinutes(60);
        private readonly TimeSpan _cacheSlidingExpiration = TimeSpan.FromMinutes(15);
        
        public async Task<ShortUrl?> GetAsync(string shortCode)
        {
            (bool found, var cacheShortUrl) = await cache.TryGetAsync<ShortUrl>(shortCode);

            if (found)
                return cacheShortUrl;
            
            var shortUrl = await dbContext.ShortUrls.FirstOrDefaultAsync(u => u.ShortCode == shortCode);

            if (shortUrl == null)
                return shortUrl;
            
            await cache.SetAsync(shortCode, shortUrl, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _cacheAbsoluteExpiration, 
                    SlidingExpiration = _cacheSlidingExpiration
                });
            
            return shortUrl;
        }

        public async Task AddAsync(ShortUrl shortUrl)
        {
            await dbContext.ShortUrls.AddAsync(shortUrl);
            await dbContext.SaveChangesAsync();
        }
    }
}