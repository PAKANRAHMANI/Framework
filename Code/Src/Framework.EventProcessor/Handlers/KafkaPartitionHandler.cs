using Confluent.Kafka;
using Framework.Core.Events;

namespace Framework.EventProcessor.Handlers;

internal sealed class KafkaPartitionHandler(IProducer<string, object> secondaryProducer) : TemplateHandler
{
    protected override bool CanHandle(KafkaData data)
    {
        return data.UseOfSpecificPartition;
    }

    protected override async Task MessageSend(KafkaData data)
    {
        await secondaryProducer.ProduceAsync(new TopicPartition(data.TopicName, new Partition(data.PartitionNumber)), new Message<string, object>
        {
            Value = data.Event,
            Key = data.TopicKey
        });
    }
}