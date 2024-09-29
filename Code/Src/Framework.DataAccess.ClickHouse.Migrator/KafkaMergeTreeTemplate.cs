using Framework.DataAccess.ClickHouse.Migrator.Columns;
using Framework.DataAccess.ClickHouse.Migrator.KafkaEngine;
using Framework.DataAccess.ClickHouse.Migrator.Tables;

namespace Framework.DataAccess.ClickHouse.Migrator;

public abstract class KafkaMergeTreeTemplate
{
    public void Create()
    {
        var columns = GetColumns();

        var kafkaSetting = GetKafkaEngineSetting();

        var mergeTreeTable = GetMergeTreeTable();

        mergeTreeTable.Columns.AddRange(columns);

        var clickHouseConfig = GetClickHouseConfiguration();

        var tableFactory = new TableFactory(mergeTreeTable, columns, clickHouseConfig);

        tableFactory.CreateKafkaEngine(kafkaSetting);

        tableFactory.CreateMergeTree();

        tableFactory.CreateMaterializedViewMigration();
    }

    protected abstract List<Column> GetColumns();
    protected abstract MergeTreeTable GetMergeTreeTable();
    protected abstract KafkaEngineSetting GetKafkaEngineSetting();
    protected abstract ClickHouseConfiguration GetClickHouseConfiguration();
}