using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using Framework.Core.Events;

namespace Framework.EventProcessor.Serialization;

public class KafkaJsonDeserializer<TMessage> : IDeserializer<TMessage> where TMessage : class
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
            //TODO:Log with logger
            Console.WriteLine($"An error occurred: {ex.Message}");

            throw;
        }
    }
}