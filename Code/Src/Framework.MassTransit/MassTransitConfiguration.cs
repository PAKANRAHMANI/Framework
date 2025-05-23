﻿namespace Framework.MassTransit
{
    public class MassTransitConfiguration
    {
        public string Connection { get; set; }
        public string QueueName { get; set; }
        public bool ConfigureConsumeTopology { get; set; }
        public RetryConfiguration RetryConfiguration { get; set; }
        public int EndpointPrefetchCount { get; set; }
        public string EndpointExchangeType { get; set; }
        public string ProducerExchangeType { get; set; }
        public byte? Priority { get; set; }
    }
}