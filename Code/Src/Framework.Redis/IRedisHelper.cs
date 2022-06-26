using StackExchange.Redis;

namespace Framework.Redis
{
    public interface IRedisHelper
    {
        IDatabase GetDatabase(string connection, int dbNumber = 0);
    }
}