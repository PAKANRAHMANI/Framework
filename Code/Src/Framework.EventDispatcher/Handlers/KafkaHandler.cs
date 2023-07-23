using Confluent.Kafka;
using Framework.Core.Events;
using Framework.EventProcessor.Events.Kafka;
using Framework.EventProcessor.Services;
using Microsoft.Extensions.Options;

namespace Framework.EventProcessor.Handlers
{
    public class KafkaHandler : TemplateHandler
    {
        private readonly SecondaryProducerConfiguration _producerConfig;
        private readonly IProducer<object, object> _producer;
        public KafkaHandler(IOptions<SecondaryProducerConfiguration> producerConfig)
        {
            _producerConfig = producerConfig.Value;
            _producer = KafkaSecondaryProducerFactory<object, object>.Create(producerConfig.Value);
        }
        protected override bool UseOfSecondarySending(ServiceConfig config)
        {
            return config.EnableSecondarySending && config.SendWithKafka;
        }

        protected override async Task MessageSend(IEvent @event)
        {
            if (_producerConfig.UseOfSpecificPartition)
            {
                await _producer.ProduceAsync(new TopicPartition(_producerConfig.TopicName, new Partition(_producerConfig.PartitionNumber)), new Message<object, object>
                {
                    Value = @event,
                    Key = _producerConfig.TopicKey
                });
            }
            else
            {
                await _producer.ProduceAsync(_producerConfig.TopicName, new Message<object, object>
                {
                    Value = @event,
                    Key = _producerConfig.TopicKey
                });
            }

        }
    }
}
