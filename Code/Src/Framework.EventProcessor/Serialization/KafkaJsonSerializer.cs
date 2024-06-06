using Confluent.Kafka;
using System.Text;
using Framework.Core.Logging;
using Newtonsoft.Json;

namespace Framework.EventProcessor.Serialization
{
    public sealed class KafkaJsonSerializer<TMessage>(ILogger logger) : ISerializer<TMessage> where TMessage : class
    {
        public byte[] Serialize(TMessage data, SerializationContext context)
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(data);

                return Encoding.UTF8.GetBytes(jsonString);
            }
            catch (Exception ex)
            {
                logger.WriteException(ex);
                return null;
            }
        }
    }
}
