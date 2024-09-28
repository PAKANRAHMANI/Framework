namespace Framework.DataAccess.ClickHouse.Migrator.KafkaEngine
{
    public class KafkaEngineSettingBuilder
    {
        private string _bootstrapServers;
        private List<string> _topics;
        private string _consumerGroupId;
        private int _flushIntervalMs;

        public KafkaEngineSettingBuilder WithBootstrapServers(string bootstrapServers)
        {
            _bootstrapServers = bootstrapServers;

            return this;
        }
        public KafkaEngineSettingBuilder WithTopics(params string[] topics)
        {
            _topics = topics.ToList();

            return this;
        }
        public KafkaEngineSettingBuilder WithConsumerGroupId(string consumerGroupId)
        {
            _consumerGroupId = consumerGroupId;

            return this;
        }
    
        public KafkaEngineSettingBuilder WithFlushIntervalMs(int flushIntervalMs)
        {
            _flushIntervalMs = flushIntervalMs;

            return this;
        }
   
        public KafkaEngineSetting Build()
        {
            return new KafkaEngineSetting
            {
                BootstrapServers = _bootstrapServers,
                Topics = _topics,
                ConsumerGroupId = _consumerGroupId,
                FlushIntervalMs = _flushIntervalMs
            };
        }
    }
}
