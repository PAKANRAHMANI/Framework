using Framework.EventProcessor.Events.Kafka;
using Framework.EventProcessor.Events.MassTransit;

namespace Framework.EventProcessor.Initial;

public interface IEventSenderBuilder
{
    IEnableSecondSenderBuilder PublishEventWithMassTransit(Action<MassTransitConfig> config);
    IEnableSecondSenderBuilder ProduceMessageWithKafka(Action<ProducerConfiguration> config);
}