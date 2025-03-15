using Microsoft.EntityFrameworkCore;
using ShortLink.Core.Exceptions;
using ShortLink.Core.Models;
using ShortLink.Core.Persistence;

namespace ShortLink.Core.Services
{
    internal class ShortLinkService(ApplicationDbContext dbContext) : IShortLinkService
    {
        public async Task<string> GetOriginalUrl(string code)
        {
            var shortUrl = await dbContext.ShortUrls.FirstOrDefaultAsync(u => u.ShortCode == code) ??
                    throw new NotFoundException();

            return shortUrl.OriginalUrl;
        }

        public async Task<ShortUrl> CreateShortUrl(string url)
        {
            var shortUrl = new ShortUrl
            {
                Id = Guid.NewGuid(),
                OriginalUrl = url,
                ShortCode = Guid.NewGuid().ToString("N")[..CoreConstants.ShortCodeLength]
            };

            await dbContext.ShortUrls.AddAsync(shortUrl);
            await dbContext.SaveChangesAsync();
            
            return shortUrl;
        }
    }
}