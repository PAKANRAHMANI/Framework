using Confluent.Kafka;
using System;
using System.Threading;
using Newtonsoft.Json;

namespace Framework.Kafka
{
    public class KafkaReceiver : IKafkaReceiver
    {
        private readonly KafkaReceiverConfiguration _configuration;
        private readonly IConsumer<Ignore, string> _consumerBuilder;
        public KafkaReceiver(KafkaReceiverConfiguration configuration)
        {
            _configuration = configuration;
            var config = new ConsumerConfig
            {
                GroupId = configuration.GroupId,
                BootstrapServers = configuration.BootstrapServers,
                EnableAutoOffsetStore = configuration.EnableAutoOffsetStore,
                AutoOffsetReset = configuration.AutoOffsetReset
            };

            _consumerBuilder = new ConsumerBuilder<Ignore, string>(config).Build();
        }
        public void Receive<T>(Action<T> action,  int? partitionNumber)
        {
            if (partitionNumber != null)
            {
                _consumerBuilder.Assign(new TopicPartition(_configuration.TopicName, new Partition((int)partitionNumber)));
            }
            else
            {
                _consumerBuilder.Subscribe(_configuration.TopicName);
            }

            var cancellationTokenSource = new CancellationTokenSource();

            while (true & !cancellationTokenSource.IsCancellationRequested)
            {
                var consumeResult = _consumerBuilder.Consume(cancellationTokenSource.Token);

                var message = JsonConvert.DeserializeObject<T>(consumeResult.Message.Value);

                action(message);
            }
        }
    }
}
