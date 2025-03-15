using Microsoft.EntityFrameworkCore;
using ShortLink.Core.Exceptions;
using ShortLink.Core.Models;
using ShortLink.Core.Persistence;
using ShortLink.Core.Services;

namespace ShortLink.Core.Tests.Services
{
    public class ShortLinkServiceTests
    {
        [Fact]
        public async Task CreateShortUrl_ShouldGenerateCorrectLengthCode()
        {
            // Arrange
            var shortLinkService = new ShortLinkService(CreateDbContext());

            // Act
            var result = await shortLinkService.CreateShortUrl("https://test.com");
            
            // Assert
            Assert.Equal(CoreConstants.ShortCodeLength, result.ShortCode.Length);
        }

        [Fact]
        public async Task CreateShortUrl_ShouldGenerateDifferentCodesForDifferentUrl()
        {
            // Arrange
            var shortLinkService = new ShortLinkService(CreateDbContext());
            
            // Act
            var firstUrl = await shortLinkService.CreateShortUrl("https://test1.com");
            var secondUrl = await shortLinkService.CreateShortUrl("https://test2.com");
            
            // Assert
            Assert.NotEqual(firstUrl.ShortCode, secondUrl.ShortCode);
        }
        
        [Fact]
        public async Task CreateShortUrl_ShouldGenerateDifferentCodesForSameUrl()
        {
            // Arrange
            var shortLinkService = new ShortLinkService(CreateDbContext());
            
            // Act
            var firstUrl = await shortLinkService.CreateShortUrl("https://test.com");
            var secondUrl = await shortLinkService.CreateShortUrl("https://test.com");
            
            // Assert
            Assert.NotEqual(firstUrl.ShortCode, secondUrl.ShortCode);
        }
        
        [Fact]
        public async Task CreateShortUrl_ShouldGenerateAlphanumericCode()
        {
            // Arrange
            var shortLinkService = new ShortLinkService(CreateDbContext());
    
            // Act
            var result = await shortLinkService.CreateShortUrl("https://test.com");
    
            // Assert
            Assert.Matches($"^[a-zA-Z0-9]{{{CoreConstants.ShortCodeLength}}}$", result.ShortCode);
        }

        [Fact]
        public async Task GetOriginalUrl_ShouldThrowNotFoundExceptionIfCodeNotExist()
        {
            // Arrange
            var shortLinkService = new ShortLinkService(CreateDbContext());

            // Act && Assert
            await Assert.ThrowsAsync<NotFoundException>(() => shortLinkService.GetOriginalUrl("not_exist"));
        }

        [Theory]
        [InlineData("https://test.com")]
        [InlineData("https://test.com/first")]
        [InlineData("https://test.com/first/second")]
        [InlineData("https://test.com/first/second?third=1")]
        [InlineData("https://test.com/first/second?one=1&two=2")]
        public async Task GetOriginalUrl_ShouldReturnCorrectUrl(string url)
        {
            // Arrange
            var shortLinkService = new ShortLinkService(CreateDbContext());
            var shortUrl = await shortLinkService.CreateShortUrl(url);
            
            // Act
            string result = await shortLinkService.GetOriginalUrl(shortUrl.ShortCode);
            
            // Assert
            Assert.Equal(url, result);
        }

        private static ApplicationDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}