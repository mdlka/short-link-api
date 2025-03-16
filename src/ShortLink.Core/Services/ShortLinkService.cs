using System.Security.Cryptography;
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
                ShortCode = GenerateShortCode(CoreConstants.ShortCodeLength)
            };

            await shortUrlsRepository.AddAsync(shortUrl);

            return shortUrl;
        }

        private static string GenerateShortCode(int length)
        {
            byte[] bytes = new byte[length];
    
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
    
            return string.Create(length, CoreConstants.ShortCodeCharacters, (span, chars) => 
            {
                for (int i = 0; i < span.Length; i++)
                    span[i] = chars[bytes[i] % chars.Length];
            });
        }
    }
}