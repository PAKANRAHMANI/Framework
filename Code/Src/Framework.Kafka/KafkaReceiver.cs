using Confluent.Kafka;
using System;
using System.Text.Json;
using System.Threading;
using System.Collections.Concurrent;

namespace Framework.Kafka
{
    public class KafkaReceiver<T> : IMessageReceiver<T>, IDisposable
    {
        private readonly IConsumer<Ignore, string> _consumer;

        public KafkaReceiver(string bootstrapServers, string topic, string groupId, bool enableAutoOffsetStore = false, AutoOffsetReset autoOffsetReset = Confluent.Kafka.AutoOffsetReset.Latest)
        {

            var config = GetConsumerConfig(groupId, bootstrapServers, enableAutoOffsetStore, autoOffsetReset);
            var builder = new ConsumerBuilder<string, byte[]>(config);
            builder.SetErrorHandler(OnError);

            _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            _consumer.Subscribe(topic);
        }

        public void Dispose()
        {
            _consumer.Dispose();
        }

        public void Receive(Action<T, MetaData> action)
        {//TODO:exception handling
            CancellationTokenSource cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;

            try
            {
                StartReceiving(action, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Closing consumer.");
                _consumer.Close();
            }
        }

        private void StartReceiving(Action<T, MetaData> action, CancellationToken cancellationToken)
        {//TODO:exception handling
            while (true)
            {
                try
                {
                    var consumeResult = _consumer.Consume(cancellationToken);

                    if (consumeResult.IsPartitionEOF)
                    {
                        continue;
                    }

                    var message = JsonSerializer.Deserialize<Message<T>>(consumeResult.Message.Value);
                    action(message.Data, message.MetaData);
                }
                catch (ConsumeException e)
                {
                }
            }
        }
        internal static ConsumerConfig GetConsumerConfig(string groupId, string bootstrapServers, bool enableAutoOffsetStore, AutoOffsetReset autoOffsetReset) =>
            new ConsumerConfig
            {
                GroupId = groupId,
                BootstrapServers = bootstrapServers,
                EnableAutoOffsetStore = enableAutoOffsetStore,
                AutoOffsetReset = autoOffsetReset
            };

        private void OnError(IConsumer<string, byte[]> consumer, Error error)
        {
        }
    }
}
