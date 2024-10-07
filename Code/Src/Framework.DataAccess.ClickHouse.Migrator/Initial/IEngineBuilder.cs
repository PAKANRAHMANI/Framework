namespace Framework.DataAccess.ClickHouse.Migrator.Initial;

public interface IEngineBuilder
{
    IMergeTreeBuilder AsMergeTree();
    IMergeTreeBuilder AsReplicatedMergeTree();
    IReplacingMergeTreeBuilder AsReplacingMergeTree();
    IReplacingMergeTreeBuilder AsReplicatedReplacingMergeTree();
}