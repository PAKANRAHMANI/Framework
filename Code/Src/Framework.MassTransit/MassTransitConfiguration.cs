using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit.RabbitMqTransport;
using RabbitMQ.Client;

namespace Framework.MassTransit
{
    public class MassTransitConfiguration
    {
        public string Connection { get; set; }
        public string QueueName { get; set; }
        public byte? Priority { get; set; }
        public int PrefetchCount { get; set; }
        public RetryConfiguration RetryConfiguration { get; set; }
        public string ExchangeType { get; set; }
        public bool ConfigureConsumeTopology { get; set; }
    }
}
