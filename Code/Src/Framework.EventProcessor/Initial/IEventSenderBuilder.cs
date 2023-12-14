﻿using Framework.EventProcessor.Configurations;
using Framework.EventProcessor.Events.Kafka;

namespace Framework.EventProcessor.Initial;

internal interface IEventSenderBuilder
{
    IEnableSecondSenderBuilder PublishEventWithMassTransit(Action<MassTransitConfig> config);
    IEnableSecondSenderBuilder ProduceMessageWithKafka(Action<ProducerConfiguration> config, List<EventKafka> kafkaKeys);
}