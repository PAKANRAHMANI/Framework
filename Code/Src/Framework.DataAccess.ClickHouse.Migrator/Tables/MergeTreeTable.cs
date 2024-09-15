using Framework.DataAccess.ClickHouse.Migrator.Columns;
using Framework.DataAccess.ClickHouse.Migrator.MergeTreeEngine;
using Framework.DataAccess.ClickHouse.Migrator.Partitions;

namespace Framework.DataAccess.ClickHouse.Migrator.Tables;

public class MergeTreeTable(string databaseName, string tableName, string clusterName) : Table(databaseName, tableName, clusterName)
{
    public MergeTreeEngineType MergeTreeEngineType { get; init; }
    public bool UsePartition { get; init; }
    public bool UseTtl { get; init; }
    public bool UseSecondaryIndex { get; init; }
    public bool IsReplicated { get; init; }
    public string VersionColumn { get; init; }
    public Ttl Ttl { get; init; }
    public List<SecondaryIndex> SecondaryIndices { get; init; }
    public List<PartitionColumn> PartitionColumns { get; init; }
    public List<string> PrimaryKeyColumns { get; init; }
    public List<Column> Columns { get; init; }
    public List<Setting> Settings { get; init; }

    private bool ValidateTtlColumn()
    {
        if (PartitionColumns.Count <= 0) return false;

        var ttlColumn = Columns.FirstOrDefault(column => column.Name == Ttl.ColumnName);

        if (ttlColumn == null)
            return false;

        return ttlColumn.Type.Equals("DateTime", StringComparison.OrdinalIgnoreCase) ||
               ttlColumn.Type.Equals("Date", StringComparison.OrdinalIgnoreCase);
    }
}