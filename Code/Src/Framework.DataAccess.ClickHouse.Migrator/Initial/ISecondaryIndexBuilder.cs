using Framework.DataAccess.ClickHouse.Migrator.Tables;

namespace Framework.DataAccess.ClickHouse.Migrator.Initial;

public interface ISecondaryIndexBuilder
{
    IMergeTreeBuilder WithSecondaryIndices(params SecondaryIndex[] indices);
}