namespace Framework.DataAccess.ClickHouse.Engines;

public class Ttl
{
    public string ColumnName { get; set; }
    public long Interval { get; set; }
    public TtlInterval IntervalType { get; set; }
}