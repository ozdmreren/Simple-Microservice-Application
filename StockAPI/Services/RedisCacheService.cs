using StackExchange.Redis;
using StockAPI.Interfaces;

namespace StockAPI.Services 
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDatabase _cache;
        private readonly IConnectionMultiplexer _redisConnection;

        public RedisCacheService(IConnectionMultiplexer redisConnection)
        {
            _redisConnection = redisConnection;
            _cache = _redisConnection.GetDatabase();
        }

        public async Task Clear(string key)
        {
            await _cache.KeyDeleteAsync(key);
        }

        public async Task<string> GetProductAsync(string key)
        {
            return await _cache.StringGetAsync(key);
        }

        public async Task<bool> SetProductAsync(string key, string value, int time = 1)
        {
            return await _cache.StringSetAsync(key, value, TimeSpan.FromHours(time));
        }
    }
}