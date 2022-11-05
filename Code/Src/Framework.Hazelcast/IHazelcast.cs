namespace Framework.Hazelcast
{
    public interface IHazelcast
    {
        Task MapSetAsync<TKey, TValue>(TKey key, TValue value, string mapName);
        Task<TValue> MapGetAsync<TKey, TValue>(TKey key, string mapName);
    }
}