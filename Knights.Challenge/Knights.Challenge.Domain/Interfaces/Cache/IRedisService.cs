using System.Threading.Tasks;

namespace Knights.Challenge.Domain.Interfaces.Cache
{
    public interface IRedisService
    {
        Task<string> GetValue(string key);
        Task SetValue(string key, string value);
        Task RemoveKey(string key);
    }
}
