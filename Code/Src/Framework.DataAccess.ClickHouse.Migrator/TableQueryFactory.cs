using Framework.DataAccess.ClickHouse.Migrator.Columns;
using Framework.DataAccess.ClickHouse.Migrator.KafkaEngine;
using Framework.DataAccess.ClickHouse.Migrator.Migrations;
using Framework.DataAccess.ClickHouse.Migrator.Tables;

namespace Framework.DataAccess.ClickHouse.Migrator;

internal class TableQueryFactory(MergeTreeTable mergeTreeTable, List<Column> columns, ClickHouseConfiguration clickHouseConfiguration)
{
    public string GetKafkaEngine(KafkaEngineSetting setting)
    {
        var table = new Table(mergeTreeTable.DatabaseName, $"{mergeTreeTable.TableName}_Messages", mergeTreeTable.ClusterName);

        var kafkaMigration = new KafkaEngineTableMigration(table, columns, setting, clickHouseConfiguration);

        return kafkaMigration.CreateTable();
    }

    public string GetMergeTree()
    {
        var mergeTreeMigration = new MergeTreeTableMigration(mergeTreeTable, clickHouseConfiguration);

        return mergeTreeMigration.CreateTable();
    }

    public string GetDistributedTable(string hashColumnName, string replicatedTableName)
    {
        var distributedTable = new DistributedTable(hashColumnName, mergeTreeTable.DatabaseName, $"{mergeTreeTable.TableName}_Distributed", mergeTreeTable.ClusterName);

        var distributedMigration = new DistributedTableMigration(distributedTable, replicatedTableName, clickHouseConfiguration);

        return distributedMigration.CreateTable();
    }
    public string GetMaterializedViewMigration()
    {
        var table = new Table(mergeTreeTable.DatabaseName, $"{mergeTreeTable.TableName}", mergeTreeTable.ClusterName);

        var distributedMigration = new MaterializedViewMigration(table, clickHouseConfiguration);

        return distributedMigration.CreateTable();
    }

    public string ModifyMergeTreeByTtl(MergeTreeTable table)
    {
        var command = @$"";

        if (table.Ttl.IsGenerateConditionOnDateTimeByFramework && table.Ttl.UseCondition)
        {
            command = @$"ALTER TABLE {table.DatabaseName}.{table.TableName}
                                 MODIFY TTL  {table.Ttl.ColumnName} + INTERVAL {table.Ttl.Interval} {table.Ttl.IntervalType.GetValue()}
                                 DELETE WHERE({table.Ttl.GroupByColumn} ,  {table.Ttl.ColumnName} ) GLOBAL NOT  IN (
                                 SELECT {table.Ttl.GroupByColumn} , MAX({table.Ttl.ColumnName}) AS ttlColumnName
                                 FROM   {table.DatabaseName}.`{table.TableName}`_Distributed
                                 WHERE toDate({table.Ttl.ColumnName}) = today()
                                 GROUP BY {table.Ttl.GroupByColumn}
                                 UNION ALL
                                 SELECT {table.Ttl.GroupByColumn}, MAX({table.Ttl.ColumnName}) AS ttlColumnName
                                 FROM   {table.DatabaseName}.`{table.TableName}`_Distributed
                                 WHERE {table.Ttl.GroupByColumn} GLOBAL NOT IN (
                                     SELECT DISTINCT {table.Ttl.GroupByColumn}
                                     FROM  {table.DatabaseName}.`{table.TableName}`_Distributed
                                     WHERE toDate({table.Ttl.ColumnName}) = today()
                                    )
                                 GROUP BY {table.Ttl.GroupByColumn}
                                 )";

            return command;

        }
        else if (table.Ttl.UseCondition)
        {
            command = @$"ALTER TABLE {table.DatabaseName}.{table.TableName} MODIFY TTL {table.Ttl.ColumnName} + INTERVAL {table.Ttl.Interval} {table.Ttl.IntervalType.GetValue()} DELETE WHERE {table.Ttl.Condition}";

            return command;
        }
        return command;
    }
}