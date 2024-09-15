namespace Framework.DataAccess.ClickHouse.Engines;

public enum MergeTreeEngine
{
    MergeTree,
    SummingMergeTree,
    AggregatingMergeTree,
    ReplacingMergeTree,
    CollapsingMergeTree,
    VersionedCollapsingMergeTree,
    GraphiteMergeTree,
    LogMergeTree,
    ReplicatedMergeTree,
    ReplicatedSummingMergeTree,
    ReplicatedAggregatingMergeTree,
    ReplicatedReplacingMergeTree,
    ReplicatedCollapsingMergeTree,
    ReplicatedVersionedCollapsingMergeTree,
    ReplicatedGraphiteMergeTree,
    ReplicatedLogMergeTree
}