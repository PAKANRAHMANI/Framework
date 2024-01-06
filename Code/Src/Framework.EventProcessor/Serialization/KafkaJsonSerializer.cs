using Confluent.Kafka;
using System.Text;
using Framework.Core.Logging;
using Newtonsoft.Json;

namespace Framework.EventProcessor.Serialization
{
    public class KafkaJsonSerializer<TMessage> : ISerializer<TMessage> where TMessage : class
    {
        private readonly ILogger _logger;

        public KafkaJsonSerializer(ILogger logger)
        {
            _logger = logger;
        }
        public byte[] Serialize(TMessage data, SerializationContext context)
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(data);

                return Encoding.UTF8.GetBytes(jsonString);
            }
            catch (Exception ex)
            {
                _logger.WriteException(ex);
                return null;
            }
        }
    }
}
