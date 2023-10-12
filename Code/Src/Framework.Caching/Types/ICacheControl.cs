namespace Framework.Caching.Types
{
    public interface ICacheControl
    {
        void Set<T>(string key, T value, int expirationTimeInMinutes) where T : class;
        T Get<T>(string key) where T : class;
    }
}
