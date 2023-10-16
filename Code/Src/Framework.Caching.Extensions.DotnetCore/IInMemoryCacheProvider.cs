using Microsoft.Extensions.Caching.Memory;

namespace Framework.Caching.Extensions.DotnetCore;
public interface IInMemoryCacheProvider
{
	IMemoryCache GetMemoryCache();
}
