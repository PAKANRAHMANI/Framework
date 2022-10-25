namespace Framework.Redis;

public class CacheConfiguration
{
    public RedisCacheConfiguration RedisCacheConfiguration { get; set; }
    public RedisHashsetCacheConfiguration RedisHashsetCacheConfiguration { get; set; }
}