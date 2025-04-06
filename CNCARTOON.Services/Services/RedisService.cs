using CNCARTOON.Services.IServices;
using StackExchange.Redis;

namespace CNCARTOON.Services.Services
{
    public class RedisService : IRedisService
    {
        private readonly IConnectionMultiplexer _redis;
        public RedisService(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }
        public async Task<bool> DeleteStringAsync(string key)
        {
            var cache = _redis.GetDatabase();
            var result = await cache.KeyDeleteAsync(key);
            return result;
        }

        public async Task<string> RetrieveStringAsync(string key)
        {
            var cache = _redis.GetDatabase();
            var result = await cache.StringGetAsync(key);
            return result;
        }

        public async Task<bool> StoreStringAsync(string key, string value, TimeSpan? expiry)
        {
            var cache = _redis.GetDatabase();
            var result = await cache.StringSetAsync(key, value, expiry);
            return result;  
        }
    }
}
