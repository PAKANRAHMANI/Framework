namespace Framework.DataAccess.ClickHouse.Columns;

public static class ClickHouseDataTypes
{
    // Integer Types
    public static readonly string UInt8 = "UInt8";
    public static readonly string UInt16 = "UInt16";
    public static readonly string UInt32 = "UInt32";
    public static readonly string UInt64 = "UInt64";
    public static readonly string Int8 = "Int8";
    public static readonly string Int16 = "Int16";
    public static readonly string Int32 = "Int32";
    public static readonly string Int64 = "Int64";

    // Floating-Point Types
    public static readonly string Float32 = "Float32";
    public static readonly string Float64 = "Float64";

    // String Types
    public static readonly string String = "String";
    public static readonly string FixedString = "FixedString";

    // Boolean Type
    public static readonly string Bool = "Bool";

    // Date and Time Types
    public static readonly string Date = "Date";
    public static readonly string DateTime = "DateTime";

    // Array Type
    public static string Array(string type) => $"Array({type})";

    // Tuple Type
    public static string Tuple(params string[] types) => $"Tuple({string.Join(", ", types)})";

    // Nullable Type
    public static string Nullable(string type) => $"Nullable({type})";

    // Enum Types
    public static readonly string Enum8 = "Enum8";
    public static readonly string Enum16 = "Enum16";

    // Nested Type
    public static string Nested(params string[] types) => $"Nested({string.Join(", ", types)})";

    // Geo Types
    public static readonly string Point = "Point";
    public static readonly string Polygon = "Polygon";
    public static readonly string LineString = "LineString";

    // UUID Type
    public static readonly string UUID = "UUID";

    // Decimal Type
    public static string Decimal(int precision, int scale) => $"Decimal({precision}, {scale})";

    // Example method to get all types
    public static IEnumerable<string> GetAllDataTypes()
    {
        yield return UInt8;
        yield return UInt16;
        yield return UInt32;
        yield return UInt64;
        yield return Int8;
        yield return Int16;
        yield return Int32;
        yield return Int64;
        yield return Float32;
        yield return Float64;
        yield return String;
        yield return FixedString;
        yield return Bool;
        yield return Date;
        yield return DateTime;
        yield return Point;
        yield return Polygon;
        yield return LineString;
        yield return UUID;
        yield return Enum8;
        yield return Enum16;
    }
}