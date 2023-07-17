using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Core.Streaming;

namespace Framework.EventProcessor.Events.Kafka
{
    internal static class KafkaProducerFactory<TKey, TMessage> where TMessage : IStream
    {
        internal static KafkaProducer<TKey, TMessage> Create(bool useOfSpecificPartition,ProducerConfiguration configuration)
        {
            if (useOfSpecificPartition)
                return new KafkaPartitionProducer<TKey, TMessage>(configuration);

            return new KafkaTopicProducer<TKey, TMessage>(configuration);
        }
    }
}
