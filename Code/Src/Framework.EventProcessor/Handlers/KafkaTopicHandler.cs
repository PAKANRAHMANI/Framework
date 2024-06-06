using Confluent.Kafka;
using Framework.Core.Events;

namespace Framework.EventProcessor.Handlers;

internal sealed class KafkaTopicHandler(IProducer<string, object> secondaryProducer) : TemplateHandler
{
    protected override bool CanHandle(KafkaData data)
    {
        return data.UseOfSpecificPartition == false;
    }

    protected override async Task MessageSend(KafkaData data)
    {
        await secondaryProducer.ProduceAsync(data.TopicName, new Message<string, object>
        {
            Value = data.Event,
            Key = data.TopicKey
        });
    }
}