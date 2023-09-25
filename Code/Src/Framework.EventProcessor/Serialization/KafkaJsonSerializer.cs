using Confluent.Kafka;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;

namespace Framework.EventProcessor.Serialization
{
    public class KafkaJsonSerializer<TMessage> : ISerializer<TMessage> where TMessage : class
    {
        //TODO:remove Newtonsoft
        public byte[] Serialize(TMessage data, SerializationContext context)
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(data);

                return Encoding.UTF8.GetBytes(jsonString);
            }
            catch (Exception ex)
            {
                //TODO:Log with logger
                Console.WriteLine($"An error occurred: {ex.Message}");

                throw;
            }
        }
    }
}
