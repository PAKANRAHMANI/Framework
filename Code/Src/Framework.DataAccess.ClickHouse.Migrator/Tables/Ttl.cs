namespace Framework.DataAccess.ClickHouse.Migrator.Tables;

public class Ttl
{
    public string ColumnName { get; set; }
    public string Condition { get; set; }
    public bool UseCondition { get; set; }
    public bool IsGenerateConditionOnDateTimeByFramework { get; set; }
    public string GroupByColumn { get; set; }
    public long Interval { get; set; }
    public TtlInterval IntervalType { get; set; }
}