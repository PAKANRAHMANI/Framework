namespace Framework.DataAccess.ClickHouse.Migrator.Tables;

public static class TtlIntervalExtensions
{
    public static string GetValue(this TtlInterval interval)
    {
        return interval switch
        {
            TtlInterval.Second => "SECOND",
            TtlInterval.Minute => "MINUTE",
            TtlInterval.Hour => "HOUR",
            TtlInterval.Day => "DAY",
            TtlInterval.Week => "WEEK",
            TtlInterval.Month => "MONTH",
            TtlInterval.Quarter => "QUARTER",
            TtlInterval.Year => "YEAR",
            _ => throw new ArgumentOutOfRangeException(nameof(interval), interval, null)
        };
    }
}