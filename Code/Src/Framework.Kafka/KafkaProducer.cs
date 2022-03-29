using System;
using Confluent.Kafka;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;


namespace Framework.Kafka
{
    public class KafkaProducer<TKey, TMessage> : IKafkaProducer<TKey, TMessage>, IDisposable
    {
        private readonly KafkaConfiguration _configuration;
        private readonly IProducer<TKey, TMessage> _producer;
        public event EventHandler<ErrorEventArgs> Error;
        public KafkaProducer(KafkaConfiguration configuration)
        {
            _configuration = configuration;
            var config = new ProducerConfig()
            {
                BootstrapServers = configuration.ProducerBootstrapServers,
                MessageTimeoutMs = configuration.MessageTimeoutMs,
                Acks = Acks.All
            };

            var producerBuilder = new ProducerBuilder<string, byte[]>(config);

            producerBuilder.SetErrorHandler(OnError);

            _producer = new ProducerBuilder<TKey, TMessage>(config).Build();
        }


        public async Task<DeliveryResult<TKey, TMessage>> ProduceAsync(TKey key, TMessage message, CancellationToken cancellationToken = default)
        {
            return await _producer.ProduceAsync(_configuration.ProducerTopicName, new Message<TKey, TMessage>
            {
                Value = message,
                Key = key
            }, cancellationToken);
        }

        public void Produce(TKey key, TMessage message, Action<DeliveryResult<TKey, TMessage>> action)
        {
            _producer.Produce(_configuration.ProducerTopicName, new Message<TKey, TMessage>
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
            _producer.Flush(TimeSpan.FromSeconds(10));
            _producer.Dispose();
        }



    }
}