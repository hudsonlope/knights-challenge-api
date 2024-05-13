using Knights.Challenge.Domain.Interfaces.Cache;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System;

namespace Knights.Challenge.Domain.Configuration
{
    public class DistributedCacheEntryOptionsFactory : IDistributedCacheEntryOptionsFactory
    {
        private readonly IConfiguration _configuration;

        public DistributedCacheEntryOptionsFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DistributedCacheEntryOptions Create()
        {
            var redisSettings = _configuration.GetSection(Constants.RedisSettings).Get<RedisSettings>();
            return new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(redisSettings.AbsoluteExpirationMinutes),
                SlidingExpiration = TimeSpan.FromMinutes(redisSettings.SlidingExpirationMinutes)
            };
        }
    }
}
