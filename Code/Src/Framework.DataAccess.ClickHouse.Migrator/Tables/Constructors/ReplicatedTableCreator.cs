using System.Text;

namespace Framework.DataAccess.ClickHouse.Migrator.Tables.Constructors;

public class ReplicatedTableCreator(MergeTreeTable table) : ITableNodeCreator
{
    public void CreateTable()
    {
        var columnsBuilder = new StringBuilder();

        foreach (var column in table.Columns)
        {
            columnsBuilder.Append($"`{column.Name}` {column.Type},");
        }
        var columns = columnsBuilder.ToString();

        var tableBuilder = new StringBuilder();

        tableBuilder.Append($"CREATE TABLE IF NOT EXISTS {table.DatabaseName}.{table.TableName} ON CLUSTER {table.ClusterName}");

        tableBuilder.AppendLine("(");

        tableBuilder.AppendLine($"{columns}");

        if (table.UseSecondaryIndex)
        {
            foreach (var index in table.SecondaryIndices)
            {
                if (index.IndexParameters is not null && index.IndexParameters.Any())
                {
                    var parameters = string.Join(",", index.IndexParameters);
                    tableBuilder.AppendLine($",INDEX {index.IndexName}_idx ({index.ColumnName}) TYPE {index.IndexType}({parameters}) GRANULARITY {index.Granularity},");
                }
                else
                {
                    tableBuilder.AppendLine($",INDEX {index.IndexName}_idx ({index.ColumnName}) TYPE {index.IndexType} GRANULARITY {index.Granularity},");
                }
            }
        }

        tableBuilder.AppendLine(")");

        if (table.IsReplicated)
        {
            tableBuilder.AppendLine($"ENGINE = {table.MergeTreeEngineType}('/clickhouse/tables/Characteristics/{{shard}}', '{{replica}}',{table.VersionColumn})");
        }
        else
        {
            tableBuilder.AppendLine($"ENGINE = {table.MergeTreeEngineType}");
        }

        var primaryColumnsBuilder = new StringBuilder();

        foreach (var column in table.PrimaryKeyColumns)
        {
            primaryColumnsBuilder.Append($"{column},");
        }
        var primaryColumns = primaryColumnsBuilder.ToString();

        if (primaryColumns.EndsWith(","))
            primaryColumns = primaryColumns.TrimEnd(',');

        tableBuilder.AppendLine($"ORDER BY ({primaryColumns})");

        var partitionColumnsBuilder = new StringBuilder();

        if (table.UsePartition)
        {
            foreach (var column in table.PartitionColumns)
            {

                if (column.IsColumnTypeDateTime)
                {
                    var partitionBy = string.Format(column.PartitionDateFormat, column.ColumnName);
                    partitionColumnsBuilder.Append($"{partitionBy},");
                }
                else
                {
                    partitionColumnsBuilder.Append($"{column},");

                }
            }
            var partitionColumns = partitionColumnsBuilder.ToString();

            if (partitionColumns.EndsWith(","))
                partitionColumns = partitionColumns.TrimEnd(',');


            tableBuilder.AppendLine($"PARTITION BY ({partitionColumns})");
        }

        if (table.UseTtl)
        {
            tableBuilder.AppendLine($"TTL {table.Ttl.ColumnName} + INTERVAL {table.Ttl.Interval} {table.Ttl.IntervalType} DELETE");
        }

        var settingBuilder = new StringBuilder();

        settingBuilder.AppendLine($"SETTINGS ");

        foreach (var setting in table.Settings)
        {
            settingBuilder.AppendLine($"{setting},");
        }
        var settings = settingBuilder.ToString();

        if (settings.EndsWith(","))
            settings = settings.TrimEnd(',');

        settingBuilder.AppendLine(settings);
        
        var command = tableBuilder.ToString();
    }

    public void DropTable(string tableName, string clusterName)
    {
        var command = $"DROP TABLE IF EXISTS {tableName} ON CLUSTER {clusterName};";
    }
}