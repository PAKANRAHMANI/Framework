namespace Framework.DataAccess.ClickHouse.Engines;

public enum PartitionDateFormat
{
    toYYYYMMDD,
    toYYYYMM,
    toYYYY,
    toQuarter,
    toWeek,
    toDayOfYear,
    toHour,
    toMinute,
    toMonth,
    toDayOfMonth,
    toDayOfWeek,
    toStartOfYear,
    toStartOfMonth,
    toStartOfQuarter,
    toStartOfWeek
}