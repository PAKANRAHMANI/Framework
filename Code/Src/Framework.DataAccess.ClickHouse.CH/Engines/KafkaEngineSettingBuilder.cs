namespace Framework.DataAccess.CH.Engines;

public class KafkaEngineSettingBuilder
{
    private string _bootstrapServers;
    private string _topics;
    private string _consumerGroupId;
    private string _format;
    private int _consumerCount;
    private int _threadPerConsumerCount;
    private int _maxBlockSize;
    private int _flushIntervalMs;
    private int _pollMaxBatchSize;
    public KafkaEngineSettingBuilder()
    {
        _bootstrapServers = string.Empty;
        _topics = string.Empty;
        _consumerGroupId = string.Empty;
        _format = "JSONEachRow";
        _consumerCount = 1;
        _threadPerConsumerCount = 1;
        _maxBlockSize = 65536;
        _flushIntervalMs = 7500;
        _pollMaxBatchSize = 65536;
    }
    public KafkaEngineSettingBuilder WithBootstrapServers(string bootstrapServers)
    {
        _bootstrapServers = bootstrapServers;

        return this;
    }
    public KafkaEngineSettingBuilder WithTopics(string topics)
    {
        _topics = topics;

        return this;
    }
    public KafkaEngineSettingBuilder WithConsumerGroupId(string consumerGroupId)
    {
        _consumerGroupId = consumerGroupId;

        return this;
    }
    public KafkaEngineSettingBuilder WithFormat(string format)
    {
        _format = format;

        return this;
    }
    public KafkaEngineSettingBuilder WithConsumerCount(int consumerCount)
    {
        _consumerCount = consumerCount;

        return this;
    }
    public KafkaEngineSettingBuilder WithThreadPerConsumerCount(int threadPerConsumerCount)
    {
        _threadPerConsumerCount = threadPerConsumerCount;

        return this;
    }
    public KafkaEngineSettingBuilder WithMaxBlockSize(int maxBlockSize)
    {
        _maxBlockSize = maxBlockSize;

        return this;
    }
    public KafkaEngineSettingBuilder WithFlushIntervalMs(int flushIntervalMs)
    {
        _flushIntervalMs = flushIntervalMs;

        return this;
    }
    public KafkaEngineSettingBuilder WithPollMaxBatchSize(int pollMaxBatchSize)
    {
        _pollMaxBatchSize = pollMaxBatchSize;

        return this;
    }

    public KafkaEngineSetting Build()
    {
        return new KafkaEngineSetting
        {
            BootstrapServers = _bootstrapServers,
            Topics = _topics,
            ConsumerGroupId = _consumerGroupId,
            Format = _format,
            ConsumerCount = _consumerCount,
            FlushIntervalMs = _flushIntervalMs,
            PollMaxBatchSize = _pollMaxBatchSize,
            MaxBlockSize = _maxBlockSize,
            ThreadPerConsumerCount = _threadPerConsumerCount
        };
    }
}