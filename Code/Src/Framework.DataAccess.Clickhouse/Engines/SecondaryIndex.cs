namespace Framework.DataAccess.ClickHouse.Engines;

public class SecondaryIndex
{
    public string IndexName { get; set; }
    public string ColumnName { get; set; }
    public long Granularity { get; set; }
    public ClickhouseIndex IndexType { get; set; }
    public List<string> IndexParameters { get; set; }
}