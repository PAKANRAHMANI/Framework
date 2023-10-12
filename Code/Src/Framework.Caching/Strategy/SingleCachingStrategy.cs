using Framework.Caching.Types;

namespace Framework.Caching.Strategy
{
    public class SingleCachingStrategy : ICache
    {
        private readonly ICacheControl _caching;

        public SingleCachingStrategy(ICacheControl caching)
        {
            _caching = caching;
        }

        public T Get<T>(string key, int? expirationTimeInMinutes = null, Func<T> query = null) where T : class
        {
            return _caching.Get<T>(key);
        }

        public void Set<T>(string key, T value, int expirationTimeInMinutes) where T : class
        {
            _caching.Set(key, value, expirationTimeInMinutes);
        }
    }
}