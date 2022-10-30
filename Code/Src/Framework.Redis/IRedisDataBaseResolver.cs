using StackExchange.Redis;

namespace Framework.Redis;

public interface IRedisDataBaseResolver
{
	IDatabase GetDatabase(string connection, int dbNumber = 0);
}
