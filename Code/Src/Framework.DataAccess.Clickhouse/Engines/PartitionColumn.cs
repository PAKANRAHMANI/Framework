namespace Framework.DataAccess.ClickHouse.Engines;

public class PartitionColumn
{
    public string ColumnName { get; set; }
    public PartitionDateFormat PartitionDateFormat { get; set; }
    public bool IsColumnTypeDateTime { get; set; }
}