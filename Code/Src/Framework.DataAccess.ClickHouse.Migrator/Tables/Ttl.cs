namespace Framework.DataAccess.ClickHouse.Migrator.Tables;

public class Ttl
{
    public string ColumnName { get; set; }
    public long Interval { get; set; }
    public TtlInterval IntervalType { get; set; }
}