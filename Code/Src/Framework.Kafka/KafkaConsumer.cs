using Confluent.Kafka;
using System;
using System.Threading;

namespace Framework.Kafka
{
    public class KafkaConsumer<TKey, TMessage> : IKafkaConsumer<TKey, TMessage>
    {
        private readonly KafkaConfiguration _configuration;
        private readonly IConsumer<TKey, TMessage> _consumerBuilder;
        public KafkaConsumer(KafkaConfiguration configuration)
        {
            _configuration = configuration;
            var config = new ConsumerConfig
            {
                GroupId = configuration.GroupId,
                BootstrapServers = configuration.BootstrapServers,
                EnableAutoOffsetStore = configuration.EnableAutoOffsetStore,
                AutoOffsetReset = configuration.AutoOffsetReset
            };

            _consumerBuilder = new ConsumerBuilder<TKey, TMessage>(config).Build();
        }

        public void Consume(Action<Message<TKey, TMessage>> action)
        {

            _consumerBuilder.Subscribe(_configuration.TopicName);

            var cancellationTokenSource = new CancellationTokenSource();

            while (true & !cancellationTokenSource.IsCancellationRequested)
            {
                var consumeResult = _consumerBuilder.Consume(cancellationTokenSource.Token);

                action(consumeResult.Message);
            }
        }

        public void Consume(Action<Message<TKey,TMessage>> action, int partitionNumber)
        {

            _consumerBuilder.Assign(new TopicPartition(_configuration.TopicName, new Partition(partitionNumber)));

            var cancellationTokenSource = new CancellationTokenSource();

            while (true & !cancellationTokenSource.IsCancellationRequested)
            {
                var consumeResult = _consumerBuilder.Consume(cancellationTokenSource.Token);

                action(consumeResult.Message);
            }
        }
    }
}
