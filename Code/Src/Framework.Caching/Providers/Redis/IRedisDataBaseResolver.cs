using StackExchange.Redis;

namespace Framework.Caching.Providers.Redis;

public interface IRedisDataBaseResolver
{
    IDatabase GetDatabase(string connection, int dbNumber = 0);
}