using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Core.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Framework.EventProcessor.Serialization
{
    public static class EventDeserializer
    {
        private static readonly JsonSerializerSettings Settings;
        static EventDeserializer()
        {
            Settings = new JsonSerializerSettings
            {
                ContractResolver = new PrivateSetterContractResolver(),
            };
        }
        public static IEvent Deserialize(Type type, string body)
        {
            return JsonConvert.DeserializeObject(body, type, Settings) as IEvent;
        }
    }
}
