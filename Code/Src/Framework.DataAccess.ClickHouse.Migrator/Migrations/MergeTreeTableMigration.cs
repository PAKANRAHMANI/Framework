using System.Text;
using Framework.DataAccess.ClickHouse.Migrator.Executors;
using Framework.DataAccess.ClickHouse.Migrator.MergeTreeEngine;
using Framework.DataAccess.ClickHouse.Migrator.Tables;

namespace Framework.DataAccess.ClickHouse.Migrator.Migrations;

internal class MergeTreeTableMigration(MergeTreeTable table, ClickHouseConfiguration clickHouseConfiguration) : IMigration
{
    public string CreateTable()
    {
        var columnsBuilder = new StringBuilder();

        foreach (var column in table.Columns)
        {
            columnsBuilder.Append($"`{column.Name}` {column.Type},");
        }
        var columns = columnsBuilder.ToString();

        if (columns.EndsWith(","))
            columns = columns.TrimEnd(',');

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
                    tableBuilder.AppendLine($",INDEX {index.IndexName}_idx ({index.ColumnName}) TYPE {index.IndexType.GetValue()}({parameters}) GRANULARITY {index.Granularity}");
                }
                else
                {
                    tableBuilder.AppendLine($",INDEX {index.IndexName}_idx ({index.ColumnName}) TYPE {index.IndexType.GetValue()} GRANULARITY {index.Granularity}");
                }
            }
        }

        tableBuilder.AppendLine(")");

        if (table.IsReplicated && table.MergeTreeEngineType == MergeTreeEngineType.ReplicatedReplacingMergeTree)
        {
            tableBuilder.AppendLine($"ENGINE = {table.MergeTreeEngineType}('/clickhouse/tables/{table.TableName}/{{shard}}', '{{replica}}',{table.VersionColumn})");
        }
        else if (table.MergeTreeEngineType == MergeTreeEngineType.ReplacingMergeTree)
        {
            tableBuilder.AppendLine($"ENGINE = {table.MergeTreeEngineType}({table.VersionColumn})");

        }
        else if (table.IsReplicated && table.MergeTreeEngineType == MergeTreeEngineType.ReplicatedMergeTree)
        {
            tableBuilder.AppendLine($"ENGINE = {table.MergeTreeEngineType}('/clickhouse/tables/{{shard}}/{table.TableName}', '{{replica}}')");
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
                    partitionColumnsBuilder.Append($"{column.ColumnName},");
                }
            }
            var partitionColumns = partitionColumnsBuilder.ToString();

            if (partitionColumns.EndsWith(","))
                partitionColumns = partitionColumns.TrimEnd(',');

            tableBuilder.AppendLine($"PARTITION BY ({partitionColumns})");
        }


        if (table.UseTtl && table.Ttl.UseCondition == false && table.Columns.Any(a => a.Name.Equals(table.Ttl.ColumnName, StringComparison.OrdinalIgnoreCase)))
        {
            tableBuilder.AppendLine($"TTL {table.Ttl.ColumnName} + INTERVAL {table.Ttl.Interval} {table.Ttl.IntervalType.GetValue()} DELETE ");
        }

        var settingBuilder = new StringBuilder();

        settingBuilder.AppendLine($"SETTINGS ");

        foreach (var setting in table.Settings)
        {
            settingBuilder.AppendLine($"{setting.Name} = {setting.Value},");
        }

        var settings = settingBuilder.ToString();

        if (settings.EndsWith(",\r\n") || settings.EndsWith(",\n\n"))
            settings = settings.AsSpan(0, settings.Length - 3).ToString();

        tableBuilder.AppendLine(settings);

        var command = tableBuilder.ToString();

        command = command.Replace("\n", " ");

       // if (command.Trim().EndsWith(",") )
            command = command.Trim().AsSpan(0, command.Length - 1).ToString();

        return command;
    }

    public async Task DropTable(string tableName, string clusterName)
    {
        var command = $"DROP TABLE IF EXISTS {tableName} ON CLUSTER {clusterName};";

        await ExecuteCommand.Execute(clickHouseConfiguration, command);

    }
}