using Framework.DataAccess.ClickHouse.Migrator.Columns;
using Framework.DataAccess.ClickHouse.Migrator.KafkaEngine;
using Framework.DataAccess.ClickHouse.Migrator.Migrations;
using Framework.DataAccess.ClickHouse.Migrator.Tables;

namespace Framework.DataAccess.ClickHouse.Migrator;

internal class TableFactory(MergeTreeTable mergeTreeTable, List<Column> columns, ClickHouseConfiguration clickHouseConfiguration)
{
    public void CreateKafkaEngine(KafkaEngineSetting setting)
    {
        var table = new Table(mergeTreeTable.DatabaseName, $"{mergeTreeTable.TableName}_Messages", mergeTreeTable.ClusterName);

        var kafkaMigration = new KafkaEngineTableMigration(table, columns, setting, clickHouseConfiguration);

        kafkaMigration.CreateTable();
    }

    public void CreateMergeTree()
    {
        var mergeTreeMigration = new MergeTreeTableMigration(mergeTreeTable, clickHouseConfiguration);

        mergeTreeMigration.CreateTable();
    }

    public void CreateDistributedTable(string hashColumnName,string replicatedTableName)
    {
        var distributedTable = new DistributedTable(hashColumnName, mergeTreeTable.DatabaseName, $"{mergeTreeTable.TableName}_Distributed", mergeTreeTable.ClusterName);

        var distributedMigration = new DistributedTableMigration(distributedTable, replicatedTableName, clickHouseConfiguration);

        distributedMigration.CreateTable();
    }
    public void CreateMaterializedViewMigration()
    {
        var table = new Table(mergeTreeTable.DatabaseName, $"{mergeTreeTable.TableName}", mergeTreeTable.ClusterName);

        var distributedMigration = new MaterializedViewMigration(table, clickHouseConfiguration);

        distributedMigration.CreateTable();
    }
}