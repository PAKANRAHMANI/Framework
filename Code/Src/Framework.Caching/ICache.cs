namespace Framework.Caching;

public interface ICache
{
    T Get<T>(string key, Func<T> query, int expirationTimeInSecond) where T : class;
    void Set<T>(string key, T value, int expirationTimeInSecond) where T : class;
}
