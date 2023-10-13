namespace Framework.Caching.Extensions.Configuration;

public class InMemoryCacheConfiguration
{
    public long SizeLimit { get; set; }
    public CachePriority CachePriority { get; set; }
}