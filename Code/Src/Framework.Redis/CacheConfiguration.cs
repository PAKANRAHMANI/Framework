namespace Framework.Redis;

public class CacheConfiguration
{
    public CacheConfiguration()
    {
        RedisCacheConfiguration = new RedisCacheConfiguration();
        RedisHashsetCacheConfiguration = new RedisHashsetCacheConfiguration();
    }
    public RedisCacheConfiguration RedisCacheConfiguration { get; set; }
    public RedisHashsetCacheConfiguration RedisHashsetCacheConfiguration { get; set; }
}
