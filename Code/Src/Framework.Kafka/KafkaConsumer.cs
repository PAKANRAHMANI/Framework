using Confluent.Kafka;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Kafka
{
    public class KafkaConsumer<TKey, TMessage> : IKafkaConsumer<TKey, TMessage>
    {
        private readonly KafkaConfiguration _configuration;
        private readonly IConsumer<TKey, TMessage> _consumer;
        public KafkaConsumer(KafkaConfiguration configuration)
        {
            _configuration = configuration;
            var config = new ConsumerConfig
            {
                GroupId = configuration.GroupId,
                BootstrapServers = configuration.ConsumerBootstrapServers,
                EnableAutoOffsetStore = configuration.EnableAutoOffsetStore,
                AutoOffsetReset = configuration.AutoOffsetReset,
                EnableAutoCommit = configuration.EnableAutoCommit
            };

            _consumer = new ConsumerBuilder<TKey, TMessage>(config).Build();
        }

        public void Consume(Action<ConsumeResult<TKey, TMessage>> action,CancellationToken cancellationToken)
        {
            _consumer.Subscribe(_configuration.ConsumerTopicName);

            while (true & !cancellationToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(cancellationToken);

                action(consumeResult);
            }
        }

        public async Task ConsumeAsync(Action<ConsumeResult<TKey, TMessage>> action, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                _consumer.Subscribe(_configuration.ConsumerTopicName);

                while (true & !cancellationToken.IsCancellationRequested)
                {
                    var consumeResult = _consumer.Consume(cancellationToken);

                    action(consumeResult);
                }
            }, cancellationToken);
        }

        public void Consume(Action<ConsumeResult<TKey, TMessage>> action, int partitionNumber,CancellationToken cancellationToken)
        {
            _consumer.Assign(new TopicPartition(_configuration.ConsumerTopicName, new Partition(partitionNumber)));

            while (true & !cancellationToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(cancellationToken);

                action(consumeResult);
            }
        }

        public async Task ConsumeAsync(Action<ConsumeResult<TKey, TMessage>> action, int partitionNumber, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                _consumer.Assign(new TopicPartition(_configuration.ConsumerTopicName, new Partition(partitionNumber)));

                while (true & !cancellationToken.IsCancellationRequested)
                {
                    var consumeResult = _consumer.Consume(cancellationToken);

                    action(consumeResult);
                }
            }, cancellationToken);
        }

        public void Consume(Action<ConsumeResult<TKey, TMessage>> action, string topicName, int partitionNumber, CancellationToken cancellationToken)
        {
            _consumer.Assign(new TopicPartition(topicName, new Partition(partitionNumber)));

            while (true & !cancellationToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(cancellationToken);

                action(consumeResult);
            }
        }

        public async Task ConsumeAsync(Action<ConsumeResult<TKey, TMessage>> action, string topicName, int partitionNumber, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                _consumer.Assign(new TopicPartition(topicName, new Partition(partitionNumber)));

                while (true & !cancellationToken.IsCancellationRequested)
                {
                    var consumeResult = _consumer.Consume(cancellationToken);

                    action(consumeResult);
                }
            }, cancellationToken);
        }

        public void Consume(Action<ConsumeResult<TKey, TMessage>> action, string topicName, CancellationToken cancellationToken)
        {
            _consumer.Subscribe(topicName);

            while (true & !cancellationToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(cancellationToken);

                action(consumeResult);
            }
        }

        public async Task ConsumeAsync(Action<ConsumeResult<TKey, TMessage>> action, string topicName, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                _consumer.Subscribe(topicName);

                while (true & !cancellationToken.IsCancellationRequested)
                {
                    var consumeResult = _consumer.Consume(cancellationToken);

                    action(consumeResult);
                }
            }, cancellationToken);
        }

        public void Consume(Action<ConsumeResult<TKey, TMessage>> action, int partitionNumber, long offset, CancellationToken cancellationToken)
        {
            var partitionOffset = new TopicPartitionOffset(_configuration.ConsumerTopicName, new Partition(partitionNumber), new Offset(offset));

            _consumer.Assign(partitionOffset);

            //_consumer.Seek(partitionOffset);

            while (true & !cancellationToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(cancellationToken);

                action(consumeResult);
            }
        }


        public void Commit(params TopicPartitionOffset[] topicPartitionOffsets)
        {
            _consumer.Commit(topicPartitionOffsets);
        }

        public void Commit(ConsumeResult<TKey, TMessage> consumeResult)
        {
            _consumer.Commit(consumeResult);
        }

        
    }
}
