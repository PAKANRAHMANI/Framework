using Framework.DataAccess.ClickHouse.Migrator.MergeTreeEngine;

namespace Framework.DataAccess.ClickHouse.Migrator.Initial;

public interface IMergeTreeBuilder
{
    IMergeTreeBuilder WithMergeTreeEngineType(MergeTreeEngineType engineType);
    IPartitionBuilder HasPartition();
    ITtlBuilder UseTtl();
    ISecondaryIndexBuilder HasSecondaryIndex();
    IMergeTreeBuilder WithPrimaryKeyColumns(params string[] primaryKeyColumns);
    IMergeTreeFamilyBuilder WithSettings(params Setting[] settings);
}