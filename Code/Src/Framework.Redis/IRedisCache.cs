using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.Redis
{
    public interface IRedisCache
    {
        void Set(string key, object data, int expirationTimeInMinutes);
        Task SetAsync(string key, object data, int expirationTimeInMinutes);
        T Get<T>(string key);
        Task<T> GetAsync<T>(string key);
        IEnumerable<T> GetValues<T>(string key);
        Task<IEnumerable<T>> GetValuesAsync<T>(string key);
        bool TryGetValue<T>(string key, out T result);
        void Remove(string key);
        Task RemoveAsync(string key);
    }
}