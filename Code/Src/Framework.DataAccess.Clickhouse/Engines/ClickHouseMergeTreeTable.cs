using Framework.DataAccess.ClickHouse.Columns;

namespace Framework.DataAccess.ClickHouse.Engines;

public class ClickHouseMergeTreeTable
{
    public string ClusterName { get; init; }
    public string DatabaseName { get; init; }
    public string TableName { get; init; }
    public MergeTreeEngine MergeTreeEngine { get; init; }
    public bool UsePartition { get; init; }
    public bool UseTtl { get; init; }
    public bool UseSecondaryIndex { get; init; }
    public Ttl Ttl { get; set; }
    public List<SecondaryIndex> SecondaryIndices { get; set; }
    public List<PartitionColumn> PartitionColumns { get; set; }
    public List<string> PrimaryKeyColumns { get; init; }
    public List<ClickHouseColumn> Columns { get; init; }

    private bool ValidateTtlColumn()
    {
        if (PartitionColumns.Count <= 0) return false;

        var ttlColumn = this.Columns.FirstOrDefault(column => column.Name == this.Ttl.ColumnName);

        if (ttlColumn == null)
            return false;

        return ttlColumn.Type.Equals("DateTime", StringComparison.OrdinalIgnoreCase) ||
               ttlColumn.Type.Equals("Date", StringComparison.OrdinalIgnoreCase);
    }
}