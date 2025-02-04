using Confluent.Kafka;
using Framework.Kafka.Configurations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            CancellationToken cancellationToken = default, string eventId = null)
        {
            return await _producer.ProduceAsync(new TopicPartition(topicName, new Partition(partitionNumber)), new Message<TKey, TMessage>
            {
                Value = message,
                Key = key,
                Headers = GetHeaders(eventId)
            }, cancellationToken);
        }

        public async Task<DeliveryResult<TKey, TMessage>> ProduceAsync(TKey key, TMessage message, CancellationToken cancellationToken = default, string eventId = default)
        {
            return await _producer.ProduceAsync(_configurations.TopicName, new Message<TKey, TMessage>
            {
                Value = message,
                Key = key,
                Headers = GetHeaders(eventId)
            }, cancellationToken);
        }

        public async Task<DeliveryResult<TKey, TMessage>> ProduceAsync(TKey key, TMessage message, KeyValuePair<string, string>[] headers, CancellationToken cancellationToken = default, string eventId = null)
        {
            var kafkaHeaders = new Headers();

            foreach (var header in headers)
            {
                kafkaHeaders.Add(new Header(header.Key, Encoding.UTF8.GetBytes(header.Value)));
            }

            kafkaHeaders.Add(GetHeader(eventId));

            return await _producer.ProduceAsync(_configurations.TopicName, new Message<TKey, TMessage>
            {
                Value = message,
                Key = key,
                Headers = kafkaHeaders
            }, cancellationToken);
        }

        public async Task<DeliveryResult<TKey, TMessage>> ProduceAsync(TKey key, TMessage message, int partitionNumber, CancellationToken cancellationToken = default, string eventId = default)
        {
            return await _producer.ProduceAsync(new TopicPartition(_configurations.TopicName, new Partition(partitionNumber)), new Message<TKey, TMessage>
            {
                Value = message,
                Key = key,
                Headers = GetHeaders(eventId)
            }, cancellationToken);
        }

        public void Produce(TKey key, TMessage message, Action<DeliveryResult<TKey, TMessage>> action, string eventId = null)
        {
            _producer.Produce(_configurations.TopicName, new Message<TKey, TMessage>
            {
                Value = message,
                Key = key,
                Headers = GetHeaders(eventId)
            }, action);
        }
         
        public void Produce(TKey key, TMessage message, Action<DeliveryResult<TKey, TMessage>> action, string eventId = null, params KeyValuePair<string, string>[] headers)
        {

            var kafkaHeaders = new Headers();

            foreach (var header in headers)
            {
                kafkaHeaders.Add(new Header(header.Key, Encoding.UTF8.GetBytes(header.Value)));
            }

            kafkaHeaders.Add(GetHeader(eventId));

            _producer.Produce(_configurations.TopicName, new Message<TKey, TMessage>
            {
                Value = message,
                Key = key,
                Headers = kafkaHeaders
            }, action);
        }

        public void Produce(TKey key, TMessage message, int partitionNumber, Action<DeliveryResult<TKey, TMessage>> action, string eventId = null)
        {
            _producer.Produce(new TopicPartition(_configurations.TopicName, new Partition(partitionNumber)), new Message<TKey, TMessage>
            {
                Value = message,
                Key = key,
                Headers = GetHeaders(eventId)
            }, action);
        }

        public void Produce(TKey key, TMessage message, string topicName, int partitionNumber,
            Action<DeliveryResult<TKey, TMessage>> action = null, string eventId = null)
        {
            _producer.Produce(new TopicPartition(topicName, new Partition(partitionNumber)), new Message<TKey, TMessage>
            {
                Value = message,
                Key = key,
                Headers = GetHeaders(eventId)
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

        public void Produce(TKey key, TMessage message, string topicName, Action<DeliveryResult<TKey, TMessage>> action = null, string eventId = null)
        {
            _producer.Produce(topicName, new Message<TKey, TMessage>
            {
                Value = message,
                Key = key,
                Headers = GetHeaders(eventId)
            }, action);
        }
        private Headers GetHeaders(string eventId = null)
        {
            return [GetHeader(eventId)];
        }
        private Header GetHeader(string eventId = null)
        {
            var kafkaHeaderKey = "eventid";

            return string.IsNullOrEmpty(eventId)
                ? new Header(kafkaHeaderKey, Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()))
                : new Header(kafkaHeaderKey, Encoding.UTF8.GetBytes(eventId));
        }
    }
}