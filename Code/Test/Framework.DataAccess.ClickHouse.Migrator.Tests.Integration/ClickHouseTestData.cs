using Framework.DataAccess.ClickHouse.Migrator.MergeTreeEngine;
using Framework.DataAccess.ClickHouse.Migrator.Partitions;
using Framework.DataAccess.ClickHouse.Migrator.Tables;

namespace Framework.DataAccess.ClickHouse.Migrator.Tests.Integration;

public static class ClickHouseTestData
{
    public static string _10_104_142_30 = "10.104.142.30";
    public static int _30536 = 30536;
    public static string Admin = "admin";
    public static string Password = "wxV2ai63KNZ8SG03beuPMdn9";
    public static string RlcMessages = "ha_RlcMessages";
    public static int _5000 = 5000;
    public static int _6000 = 6000;
    public static string CipTraderCharismaTech9092 = "kafka-headless.cip.svc.trader-stage.charisma.tech:9092";
    public static string CharacteristicsTest2 = "characteristics-test2";
    public static string ClickhouseCharacteristicsGroupIdTest2 = "clickhouse-characteristics-group-id-test2";
    public static int _10 = 10;
    public static string ChsCip = "chscip";
    public static string Characteristics = "Characteristics";
    public static string Symbol = "Symbol";
    public static string CreationDateTime = "CreationDateTime";
    public static Ttl Day = new()
    {
        ColumnName = "CreateDateTime",
        Interval = 1,
        IntervalType = TtlInterval.Day
    };
    public static string InstrumentId = "InstrumentId";
    public static SecondaryIndex MinMax = new()
    {
        ColumnName = "CreationDateTime",
        IndexName = "CreationDateTime",
        IndexType = SecondaryIndexType.MinMax,
        Granularity = 1
    };
    public static SecondaryIndex BloomFilter = new()
    {
        ColumnName = "CreationDateTime",
        IndexName = "CreationDateTime",
        IndexType = SecondaryIndexType.BloomFilter,
        Granularity = 1
    };
    public static Setting IndexGranularity = new()
    {
        Name = MergeTreeSetting.IndexGranularityName,
        Value = "8192"
    };
    public static PartitionColumn CreationDateTimePartitionColumn = new()
    {
        IsColumnTypeDateTime = true,
        PartitionDateFormat = PartitionDateFormats.ToYYYYMMDD,
        ColumnName = "CreationDateTime"

    };
}