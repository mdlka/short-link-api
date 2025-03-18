using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Distributed;

namespace ShortLink.Infrastructure.Extensions
{
    public static class DistributedCacheExtensions
    {
        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNamingPolicy = null,
            WriteIndented = true,
            AllowTrailingCommas = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public static Task SetAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value, SerializerOptions));
            return cache.SetAsync(key, bytes, options);
        }
        
        public static async Task<(bool found, T? value)> TryGetAsync<T>(this IDistributedCache cache, string key)
        {
            byte[]? bytes = await cache.GetAsync(key);

            return bytes == null 
                ? (false, default) 
                : (true, JsonSerializer.Deserialize<T>(bytes, SerializerOptions));
        }
    }
}