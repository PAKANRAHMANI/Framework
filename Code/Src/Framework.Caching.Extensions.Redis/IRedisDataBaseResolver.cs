using StackExchange.Redis;

namespace Framework.Caching.Extensions.Redis;

public interface IRedisDataBaseResolver
{
    IDatabase GetDatabase(string connection, int dbNumber = 0);
}