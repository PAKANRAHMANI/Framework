namespace Framework.DataAccess.ClickHouse.Migrator.Tables;

public enum TtlInterval
{
    Second,
    Minute,
    Hour,
    Day,
    Week,
    Month,
    Quarter,
    Year
}