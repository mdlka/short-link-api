using Microsoft.EntityFrameworkCore;
using ShortLink.Core.Models;
using ShortLink.Core.Repositories;
using ShortLink.Infrastructure.Persistence;

namespace ShortLink.Infrastructure.Repositories
{
    internal class ShortUrlsRepository(ApplicationDbContext dbContext) : IShortUrlsRepository
    {
        public async Task<ShortUrl?> GetAsync(string shortCode)
        {
            return await dbContext.ShortUrls.FirstOrDefaultAsync(u => u.ShortCode == shortCode);
        }

        public async Task AddAsync(ShortUrl shortUrl)
        {
            await dbContext.ShortUrls.AddAsync(shortUrl);
            await dbContext.SaveChangesAsync();
        }
    }
}