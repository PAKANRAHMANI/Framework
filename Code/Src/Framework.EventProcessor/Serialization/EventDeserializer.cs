using Framework.Core.Events;
using Framework.Core.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Framework.EventProcessor.Serialization
{
    internal static class EventDeserializer
    {
        private static readonly JsonSerializerSettings Settings;
        static EventDeserializer()
        {
            Settings = new JsonSerializerSettings
            {
                ContractResolver = new PrivateSetterContractResolver(),
            };
        }
        public static IEvent Deserialize(Type type, string body, ILogger logger)
        {
            try
            {
                return JsonConvert.DeserializeObject(body, type, Settings) as IEvent;
            }
            catch (Exception e)
            {
                logger.WriteException(e);
                return null;
            }
        }
    }
}
