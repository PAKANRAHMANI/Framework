namespace Framework.DataAccess.ClickHouse.Migrator.KafkaEngine
{
    public sealed class KafkaEngineSetting
    {
        public string BootstrapServers { get; set; }
        public List<string> Topics { get; set; }
        public string ConsumerGroupId { get; set; }
        //Timeout for flushing data from Kafka
        public int FlushIntervalMs { get; set; }
    }
}
