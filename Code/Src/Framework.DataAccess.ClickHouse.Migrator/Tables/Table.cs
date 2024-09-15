using Framework.DataAccess.ClickHouse.Migrator.Partitions;
using System.Globalization;

namespace Framework.DataAccess.ClickHouse.Migrator.Tables
{
    public abstract class Table(string databaseName, string tableName, string clusterName)
    {
        public string DatabaseName { get; init; } = databaseName;
        public string TableName { get; init; } = tableName;
        public string ClusterName { get; init; } = clusterName;
    }
    //public class DistributedTable(string databaseName, string tableName, string clusterName) : Table(databaseName, tableName, clusterName)
    //{
    //    public string HashColumnName { get; init; }
    //}
    //public class MergeTreeTable(string databaseName, string tableName, string clusterName) : Table(databaseName, tableName, clusterName)
    //{
    //    public MergeTreeEngineType MergeTreeEngineType { get; init; }
    //    public bool UsePartition { get; init; }
    //    public bool UseTtl { get; init; }
    //    public bool UseSecondaryIndex { get; init; }
    //    public Ttl Ttl { get; set; }
    //    public List<SecondaryIndex> SecondaryIndices { get; set; }
    //    public List<PartitionColumn> PartitionColumns { get; set; }
    //    public List<string> PrimaryKeyColumns { get; init; }
    //    public List<Column> Columns { get; init; }

    //    private bool ValidateTtlColumn()
    //    {
    //        if (PartitionColumns.Count <= 0) return false;

    //        var ttlColumn = Columns.FirstOrDefault(column => column.Name == Ttl.ColumnName);

    //        if (ttlColumn == null)
    //            return false;

    //        return ttlColumn.Type.Equals("DateTime", StringComparison.OrdinalIgnoreCase) ||
    //               ttlColumn.Type.Equals("Date", StringComparison.OrdinalIgnoreCase);
    //    }
    //}
    //public class SecondaryIndex
    //{
    //    public string IndexName { get; set; }
    //    public string ColumnName { get; set; }
    //    public long Granularity { get; set; }
    //    public SecondaryIndexType IndexType { get; set; }
    //    public List<string> IndexParameters { get; set; }
    //}
    //public enum SecondaryIndexType
    //{
    //    MinMax,
    //    Set,
    //    BloomFilter,
    //    TokenBfV1,
    //    NgramBfV1
    //}
    //public class Ttl
    //{
    //    public string ColumnName { get; set; }
    //    public long Interval { get; set; }
    //    public TtlInterval IntervalType { get; set; }
    //}
    //public enum TtlInterval
    //{
    //    Second,
    //    Minute,
    //    Hour,
    //    Day,
    //    Week,
    //    Month,
    //    Quarter,
    //    Year
    //}
    //public enum MergeTreeEngineType
    //{
    //    MergeTree,
    //    SummingMergeTree,
    //    AggregatingMergeTree,
    //    ReplacingMergeTree,
    //    ReplicatedMergeTree,
    //    ReplicatedSummingMergeTree,
    //    ReplicatedReplacingMergeTree,
    //}
    //public class Column
    //{
    //    public string Name { get; set; }
    //    public string Type { get; set; }
    //}
    //public static class ColumnDataTypes
    //{
    //    // Integer Types
    //    public static readonly string UInt8 = "UInt8";
    //    public static readonly string UInt16 = "UInt16";
    //    public static readonly string UInt32 = "UInt32";
    //    public static readonly string UInt64 = "UInt64";
    //    public static readonly string Int8 = "Int8";
    //    public static readonly string Int16 = "Int16";
    //    public static readonly string Int32 = "Int32";
    //    public static readonly string Int64 = "Int64";

    //    // Floating-Point Types
    //    public static readonly string Float32 = "Float32";
    //    public static readonly string Float64 = "Float64";

    //    // String Types
    //    public static readonly string String = "String";
    //    public static readonly string FixedString = "FixedString";

    //    // Boolean Type
    //    public static readonly string Bool = "Bool";

    //    // Date and Time Types
    //    public static readonly string Date = "Date";
    //    public static readonly string DateTime = "DateTime";

    //    // Array Type
    //    public static string Array(string type) => $"Array({type})";

    //    // Tuple Type
    //    public static string Tuple(params string[] types) => $"Tuple({string.Join(", ", types)})";

    //    // Nullable Type
    //    public static string Nullable(string type) => $"Nullable({type})";

    //    // Enum Types
    //    public static readonly string Enum8 = "Enum8";
    //    public static readonly string Enum16 = "Enum16";

    //    // Nested Type
    //    public static string Nested(params string[] types) => $"Nested({string.Join(", ", types)})";

    //    // Geo Types
    //    public static readonly string Point = "Point";
    //    public static readonly string Polygon = "Polygon";
    //    public static readonly string LineString = "LineString";

    //    // UUID Type
    //    public static readonly string UUID = "UUID";

    //    // Decimal Type
    //    public static string Decimal(int precision, int scale) => $"Decimal({precision}, {scale})";

    //    // Example method to get all types
    //    public static IEnumerable<string> GetAllDataTypes()
    //    {
    //        yield return UInt8;
    //        yield return UInt16;
    //        yield return UInt32;
    //        yield return UInt64;
    //        yield return Int8;
    //        yield return Int16;
    //        yield return Int32;
    //        yield return Int64;
    //        yield return Float32;
    //        yield return Float64;
    //        yield return String;
    //        yield return FixedString;
    //        yield return Bool;
    //        yield return Date;
    //        yield return DateTime;
    //        yield return Point;
    //        yield return Polygon;
    //        yield return LineString;
    //        yield return UUID;
    //        yield return Enum8;
    //        yield return Enum16;
    //    }
    //}
    //public class PartitionColumn
    //{
    //    public string ColumnName { get; set; }
    //    public PartitionDateFormat PartitionDateFormat { get; set; }
    //    public bool IsColumnTypeDateTime { get; set; }
    //}
    //public readonly struct PartitionDateFormat : IEquatable<PartitionDateFormat>, IComparable<PartitionDateFormat>
    //{
    //    private readonly IDateFormatter _formatter;
    //    public string Name { get; }

    //    private PartitionDateFormat(string name, IDateFormatter formatter)
    //    {
    //        Name = name ?? throw new ArgumentNullException(nameof(name));
    //        _formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
    //    }

    //    public static readonly PartitionDateFormat ToYYYYMMDD = new PartitionDateFormat("toYYYYMMDD", new DateFormatter(d => d.ToString("yyyyMMdd")));
    //    public static readonly PartitionDateFormat ToYYYYMM = new PartitionDateFormat("toYYYYMM", new DateFormatter(d => d.ToString("yyyyMM")));
    //    public static readonly PartitionDateFormat ToYYYY = new PartitionDateFormat("toYYYY", new DateFormatter(d => d.ToString("yyyy")));
    //    public static readonly PartitionDateFormat ToQuarter = new PartitionDateFormat("toQuarter", new QuarterFormatter());
    //    public static readonly PartitionDateFormat ToWeek = new PartitionDateFormat("toWeek", new WeekFormatter());
    //    public static readonly PartitionDateFormat ToDayOfYear = new PartitionDateFormat("toDayOfYear", new DateFormatter(d => d.DayOfYear.ToString()));
    //    public static readonly PartitionDateFormat ToHour = new PartitionDateFormat("toHour", new DateFormatter(d => d.ToString("yyyyMMddHH")));
    //    public static readonly PartitionDateFormat ToMinute = new PartitionDateFormat("toMinute", new DateFormatter(d => d.ToString("yyyyMMddHHmm")));
    //    public static readonly PartitionDateFormat ToMonth = new PartitionDateFormat("toMonth", new DateFormatter(d => d.ToString("MMMM")));
    //    public static readonly PartitionDateFormat ToDayOfMonth = new PartitionDateFormat("toDayOfMonth", new DateFormatter(d => d.ToString("dd")));
    //    public static readonly PartitionDateFormat ToDayOfWeek = new PartitionDateFormat("toDayOfWeek", new DateFormatter(d => d.DayOfWeek.ToString()));
    //    public static readonly PartitionDateFormat ToStartOfYear = new PartitionDateFormat("toStartOfYear", new DateFormatter(d => new DateTime(d.Year, 1, 1).ToString("yyyyMMdd")));
    //    public static readonly PartitionDateFormat ToStartOfMonth = new PartitionDateFormat("toStartOfMonth", new DateFormatter(d => new DateTime(d.Year, d.Month, 1).ToString("yyyyMMdd")));
    //    public static readonly PartitionDateFormat ToStartOfQuarter = new PartitionDateFormat("toStartOfQuarter", new StartOfQuarterFormatter());
    //    public static readonly PartitionDateFormat ToStartOfWeek = new PartitionDateFormat("toStartOfWeek", new StartOfWeekFormatter());

    //    public string Format(DateTime date) => _formatter.Format(date);
    //    public string Format(DateOnly date) => _formatter.Format(date.ToDateTime(TimeOnly.MinValue));
    //    public string Format(TimeOnly time) => _formatter.Format(DateTime.MinValue.Add(time.ToTimeSpan()));
    //    public string Format(DateTimeOffset date) => _formatter.Format(date.DateTime);

    //    public override string ToString() => Name;

    //    public override bool Equals(object obj) => obj is PartitionDateFormat format && Equals(format);
    //    public bool Equals(PartitionDateFormat other) => Name == other.Name;

    //    public override int GetHashCode() => Name.GetHashCode();
    //    public int CompareTo(PartitionDateFormat other) => string.Compare(Name, other.Name, StringComparison.Ordinal);

    //    public static bool operator ==(PartitionDateFormat left, PartitionDateFormat right) => left.Equals(right);
    //    public static bool operator !=(PartitionDateFormat left, PartitionDateFormat right) => !(left == right);
    //}
    //public interface IDateFormatter
    //{
    //    string Format(DateTime date);
    //}
    //public class DateFormatter(Func<DateTime, string> format) : IDateFormatter
    //{
    //    private readonly Func<DateTime, string> _format = format ?? throw new ArgumentNullException(nameof(format));

    //    public string Format(DateTime date) => _format(date);
    //}
    //public sealed class QuarterFormatter : IDateFormatter
    //{
    //    public string Format(DateTime date)
    //    {
    //        var quarter = (date.Month - 1) / 3 + 1;
    //        return $"{date.Year}Q{quarter}";
    //    }
    //}
    //public sealed class StartOfQuarterFormatter : IDateFormatter
    //{
    //    public string Format(DateTime date)
    //    {
    //        var startMonthOfQuarter = (date.Month - 1) / 3 * 3 + 1;
    //        return new DateTime(date.Year, startMonthOfQuarter, 1).ToString("yyyyMMdd");
    //    }
    //}
    //public sealed class StartOfWeekFormatter : IDateFormatter
    //{
    //    public string Format(DateTime date)
    //    {
    //        var dfi = CultureInfo.CurrentCulture.DateTimeFormat;
    //        var startOfWeek = date.AddDays(-(int)date.DayOfWeek + (int)dfi.FirstDayOfWeek);
    //        return startOfWeek.ToString("yyyyMMdd");
    //    }
    //}
    //public sealed class WeekFormatter : IDateFormatter
    //{
    //    public string Format(DateTime date)
    //    {
    //        var dfi = CultureInfo.CurrentCulture.DateTimeFormat;
    //        var weekOfYear = dfi.Calendar.GetWeekOfYear(date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
    //        return $"Week {weekOfYear}, {date.Year}";
    //    }
    //}
}
