﻿using Confluent.Kafka;
using Framework.Kafka.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
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
                SaslMechanism = configurations.SaslMechanism,
                IsolationLevel = configurations.IsolationLevel,
                AutoCommitIntervalMs = configurations.AutoCommitIntervalMs,
                HeartbeatIntervalMs = configurations.HeartbeatIntervalMs,
                FetchErrorBackoffMs = configurations.FetchErrorBackoffMs,
                FetchWaitMaxMs = configurations.FetchWaitMaxMs,
                SessionTimeoutMs = configurations.SessionTimeoutMs
            };

            _consumer = new ConsumerBuilder<TKey, TMessage>(config).Build();
        }

        public void Consume(Action<ConsumeResult<TKey, TMessage>> action, CancellationToken cancellationToken)
        {
            ConfigKafkaConsumer();

            while (true & !cancellationToken.IsCancellationRequested)
            {
                ConsumeFromKafka(action, cancellationToken);
            }
        }

        public async Task ConsumeAsync(Func<ConsumeResult<TKey, TMessage>, Task> action, CancellationToken cancellationToken)
        {
            await Task.Run(async () =>
            {
                ConfigKafkaConsumer();

                while (true & !cancellationToken.IsCancellationRequested)
                {
                    await ConsumeFromKafkaAsync(action, cancellationToken);
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

        public void DoSeek(ConsumeResult<TKey, TMessage> consumeResult)
        {
            var newOffset = new TopicPartitionOffset(consumeResult.Topic, consumeResult.Partition,
                consumeResult.Offset);

            _consumer.Seek(newOffset);
        }

        private void ConfigKafkaConsumer()
        {
            if (_configurations.ReadTodayData)
            {

                var topicPartitionTimestamps =
                    _configurations
                        .PartitionsOffsets
                        .Select(partitionsOffset => new TopicPartitionTimestamp(_configurations.TopicName,
                            partitionsOffset.PartitionNumber, new Timestamp(DateTime.Now.AddHours(_configurations.Hour)))).ToList();

                var topicPartitionOffsets = _consumer.OffsetsForTimes(topicPartitionTimestamps, TimeSpan.FromSeconds(10));

                _consumer.Assign(topicPartitionOffsets);
            }
            else
            {
                if (_configurations.ReadFromOffset)
                {
                    foreach (var partitionsOffset in _configurations.PartitionsOffsets)
                    {
                        _consumer.Assign(new TopicPartitionOffset(new TopicPartition(_configurations.TopicName, partitionsOffset.PartitionNumber), new Offset(partitionsOffset.Offset)));
                    }
                }
                else
                    _consumer.Subscribe(_configurations.TopicName);
            }
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

        private async Task ConsumeFromKafkaAsync(Func<ConsumeResult<TKey, TMessage>, Task> func, CancellationToken cancellationToken)
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
