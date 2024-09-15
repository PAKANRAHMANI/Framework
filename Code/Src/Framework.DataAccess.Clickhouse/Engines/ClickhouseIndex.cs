namespace Framework.DataAccess.ClickHouse.Engines;

public enum ClickhouseIndex
{
    minmax,
    set,
    bloom_filter,
    tokenbf_v1,
    ngrambf_v1
}