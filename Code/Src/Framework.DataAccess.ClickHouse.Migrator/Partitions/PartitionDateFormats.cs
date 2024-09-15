namespace Framework.DataAccess.ClickHouse.Migrator.Partitions;

public static class PartitionDateFormats
{
    public const string ToYYYYMMDD = "toYYYYMMDD({0})";
    public const string ToYYYYMM = "toYYYYMM({0})";
    public const string ToYYYY = "toYYYY({0})";
    public const string ToQuarter = "toQuarter({0})";
    public const string ToWeek = "toWeek({0})";
    public const string ToDayOfYear = "toDayOfYear({0})";
    public const string ToMonth = "toMonth({0})";
    public const string ToDayOfMonth = "toDayOfMonth({0})";
    public const string ToDayOfWeek = "toDayOfWeek({0})";
    public const string ToStartOfYear = "toStartOfYear({0})";
    public const string ToStartOfMonth = "toStartOfMonth({0})";
    public const string ToStartOfQuarter = "toStartOfQuarter({0})";
    public const string ToStartOfWeek = "toStartOfWeek({0})";
}