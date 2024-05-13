using Microsoft.Extensions.Caching.Distributed;

namespace Knights.Challenge.Domain.Interfaces.Cache
{
    public interface IDistributedCacheEntryOptionsFactory
    {
        DistributedCacheEntryOptions Create();
    }
}
