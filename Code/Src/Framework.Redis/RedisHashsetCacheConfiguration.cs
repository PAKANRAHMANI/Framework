namespace Framework.Redis;

public class RedisHashsetCacheConfiguration
{
    public string Connection { get; set; }
    public string InstanceName { get; set; }
    public int DbNumber { get; set; }
    /// <summary>
    /// set true for adding instance Name before key
    /// </summary>
    public bool UseFromInstanceNameInKey { get; set; }
}