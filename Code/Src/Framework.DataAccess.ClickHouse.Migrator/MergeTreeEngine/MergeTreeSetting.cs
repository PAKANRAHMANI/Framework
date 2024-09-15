namespace Framework.DataAccess.ClickHouse.Migrator.MergeTreeEngine;

public static class MergeTreeSetting
{
    public static readonly string IndexGranularityName = "index_granularity";
    public static readonly string IndexGranularityBytesName = "index_granularity_bytes";
    public static readonly string MinBytesForWidePartName = "min_bytes_for_wide_part";
    public static readonly string MinRowsForWidePartName = "min_rows_for_wide_part";
    public static readonly string MinBytesForCompactPartName = "min_bytes_for_compact_part";
    public static readonly string MinRowsForCompactPartName = "min_rows_for_compact_part";
    public static readonly string MaxPartsInTotalName = "max_parts_in_total";
    public static readonly string MergeMaxBlockSizeName = "merge_max_block_size";
    public static readonly string MergeWithTtlTimeoutName = "merge_with_ttl_timeout";
    public static readonly string MaxPartRemovalThreadsName = "max_part_removal_threads";
    public static readonly string WriteFinalMarkName = "write_final_mark";
    public static readonly string ReplicatedDeduplicationWindowName = "replicated_deduplication_window";
    public static readonly string ReplicatedDeduplicationWindowSecondsName = "replicated_deduplication_window_seconds";
    public static readonly string InMemoryPartsEnableWalName = "in_memory_parts_enable_wal";
    public static readonly string InMemoryPartsEnableFlushName = "in_memory_parts_enable_flush";
    public static readonly string InMemoryPartsFlushIntervalMillisecondsName = "in_memory_parts_flush_interval_milliseconds";
    public static readonly string PartsToDelayInsertName = "parts_to_delay_insert";
    public static readonly string PartsToThrowInsertName = "parts_to_throw_insert";
    public static readonly string MaxDelayToInsertPartName = "max_delay_to_insert_part";
    public static readonly string AllowNullableKeyName = "allow_nullable_key";
    public static readonly string PreferFetchMergedPartSizeThresholdName = "prefer_fetch_merged_part_size_threshold";
    public static readonly string AllowRemoteFsReadPriorityName = "allow_remote_fs_read_priority";
    public static readonly string ForceCompressionTtlTimeoutName = "force_compression_ttl_timeout";
}