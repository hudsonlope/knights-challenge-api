using Knights.Challenge.Domain.Interfaces.Cache;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;

namespace Knights.Challenge.Service.Services
{
    public class RedisService : IRedisService
    {
        private readonly IDistributedCache _redisConnection;
        private readonly DistributedCacheEntryOptions _options;

        public RedisService(IDistributedCache redisConnection, IDistributedCacheEntryOptionsFactory optionsFactory)
        {
            _redisConnection = redisConnection;
            _options = optionsFactory.Create();
        }

        public async Task<string> GetValue(string key)
        {
            return await _redisConnection.GetStringAsync(key);
        }

        public async Task SetValue(string key, string value)
        {
            await _redisConnection.SetStringAsync(key, value, _options);
        }

        public async Task RemoveKey(string key)
        {
            await _redisConnection.RemoveAsync(key);
        }
    }
}
