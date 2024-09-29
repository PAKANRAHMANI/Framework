namespace Framework.DataAccess.ClickHouse.Migrator.Partitions;

public class PartitionColumn
{
    public string ColumnName { get; set; }
    public string PartitionDateFormat { get; set; }
    public bool IsColumnTypeDateTime { get; set; }
}