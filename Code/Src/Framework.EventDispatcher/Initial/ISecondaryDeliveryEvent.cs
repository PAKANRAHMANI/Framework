using Framework.EventProcessor.Events.Kafka;
using Framework.EventProcessor.Events.MassTransit;

namespace Framework.EventProcessor.Initial;

public interface ISecondaryDeliveryEvent
{
    IEventProcessor SecondaryDeliveryWithKafka(Action<SecondaryProducerConfiguration> config);
    IEventProcessor SecondaryDeliveryWithMassTransit(Action<SecondaryMassTransitConfiguration> config);
}