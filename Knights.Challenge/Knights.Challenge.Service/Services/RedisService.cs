using Knights.Challenge.Domain.Configuration;
using Knights.Challenge.Domain.Interfaces.Cache;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Knights.Challenge.Service.Services
{
    public class RedisService : IRedisService
    {
        private readonly IDistributedCache _redisConnection;
        private readonly IConfiguration _configuration;
        private readonly DistributedCacheEntryOptions _options;

        public RedisService(IDistributedCache redisConnection, IConfiguration configuration)
        {
            _redisConnection = redisConnection;
            _configuration = configuration;

            var redisSettings = _configuration.GetSection(Constants.RedisSettings).Get<RedisSettings>();
            _options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(redisSettings.AbsoluteExpirationMinutes),
                SlidingExpiration = TimeSpan.FromMinutes(redisSettings.SlidingExpirationMinutes)
            };
        }

        public async Task<string> GetValue(string key)
        {
            return await _redisConnection.GetStringAsync(key);
        }

        public async Task SetValue(string key, string value)
        {
            await _redisConnection.SetStringAsync(key, value);
        }

        public async Task RemoveKey(string key)
        {
            await _redisConnection.RemoveAsync(key);
        }
    }
}
