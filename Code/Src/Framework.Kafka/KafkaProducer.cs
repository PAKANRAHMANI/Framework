using System;
using Confluent.Kafka;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;


namespace Framework.Kafka
{
    public class KafkaProducer<TKey, TMessage> : IKafkaProducer<TKey,TMessage>, IDisposable
    {
        private readonly KafkaConfiguration _configuration;
        private readonly IProducer<TKey, TMessage> _producer;
        public event EventHandler<ErrorEventArgs> Error;
        public KafkaProducer(KafkaConfiguration configuration)
        {
            _configuration = configuration;
            var config = new ProducerConfig()
            {
                BootstrapServers = configuration.BootstrapServers,
                MessageTimeoutMs = configuration.MessageTimeoutMs,
                Acks = Acks.All
            };

            var producerBuilder = new ProducerBuilder<string, byte[]>(config);

            producerBuilder.SetErrorHandler(OnError);

            _producer = new ProducerBuilder<TKey, TMessage>(config).Build();
        }


        public async Task<DeliveryResult<TKey, TMessage>> Send(TKey tKey,TMessage message, CancellationToken cancellationToken = default)
        {
            return  await _producer.ProduceAsync(_configuration.ProducerTopicName, new Message<TKey, TMessage>
            {
                Value = message,
                Key = tKey
            }, cancellationToken);
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