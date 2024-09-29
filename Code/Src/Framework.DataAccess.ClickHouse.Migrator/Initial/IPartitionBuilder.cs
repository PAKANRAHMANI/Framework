using Framework.DataAccess.ClickHouse.Migrator.Partitions;

namespace Framework.DataAccess.ClickHouse.Migrator.Initial;

public interface IPartitionBuilder
{
    IMergeTreeBuilder WithPartitionColumns(params PartitionColumn[] partitionColumns);
}