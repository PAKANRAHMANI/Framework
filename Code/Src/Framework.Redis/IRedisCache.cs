using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.Redis
{
    public interface IRedisCache
    {
        T GetString<T>(string key);
        Task<T> GetStringAsync<T>(string key);
        void SetString(string key, object data, int expirationTimeInMinutes);
        Task SetStringAsync(string key, object data, int expirationTimeInMinutes);
        void Remove(string key);
        Task RemoveAsync(string key);
        bool TryGetValue<T>(string key, out T result);
        Task Set(string key, IEnumerable<object> data, int expirationTimeInMinutes);
        Task<List<T>> GetAsync<T>(string key);
        List<T> Get<T>(string key);
    }
}
