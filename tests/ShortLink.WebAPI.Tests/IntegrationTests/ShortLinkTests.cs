using System.Net;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using ShortLink.Core.Models;
using ShortLink.Infrastructure.Extensions;
using ShortLink.Infrastructure.Persistence;

namespace ShortLink.WebAPI.Tests.IntegrationTests
{
    public class ShortLinkTests : IClassFixture<ShortLinkWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _dbContext;
        private readonly IDistributedCache _cache;

        public ShortLinkTests(ShortLinkWebApplicationFactory factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            
            var serviceScope = factory.Services.CreateScope();
            _dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _cache = serviceScope.ServiceProvider.GetRequiredService<IDistributedCache>();
        }
        
        [Theory]
        [InlineData("invalid_url")]
        [InlineData("test.com")]
        public async Task PostShorten_ShouldReturnBadRequest_WhenUrlIsInvalid(string url)
        {
            // Arrange
            var content = new StringContent($"\"{url}\"", Encoding.UTF8, "application/json"); 
            
            // Act
            var response = await _client.PostAsync("shorten/", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PostShorten_ShouldAddShortUrl_WhenUrlIsValid()
        {
            // Arrange
            var content = new StringContent("\"https://test.com/\"", Encoding.UTF8, "application/json"); 
            
            // Act
            var response = await _client.PostAsync("shorten/", content);
            var responseShortUrl = await response.Content.ReadFromJsonAsync<ShortUrl>();

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(responseShortUrl);
            
            var shortUrl = _dbContext.ShortUrls.FirstOrDefault(u => u.ShortCode == responseShortUrl.ShortCode);

            Assert.NotNull(shortUrl);
            Assert.Equal("https://test.com/", shortUrl.OriginalUrl);
        }
        
        [Fact]
        public async Task GetRedirect_ShouldReturnNotFound_WhenCodeInvalid()
        {
            // Act
            var response = await _client.GetAsync("/invalid_code");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        
        [Fact]
        public async Task GetRedirect_ShouldReturnRedirect_WhenCodeValid()
        {
            // Arrange
            var content = new StringContent("\"https://test.com/\"", Encoding.UTF8, "application/json"); 
            var postResponse = await _client.PostAsync("shorten/", content);
            var shortUrl = await postResponse.Content.ReadFromJsonAsync<ShortUrl>() ?? throw new InvalidOperationException();    
            
            // Act
            var response = await _client.GetAsync($"/{shortUrl.ShortCode}");

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal(new Uri("https://test.com/"), response.Headers.Location);
        }
        
        [Fact]
        public async Task GetRedirect_ShouldAddUrlToCache_WhenCodeValid()
        {
            // Arrange
            var content = new StringContent("\"https://test.com/\"", Encoding.UTF8, "application/json"); 
            var postResponse = await _client.PostAsync("shorten/", content);
            var shortUrl = await postResponse.Content.ReadFromJsonAsync<ShortUrl>() ?? throw new InvalidOperationException();    
            
            // Act && Assert
            (bool found, _) = await _cache.TryGetAsync<ShortUrl>(shortUrl.ShortCode);
            
            Assert.False(found);
            
            await _client.GetAsync($"/{shortUrl.ShortCode}");
            (found, var cacheShortUrl) = await _cache.TryGetAsync<ShortUrl>(shortUrl.ShortCode);

            Assert.True(found);
            Assert.Equal("https://test.com/", cacheShortUrl?.OriginalUrl);
        }
    }
}