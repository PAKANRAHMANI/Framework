using System;
using Confluent.Kafka;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;

namespace Framework.Kafka
{
    public class KafkaSender<T> : IMessageSender<T>, IDisposable
    {
        private readonly string _topic;
        private readonly IProducer<Null, string> _producer;
        private readonly int _messageTimeoutMs;

        public KafkaSender(string bootstrapServers, string topic, int messageTimeoutMs = 10000)
        {
            _topic = topic;
            _messageTimeoutMs = messageTimeoutMs;

            var config = GetProducerConfig(bootstrapServers, messageTimeoutMs);

            var producerBuilder = new ProducerBuilder<string, byte[]>(config);

            producerBuilder.SetErrorHandler(OnError);

            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public void Dispose()
        {
            _producer.Flush(TimeSpan.FromSeconds(10));
            _producer.Dispose();
        }

        public async Task SendAsync(T message, MetaData metaData, CancellationToken cancellationToken = default)
        {  
            _ = await _producer.ProduceAsync(_topic, new Message<Null, string>
            {
                Value = JsonConvert.SerializeObject(new Message<T>
                {
                    Data = message,
                    MetaData = metaData,
                }),
            }, cancellationToken);
        }

        internal static ProducerConfig GetProducerConfig(string bootstrapServers, int messageTimeoutMs)
        {
            return new ProducerConfig()
            {
                BootstrapServers = bootstrapServers,
                MessageTimeoutMs = messageTimeoutMs
            };
        }

        private void OnError(IProducer<string, byte[]> producer, Error error)
        {
            Error?.Invoke(this, new ErrorEventArgs(new KafkaException(error)));
        }

        public event EventHandler<ErrorEventArgs> Error;
    }
}