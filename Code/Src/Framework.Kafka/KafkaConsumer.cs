using Confluent.Kafka;
using Framework.Kafka.Configurations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Kafka
{
    public class KafkaConsumer<TKey, TMessage> : IKafkaConsumer<TKey, TMessage>
    {
        private readonly ConsumerConfiguration _configurations;
        private readonly IConsumer<TKey, TMessage> _consumer;
        public KafkaConsumer(ConsumerConfiguration configurations)
        {
            _configurations = configurations;
            var config = new ConsumerConfig
            {
                GroupId = configurations.GroupId,
                BootstrapServers = configurations.BootstrapServers,
                EnableAutoOffsetStore = configurations.EnableAutoOffsetStore,
                AutoOffsetReset = configurations.AutoOffsetReset,
                EnableAutoCommit = configurations.EnableAutoCommit,
                SaslUsername = configurations.SaslUserName,
                SaslPassword = configurations.SaslPassword,
                SecurityProtocol = configurations.SecurityProtocol,
                SaslMechanism = configurations.SaslMechanism
            };

            _consumer = new ConsumerBuilder<TKey, TMessage>(config).Build();
        }

        public void Consume(Action<ConsumeResult<TKey, TMessage>> action, CancellationToken cancellationToken)
        {
            _consumer.Subscribe(_configurations.TopicName);

            while (true & !cancellationToken.IsCancellationRequested)
            {
                ConsumeFromKafka(action, cancellationToken);
            }
        }


        public async Task ConsumeAsync(Func<ConsumeResult<TKey, TMessage>, Task> action, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                _consumer.Subscribe(_configurations.TopicName);

                while (true & !cancellationToken.IsCancellationRequested)
                {
                    ConsumeFromKafka(action, cancellationToken);
                }

            }, cancellationToken);
        }

        public void Consume(Action<ConsumeResult<TKey, TMessage>> action, int partitionNumber, CancellationToken cancellationToken)
        {
            _consumer.Assign(new TopicPartition(_configurations.TopicName, new Partition(partitionNumber)));

            while (true & !cancellationToken.IsCancellationRequested)
            {
                ConsumeFromKafka(action, cancellationToken);
            }
        }

        public async Task ConsumeAsync(Action<ConsumeResult<TKey, TMessage>> action, int partitionNumber, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                _consumer.Assign(new TopicPartition(_configurations.TopicName, new Partition(partitionNumber)));

                while (true & !cancellationToken.IsCancellationRequested)
                {
                    ConsumeFromKafka(action, cancellationToken);
                }
            }, cancellationToken);
        }

        public void Consume(Action<ConsumeResult<TKey, TMessage>> action, string topicName, int partitionNumber, CancellationToken cancellationToken)
        {
            _consumer.Assign(new TopicPartition(topicName, new Partition(partitionNumber)));

            while (true & !cancellationToken.IsCancellationRequested)
            {
                ConsumeFromKafka(action, cancellationToken);
            }
        }

        public async Task ConsumeAsync(Action<ConsumeResult<TKey, TMessage>> action, string topicName, int partitionNumber, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                _consumer.Assign(new TopicPartition(topicName, new Partition(partitionNumber)));

                while (true & !cancellationToken.IsCancellationRequested)
                {
                    ConsumeFromKafka(action, cancellationToken);
                }
            }, cancellationToken);
        }

        public void Consume(Action<ConsumeResult<TKey, TMessage>> action, string topicName, CancellationToken cancellationToken)
        {
            _consumer.Subscribe(topicName);

            while (true & !cancellationToken.IsCancellationRequested)
            {
                ConsumeFromKafka(action, cancellationToken);
            }
        }

        public async Task ConsumeAsync(Action<ConsumeResult<TKey, TMessage>> action, string topicName, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                _consumer.Subscribe(topicName);

                while (true & !cancellationToken.IsCancellationRequested)
                {
                    ConsumeFromKafka(action, cancellationToken);
                }
            }, cancellationToken);
        }

        public void Consume(Action<ConsumeResult<TKey, TMessage>> action, int partitionNumber, long offset, CancellationToken cancellationToken)
        {
            var partitionOffset = new TopicPartitionOffset(_configurations.TopicName, new Partition(partitionNumber), new Offset(offset));

            _consumer.Assign(partitionOffset);

            //_consumer.Seek(partitionOffset);

            while (true & !cancellationToken.IsCancellationRequested)
            {
                ConsumeFromKafka(action, cancellationToken);
            }
        }

        public async Task ConsumeAsync(Action<ConsumeResult<TKey, TMessage>> action, int partitionNumber, long offset, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                var partitionOffset = new TopicPartitionOffset(_configurations.TopicName, new Partition(partitionNumber), new Offset(offset));

                _consumer.Assign(partitionOffset);

                //_consumer.Seek(partitionOffset);

                while (true & !cancellationToken.IsCancellationRequested)
                {
                    ConsumeFromKafka(action, cancellationToken);
                }
            }, cancellationToken);
        }

        public void Consume(Action<ConsumeResult<TKey, TMessage>> action, string topic, int partitionNumber, Timestamp timestamp, TimeSpan timeout, CancellationToken cancellationToken)
        {
            var topicPartitionTimeStamp = new TopicPartitionTimestamp(topic, new Partition(partitionNumber), timestamp);

            var topicPartitionOffsets = _consumer.OffsetsForTimes(new List<TopicPartitionTimestamp>() { topicPartitionTimeStamp }, timeout);

            _consumer.Assign(topicPartitionOffsets);

            while (true & !cancellationToken.IsCancellationRequested)
            {
                ConsumeFromKafka(action, cancellationToken);
            }
        }

        public async Task ConsumeAsync(Action<ConsumeResult<TKey, TMessage>> action, string topic, int partitionNumber, Timestamp timestamp, TimeSpan timeout, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                var topicPartitionTimeStamp = new TopicPartitionTimestamp(topic, new Partition(partitionNumber), timestamp);

                var topicPartitionOffsets = _consumer.OffsetsForTimes(new List<TopicPartitionTimestamp>() { topicPartitionTimeStamp }, timeout);

                _consumer.Assign(topicPartitionOffsets);

                while (true & !cancellationToken.IsCancellationRequested)
                {

                    ConsumeFromKafka(action, cancellationToken);
                }
            }, cancellationToken);
        }

        public void Commit(params TopicPartitionOffset[] topicPartitionOffsets)
        {
            _consumer.Commit(topicPartitionOffsets);
        }

        public void Commit(ConsumeResult<TKey, TMessage> consumeResult)
        {
            _consumer.Commit(consumeResult);
        }

        private void ConsumeFromKafka(Action<ConsumeResult<TKey, TMessage>> action, CancellationToken cancellationToken)
        {
            try
            {
                var consumeResult = _consumer.Consume(cancellationToken);

                action(consumeResult);
            }
            catch (Exception)
            {
                //_consumer.Close();
            }
        }

        private async Task ConsumeFromKafka(Func<ConsumeResult<TKey, TMessage>, Task> func, CancellationToken cancellationToken)
        {
            try
            {
                var consumeResult = _consumer.Consume(cancellationToken);

                await func(consumeResult);
            }
            catch (Exception)
            {
                //_consumer.Close();
            }
        }
    }
}
