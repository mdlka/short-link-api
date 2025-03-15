using ShortLink.Core.Exceptions;
using ShortLink.Core.Models;
using ShortLink.Core.Repositories;

namespace ShortLink.Core.Services
{
    internal class ShortLinkService(IShortUrlsRepository shortUrlsRepository) : IShortLinkService
    {
        public async Task<string> GetOriginalUrl(string code)
        {
            var shortUrl = await shortUrlsRepository.GetAsync(code) ?? throw new NotFoundException();

            return shortUrl.OriginalUrl;
        }

        public async Task<ShortUrl> CreateShortUrl(string url)
        {
            var shortUrl = new ShortUrl
            {
                OriginalUrl = url,
                ShortCode = Guid.NewGuid().ToString("N")[..CoreConstants.ShortCodeLength]
            };

            await shortUrlsRepository.AddAsync(shortUrl);

            return shortUrl;
        }
    }
}