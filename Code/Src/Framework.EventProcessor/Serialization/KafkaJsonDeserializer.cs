using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using Framework.Core.Logging;

namespace Framework.EventProcessor.Serialization;

public sealed class KafkaJsonDeserializer<TMessage>(ILogger logger) : IDeserializer<TMessage> where TMessage : class
{
    public TMessage Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        try
        {
            var jsonString = Encoding.UTF8.GetString(data);

            return JsonSerializer.Deserialize<TMessage>(jsonString);
        }
        catch (Exception ex)
        {
            logger.WriteException(ex);
            return null;
        }
    }
}