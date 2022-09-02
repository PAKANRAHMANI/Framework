using System.Collections.Generic;
using Framework.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Framework.EasyNetQ.Helpers
{
    internal static class EasyNetQHelpers
    {
        internal static MessageContext CreateMessageContext(string correlationId, 
            IReadOnlyCollection<string> messageTypes, IMessage message)
        {
            return new MessageContext
            {
                CorrelationId = correlationId,
                MessageType = messageTypes,
                Message = message
            };
        }

        internal static JsonSerializerSettings CreateJsonSerializerSettings(Formatting formatting, IContractResolver contractResolver)
        {
            return new JsonSerializerSettings
            {
                Formatting = formatting,
                ContractResolver = contractResolver
            };
        }
    }
}
