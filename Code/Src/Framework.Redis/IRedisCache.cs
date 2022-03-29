using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.Redis
{
    public interface IRedisCache
    {
        T Get<T>(string key);
        Task<T> GetAsync<T>(string key);
        List<T> GetValues<T>(string key);
        Task<List<T>> GetValuesAsync<T>(string key);
        bool TryGetValue<T>(string key, out T result);
        void Set(string key, object data, int expirationTimeInMinutes);
        Task SetAsync(string key, object data, int expirationTimeInMinutes);
        void Set(string key, IEnumerable<object> data, int expirationTimeInMinutes);
        Task SetAsync(string key, IEnumerable<object> data, int expirationTimeInMinutes);
        void Remove(string key);
        Task RemoveAsync(string key);

    }
}
