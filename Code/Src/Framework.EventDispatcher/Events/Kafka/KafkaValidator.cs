using Confluent.Kafka;


namespace Framework.EventProcessor.Events.Kafka
{
    public class KafkaValidator : IKafkaValidator
    {
        private readonly AdminClientConfig _adminClientConfig;
        public KafkaValidator(ProducerConfiguration configuration)
        {
            _adminClientConfig = new AdminClientConfig()
            {
                BootstrapServers = configuration.BootstrapServer
            };
        }
        public bool TopicIsExist(string topic)
        {
            using var adminClient = new AdminClientBuilder(_adminClientConfig).Build();
            var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(3));
            var topicNames = metadata.Topics.Select(topicMetadata => topicMetadata.Topic).ToList();
            return topicNames.Any(topicName => topicName == topic);
        }
    }
}
