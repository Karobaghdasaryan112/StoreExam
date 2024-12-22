using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SecureStore.Api.ApplicationLayer.Common.Extentions
{
    public static class DistributedCacheExtentions
    {
        private static JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = null,
            AllowTrailingCommas = true,
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        public static Task SetAsync<T>(this IDistributedCache distributedCache,string key,T value,DistributedCacheEntryOptions options)
        {
            var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value, JsonSerializerOptions));
            return distributedCache.SetAsync(key, bytes, options);
        }

        public static Task SetAsync<T>(this IDistributedCache distributedCache, string key, T value)
        {
            return SetAsync<T>(
                distributedCache,
                key,
                value,
                new DistributedCacheEntryOptions().
                SetSlidingExpiration(TimeSpan.FromMinutes(10)).
                SetAbsoluteExpiration(TimeSpan.FromHours(2)));
        }

        public static bool TryGetValue<T>(this IDistributedCache distributedCache, string key, out T? value)
        {
            var val = distributedCache.Get(key);
            value = default;
            if (val == null)
                return false;
            value = JsonSerializer.Deserialize<T>(val, JsonSerializerOptions);
            return true;
        }

        public static async Task<T?> GetOrSetAsync<T>(this IDistributedCache cache, string key, Func<Task<T>> task, DistributedCacheEntryOptions? options = null)
        {
            if (options == null)
            {
                options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(10))
                .SetAbsoluteExpiration(TimeSpan.FromHours(2));
            }

            if (cache.TryGetValue(key, out T? value) && value is not null)
            {
                return value;
            }

            value = await task();

            if (value is not null)
            {
                await cache.SetAsync<T>(key, value, options);
            }

            return value;
        }

    }

}
