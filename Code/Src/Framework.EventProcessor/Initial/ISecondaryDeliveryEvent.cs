using Framework.EventProcessor.Configurations;

namespace Framework.EventProcessor.Initial;

internal interface ISecondaryDeliveryEvent
{
    IEventConsumer SecondaryDeliveryWithKafka(Action<SecondaryProducerConfiguration> config);
    IEventConsumer SecondaryDeliveryWithMassTransit(Action<SecondaryMassTransitConfiguration> config);
}