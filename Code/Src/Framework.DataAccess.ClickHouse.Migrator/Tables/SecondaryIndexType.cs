namespace Framework.DataAccess.ClickHouse.Migrator.Tables;

public enum SecondaryIndexType
{
    MinMax,
    Set,
    BloomFilter,
    TokenBfV1,
    NgramBfV1
}