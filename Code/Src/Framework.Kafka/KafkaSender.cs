using System;
using Confluent.Kafka;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;


namespace Framework.Kafka
{
    public class KafkaSender : IKafkaSender, IDisposable
    {
        private readonly KafkaSenderConfiguration _configuration;
        private readonly IProducer<Null, string> _producer;
        public event EventHandler<ErrorEventArgs> Error;
        public KafkaSender(KafkaSenderConfiguration configuration)
        {
            _configuration = configuration;
            var config = new ProducerConfig()
            {
                BootstrapServers = configuration.BootstrapServers,
                MessageTimeoutMs = configuration.MessageTimeoutMs
            };

            var producerBuilder = new ProducerBuilder<string, byte[]>(config);

            producerBuilder.SetErrorHandler(OnError);

            _producer = new ProducerBuilder<Null, string>(config).Build();
        }


        public async Task Send<T>(T message, CancellationToken cancellationToken = default)
        {
            await _producer.ProduceAsync(_configuration.TopicName, new Message<Null, string>
            {
                Value = JsonConvert.SerializeObject(message),
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