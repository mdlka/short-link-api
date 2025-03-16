using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using ShortLink.Core.Models;
using ShortLink.Core.Repositories;
using ShortLink.Infrastructure.Persistence;

namespace ShortLink.Infrastructure.Repositories
{
    internal class ShortUrlsRepository(ApplicationDbContext dbContext, IDistributedCache cache) : IShortUrlsRepository
    {
        private readonly TimeSpan _cacheAbsoluteExpiration = TimeSpan.FromMinutes(10);
        private readonly TimeSpan _cacheSlidingExpiration = TimeSpan.FromMinutes(2);
        
        public async Task<ShortUrl?> GetAsync(string shortCode)
        {
            string? cacheShortUrl = await cache.GetStringAsync(shortCode);

            if (cacheShortUrl != null)
                return JsonConvert.DeserializeObject<ShortUrl>(cacheShortUrl);
            
            var shortUrl = await dbContext.ShortUrls.FirstOrDefaultAsync(u => u.ShortCode == shortCode);

            if (shortUrl == null)
                return shortUrl;

            await cache.SetStringAsync(shortCode, JsonConvert.SerializeObject(shortUrl),
                new DistributedCacheEntryOptions
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