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

    protected abstract List<Column> GetColumns();
    protected abstract MergeTreeTable GetMergeTreeTable();
    protected abstract KafkaEngineSetting GetKafkaEngineSetting();
    protected abstract ClickHouseConfiguration GetClickHouseConfiguration();
}