using Framework.Caching.Extensions.Configuration;
using Microsoft.Extensions.Caching.Memory;

namespace Framework.Caching.Extensions.DotnetCore;

public class InMemoryCacheProvider : IInMemoryCacheProvider
{
	private readonly InMemoryCacheConfiguration _inMemoryCacheConfiguration;

	public InMemoryCacheProvider(InMemoryCacheConfiguration inMemoryCacheConfiguration)
	{
		_inMemoryCacheConfiguration = inMemoryCacheConfiguration;
	}
	public IMemoryCache GetMemoryCache()
	{
		return new MemoryCache(new MemoryCacheOptions { SizeLimit = _inMemoryCacheConfiguration.SizeLimit,CompactionPercentage = _inMemoryCacheConfiguration.CompactionPercentage});
	}
}
