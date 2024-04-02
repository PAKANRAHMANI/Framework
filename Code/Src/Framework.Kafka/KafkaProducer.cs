using System;
using System.Collections.Generic;
using Confluent.Kafka;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Text;
using Framework.Kafka.Configurations;

namespace Framework.Kafka
{
    public class KafkaProducer<TKey, TMessage> : IKafkaProducer<TKey, TMessage>, IDisposable
    {
        private readonly ProducerConfiguration _configurations;
        private readonly IProducer<TKey, TMessage> _producer;
        public event EventHandler<ErrorEventArgs> Error;
        public KafkaProducer(ProducerConfiguration configurations)
        {
            _configurations = configurations;
            var config = new ProducerConfig()
            {
                BootstrapServers = configurations.BootstrapServers,
                MessageTimeoutMs = configurations.MessageTimeoutMs,
                Acks = Acks.All,
                SaslUsername = configurations.SaslUserName,
                SaslPassword = configurations.SaslPassword,
                SecurityProtocol = configurations.SecurityProtocol,
                SaslMechanism = configurations.SaslMechanism
            };

            var producerBuilder = new ProducerBuilder<string, byte[]>(config);

            producerBuilder.SetErrorHandler(OnError);

            _producer = new ProducerBuilder<TKey, TMessage>(config).Build();
        }


        public async Task<DeliveryResult<TKey, TMessage>> ProduceAsync(TKey key, TMessage message, string topicName, int partitionNumber,
            CancellationToken cancellationToken = default)
        {
            return await _producer.ProduceAsync(new TopicPartition(topicName, new Partition(partitionNumber)), new Message<TKey, TMessage>
            {
                Value = message,
                Key = key
            }, cancellationToken);
        }

        public async Task<DeliveryResult<TKey, TMessage>> ProduceAsync(TKey key, TMessage message, CancellationToken cancellationToken = default)
        {
            return await _producer.ProduceAsync(_configurations.TopicName, new Message<TKey, TMessage>
            {
                Value = message,
                Key = key
            }, cancellationToken);
        }

        public async Task<DeliveryResult<TKey, TMessage>> ProduceAsync(TKey key, TMessage message, KeyValuePair<string, string>[] headers, CancellationToken cancellationToken = default)
        {
            var kafkaHeaders = new Headers();

            foreach (var header in headers)
            {
                kafkaHeaders.Add(new Header(header.Key, Encoding.UTF8.GetBytes(header.Value)));
            }

            return await _producer.ProduceAsync(_configurations.TopicName, new Message<TKey, TMessage>
            {
                Value = message,
                Key = key,
                Headers = kafkaHeaders
            }, cancellationToken);
        }

        public async Task<DeliveryResult<TKey, TMessage>> ProduceAsync(TKey key, TMessage message, int partitionNumber, CancellationToken cancellationToken = default)
        {
            return await _producer.ProduceAsync(new TopicPartition(_configurations.TopicName, new Partition(partitionNumber)), new Message<TKey, TMessage>
            {
                Value = message,
                Key = key
            }, cancellationToken);
        }

        public void Produce(TKey key, TMessage message, Action<DeliveryResult<TKey, TMessage>> action)
        {
            _producer.Produce(_configurations.TopicName, new Message<TKey, TMessage>
            {
                Value = message,
                Key = key
            }, action);
        }
        public void Produce(TKey key, TMessage message,int partitionNumber, Action<DeliveryResult<TKey, TMessage>> action)
        {
            _producer.Produce(new TopicPartition(_configurations.TopicName,new Partition(partitionNumber)), new Message<TKey, TMessage>
            {
                Value = message,
                Key = key
            }, action);
        }

        public void Produce(TKey key, TMessage message, string topicName, int partitionNumber,
            Action<DeliveryResult<TKey, TMessage>> action = null)
        {
            _producer.Produce(new TopicPartition(topicName, new Partition(partitionNumber)), new Message<TKey, TMessage>
            {
                Value = message,
                Key = key
            }, action);
        }

        private void OnError(IProducer<string, byte[]> producer, Error error)
        {
            Error?.Invoke(this, new ErrorEventArgs(new KafkaException(error)));
        }
        public void Dispose()
        {
            _producer.Flush(TimeSpan.FromSeconds(2));
            _producer.Dispose();
        }

        public void Produce(TKey key, TMessage message, string topicName, Action<DeliveryResult<TKey, TMessage>> action = null)
        {
            _producer.Produce(topicName, new Message<TKey, TMessage>
            {
                Value = message,
                Key = key
            }, action);
        }
    }
}