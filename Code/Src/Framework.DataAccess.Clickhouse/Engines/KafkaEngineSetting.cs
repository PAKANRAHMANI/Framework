namespace Framework.DataAccess.ClickHouse.Engines;

public sealed class KafkaEngineSetting
{
    public string BootstrapServers { get; set; }
    public string Topics { get; set; }
    public string ConsumerGroupId { get; set; }
    public string Format { get; set; }
    public int ConsumerCount { get; set; }
    public int ThreadPerConsumerCount { get; set; }
    public int MaxBlockSize { get; set; }
    public int FlushIntervalMs { get; set; }
    public int PollMaxBatchSize { get; set; }
}