namespace Framework.DataAccess.ClickHouse.Migrator.MergeTreeEngine;

public enum MergeTreeEngineType
{
    MergeTree,
    SummingMergeTree,
    AggregatingMergeTree,
    ReplacingMergeTree,
    ReplicatedMergeTree,
    ReplicatedSummingMergeTree,
    ReplicatedReplacingMergeTree,
}