using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using Framework.Core.Logging;

namespace Framework.EventProcessor.Serialization;

public class KafkaJsonDeserializer<TMessage> : IDeserializer<TMessage> where TMessage : class
{
    private readonly ILogger _logger;

    public KafkaJsonDeserializer(ILogger logger)
    {
        _logger = logger;
    }
    public TMessage Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        try
        {
            var jsonString = Encoding.UTF8.GetString(data);

            return JsonSerializer.Deserialize<TMessage>(jsonString);
        }
        catch (Exception ex)
        {
            _logger.WriteException(ex);
            return null;
        }
    }
}