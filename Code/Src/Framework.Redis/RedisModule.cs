using Framework.Config;
using System;

namespace Framework.Redis;

public class RedisModule : IFrameworkModule
{
	private readonly CacheConfiguration _config;

	public RedisModule(Action<CacheConfiguration> configuration)
	{
		_config = new CacheConfiguration();

		configuration.Invoke(_config);
	}

	public void Register(IDependencyRegister dependencyRegister)
	{
		dependencyRegister.RegisterSingleton(typeof(CacheConfiguration), _config);

		dependencyRegister.RegisterSingleton<IRedisDataBaseResolver, RedisDataBaseResolver>();

		dependencyRegister.RegisterSingleton<IRedisCache, RedisCache>();

		dependencyRegister.RegisterSingleton<IRedisHashsetCache, RedisHashsetCache>();
	}
}
