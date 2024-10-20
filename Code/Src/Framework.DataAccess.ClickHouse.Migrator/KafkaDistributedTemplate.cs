using Framework.DataAccess.ClickHouse.Migrator.Columns;
using Framework.DataAccess.ClickHouse.Migrator.KafkaEngine;
using Framework.DataAccess.ClickHouse.Migrator.Tables;

namespace Framework.DataAccess.ClickHouse.Migrator;

public abstract class KafkaDistributedTemplate
{
    public async Task Create(string distributedHashColumn)
    {
        var columns = GetColumns();

        var kafkaSetting = GetKafkaEngineSetting();

        var mergeTreeTable = GetMergeTreeTable();

        mergeTreeTable.Columns.AddRange(columns);

        var clickHouseConfig = GetClickHouseConfiguration();

        var tableFactory = new TableFactory(mergeTreeTable, columns, clickHouseConfig);

        await tableFactory.CreateKafkaEngine(kafkaSetting);

        await tableFactory.CreateMergeTree();

        await tableFactory.CreateDistributedTable(distributedHashColumn, mergeTreeTable.TableName);

        await tableFactory.CreateMaterializedViewMigration();

        if (mergeTreeTable.UseTtl)
            await tableFactory.ModifyMergeTreeByTtl(mergeTreeTable);
    }

    public List<string> GetTables(string distributedHashColumn)
    {
        var tables = new List<string>();

        var columns = GetColumns();

        var kafkaSetting = GetKafkaEngineSetting();

        var mergeTreeTable = GetMergeTreeTable();

        mergeTreeTable.Columns.AddRange(columns);

        var clickHouseConfig = GetClickHouseConfiguration();

        var tableFactory = new TableQueryFactory(mergeTreeTable, columns, clickHouseConfig);

        tables.Add(tableFactory.GetKafkaEngine(kafkaSetting));

        tables.Add(tableFactory.GetMergeTree());

        tables.Add(tableFactory.GetDistributedTable(distributedHashColumn, mergeTreeTable.TableName));

        tables.Add(tableFactory.GetMaterializedViewMigration());

        if (mergeTreeTable.UseTtl)
            tables.Add(tableFactory.ModifyMergeTreeByTtl(mergeTreeTable));

        return tables;
    }
    protected abstract List<Column> GetColumns();
    protected abstract MergeTreeTable GetMergeTreeTable();
    protected abstract KafkaEngineSetting GetKafkaEngineSetting();
    protected abstract ClickHouseConfiguration GetClickHouseConfiguration();
}