using Confluent.Kafka;
using System;
using System.Threading;
using System.Threading.Tasks;

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
                BootstrapServers = configuration.ConsumerBootstrapServers,
                EnableAutoOffsetStore = configuration.EnableAutoOffsetStore,
                AutoOffsetReset = configuration.AutoOffsetReset
            };

            _consumerBuilder = new ConsumerBuilder<TKey, TMessage>(config).Build();
        }

        public void Consume(Action<ConsumeResult<TKey, TMessage>> action)
        {
            _consumerBuilder.Subscribe(_configuration.ConsumerTopicName);

            var cancellationTokenSource = new CancellationTokenSource();

            while (true & !cancellationTokenSource.IsCancellationRequested)
            {
                var consumeResult = _consumerBuilder.Consume(cancellationTokenSource.Token);

                action(consumeResult);
            }
        }

        public async Task ConsumeAsync(Action<ConsumeResult<TKey, TMessage>> action)
        {
            await Task.Run(() =>
            {
                _consumerBuilder.Subscribe(_configuration.ConsumerTopicName);

                var cancellationTokenSource = new CancellationTokenSource();

                while (true & !cancellationTokenSource.IsCancellationRequested)
                {
                    var consumeResult = _consumerBuilder.Consume(cancellationTokenSource.Token);

                    action(consumeResult);
                }
            });
        }

        public void Consume(Action<ConsumeResult<TKey,TMessage>> action, int partitionNumber)
        {
            _consumerBuilder.Assign(new TopicPartition(_configuration.ConsumerTopicName, new Partition(partitionNumber)));

            var cancellationTokenSource = new CancellationTokenSource();

            while (true & !cancellationTokenSource.IsCancellationRequested)
            {
                var consumeResult = _consumerBuilder.Consume(cancellationTokenSource.Token);

                action(consumeResult);
            }
        }

        public async Task ConsumeAsync(Action<ConsumeResult<TKey, TMessage>> action, int partitionNumber)
        {
            await Task.Run(() =>
            {
                _consumerBuilder.Assign(new TopicPartition(_configuration.ConsumerTopicName, new Partition(partitionNumber)));

                var cancellationTokenSource = new CancellationTokenSource();

                while (true & !cancellationTokenSource.IsCancellationRequested)
                {
                    var consumeResult = _consumerBuilder.Consume(cancellationTokenSource.Token);

                    action(consumeResult);
                }
            });
        }

        public void Consume(Action<ConsumeResult<TKey, TMessage>> action, string topicName, int partitionNumber)
        {
            _consumerBuilder.Assign(new TopicPartition(topicName, new Partition(partitionNumber)));

            var cancellationTokenSource = new CancellationTokenSource();

            while (true & !cancellationTokenSource.IsCancellationRequested)
            {
                var consumeResult = _consumerBuilder.Consume(cancellationTokenSource.Token);

                action(consumeResult);
            }
        }

        public async Task ConsumeAsync(Action<ConsumeResult<TKey, TMessage>> action, string topicName, int partitionNumber)
        {
            await Task.Run(() =>
            {
                _consumerBuilder.Assign(new TopicPartition(topicName, new Partition(partitionNumber)));

                var cancellationTokenSource = new CancellationTokenSource();

                while (true & !cancellationTokenSource.IsCancellationRequested)
                {
                    var consumeResult = _consumerBuilder.Consume(cancellationTokenSource.Token);

                    action(consumeResult);
                }
            });
        }

        public void Consume(Action<ConsumeResult<TKey, TMessage>> action, string topicName)
        {
            _consumerBuilder.Subscribe(topicName);

            var cancellationTokenSource = new CancellationTokenSource();

            while (true & !cancellationTokenSource.IsCancellationRequested)
            {
                var consumeResult = _consumerBuilder.Consume(cancellationTokenSource.Token);

                action(consumeResult);
            }
        }

        public async Task ConsumeAsync(Action<ConsumeResult<TKey, TMessage>> action, string topicName)
        {
            await Task.Run(() =>
            {
                _consumerBuilder.Subscribe(topicName);

                var cancellationTokenSource = new CancellationTokenSource();

                while (true & !cancellationTokenSource.IsCancellationRequested)
                {
                    var consumeResult = _consumerBuilder.Consume(cancellationTokenSource.Token);

                    action(consumeResult);
                }
            });
        }
    }
}
