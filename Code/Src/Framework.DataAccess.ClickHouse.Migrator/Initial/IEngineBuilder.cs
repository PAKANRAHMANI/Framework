namespace Framework.DataAccess.ClickHouse.Migrator.Initial;

public interface IEngineBuilder
{
    IMergeTreeBuilder AsMergeTree();
    IReplacingMergeTreeBuilder AsReplicatedMergeTree();
    IReplacingMergeTreeBuilder AsReplacingMergeTree();
    IReplacingMergeTreeBuilder AsReplicatedReplacingMergeTree();
}