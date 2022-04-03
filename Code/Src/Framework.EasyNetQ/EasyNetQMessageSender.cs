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
            _bus = bus;
        }
        
        public async Task SendAsync(IMessage message, string queueName, Priority priority)
        {
            //TODO:Refactor use TemplateMethod

            var jsonBody = SetupMessage(message);

            await _bus.Advanced.PublishAsync(
                new Exchange(queueName),
                string.Empty,
                false,
               messageProperties: new MessageProperties
               {
                   Priority = (byte)priority,
               },
                Encoding.UTF8.GetBytes(jsonBody)
            );
        }

        public void Send(IMessage message, string queueName, Priority priority)
        {
            //TODO:Refactor use TemplateMethod

            var jsonBody = SetupMessage(message);

             _bus.Advanced.Publish(
                new Exchange(queueName),
                string.Empty,
                false,
                messageProperties: new MessageProperties
                {
                    Priority = (byte)priority,
                },
                Encoding.UTF8.GetBytes(jsonBody)
            );
        }

        public async Task SendAsync(IMessage message, string queueName)
        {
            var jsonBody = SetupMessage(message);

            await _bus.Advanced.PublishAsync(
                new Exchange(queueName),
                string.Empty,
                false,
                messageProperties: new MessageProperties { },
                Encoding.UTF8.GetBytes(jsonBody)
            );
        }

        public void Send(IMessage message, string queueName)
        {
            var jsonBody = SetupMessage(message);

            _bus.Advanced.Publish(
                new Exchange(queueName),
                string.Empty,
                false,
                messageProperties: new MessageProperties { },
                Encoding.UTF8.GetBytes(jsonBody));
        }

        public async Task SendBatchAsync(IEnumerable<IMessage> messages, string queueName, Priority priority)
        {
            foreach (var message in messages)
            {
                await SendAsync(message, queueName, priority);
            }
        }

        public void SendBatch(IEnumerable<IMessage> messages, string queueName, Priority priority)
        {
            foreach (var message in messages)
            {
                 Send(message, queueName, priority);
            }
        }

        public async Task SendBatchAsync(IEnumerable<IMessage> messages, string queueName)
        {
            foreach (var message in messages)
            {
                await SendAsync(message, queueName);
            }
        }

        public void SendBatch(IEnumerable<IMessage> messages, string queueName)
        {
            foreach (var message in messages)
            {
                 Send(message, queueName);
            }
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
    }
}