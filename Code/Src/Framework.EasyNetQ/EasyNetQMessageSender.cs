using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.Topology;
using Framework.EasyNetQ.Helpers;
using Framework.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using IMessage = Framework.Messages.IMessage;

namespace Framework.EasyNetQ
{
    public class EasyNetQMessageSender : IMessageSender
    {
        private readonly IBus _bus;
        public EasyNetQMessageSender(IBus bus)
        {
            this._bus = bus;
        }

        public async Task Send(IMessage message, string queueName, Priority priority)
        {
            var jsonBody = SetupMessage(message);

            await _bus.Advanced.PublishAsync(
                new Exchange(queueName),
                "",
                false,
               messageProperties: new MessageProperties
               {
                   Priority = (byte)priority,
               },
                Encoding.UTF8.GetBytes(jsonBody)
            );
        }

        public async Task Send(IMessage message, string queueName)
        {
            var jsonBody = SetupMessage(message);

            await _bus.Advanced.PublishAsync(
                new Exchange(queueName),
                "",
                false,
                messageProperties: new MessageProperties
                {

                },
                Encoding.UTF8.GetBytes(jsonBody)
            );
        }

        private static string SetupMessage(IMessage message)
        {
            var messageContext = EasyNetQHelpers.CreateMessageContext(
                Guid.NewGuid().ToString(),
                messageTypes: BaseEventTypes.TypeNames,
                 message: message);

            var jsonSerializerSettings = EasyNetQHelpers.CreateJsonSerializerSettings(
               formatting: Formatting.Indented,
               contractResolver: new CamelCasePropertyNamesContractResolver());

            var jsonBody = JsonConvert.SerializeObject(messageContext, jsonSerializerSettings);

            return jsonBody;
        }

        public async Task SendBatch(IEnumerable<IMessage> messages, string queueName, Priority priority)
        {
            foreach (var message in messages)
            {
                await this.Send(message, queueName, priority);
            }
        }
    }
}