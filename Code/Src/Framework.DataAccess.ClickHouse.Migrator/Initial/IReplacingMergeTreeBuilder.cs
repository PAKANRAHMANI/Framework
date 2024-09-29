using Framework.DataAccess.ClickHouse.Migrator.Partitions;

namespace Framework.DataAccess.ClickHouse.Migrator.Initial;

public interface IReplacingMergeTreeBuilder
{
    IMergeTreeBuilder WithVersionColumn(string versionColumn);
}