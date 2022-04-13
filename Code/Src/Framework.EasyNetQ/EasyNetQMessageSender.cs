using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.Topology;
using Framework.EasyNetQ.Helpers;
using Framework.Messages;
using MassTransit.RabbitMqTransport;
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

        private EasyNetQConfiguration Configure(string exchangeName, string exchangeType, Priority? priority)
        {
            var config = new EasyNetQConfiguration
            {
                Exchange = new Exchange(exchangeName, exchangeType),
                
            };
            if (priority != null)
            {
                config.MessageProperties = new MessageProperties()
                {
                    Priority = (byte) priority
                };
            }
            else
            {
                config.MessageProperties = new MessageProperties();
            }

            return config;
        }
        public async Task SendAsync(IMessage message, string exchangeName, string exchangeType, Priority priority)
        {
            var jsonBody = SetupMessage(message);

            var config = Configure(exchangeName, exchangeType, priority);

            await _bus.Advanced.PublishAsync(config.Exchange, string.Empty, false, config.MessageProperties, Encoding.UTF8.GetBytes(jsonBody));
        }

        public void Send(IMessage message, string exchangeName, string exchangeType, Priority priority)
        {
            var jsonBody = SetupMessage(message);

            var config = Configure(exchangeName, exchangeType, priority);

            _bus.Advanced.Publish(config.Exchange, string.Empty, false, config.MessageProperties, Encoding.UTF8.GetBytes(jsonBody));
        }

        public async Task SendAsync(IMessage message, string exchangeName, string exchangeType)
        {
            var jsonBody = SetupMessage(message);

            var config = Configure(exchangeName, exchangeType,null);

            await _bus.Advanced.PublishAsync(config.Exchange, string.Empty, false, messageProperties: config.MessageProperties, Encoding.UTF8.GetBytes(jsonBody));
        }

        public void Send(IMessage message, string queueName, string exchangeType)
        {
            var jsonBody = SetupMessage(message);

            _bus.Advanced.Publish(
                new Exchange(queueName),
                string.Empty,
                false,
                messageProperties: new MessageProperties { },
                Encoding.UTF8.GetBytes(jsonBody));
        }

        public async Task SendBatchAsync(IEnumerable<IMessage> messages,  string exchangeName, string exchangeType, Priority priority)
        {
            foreach (var message in messages)
            {
                await SendAsync(message,exchangeName,exchangeType, priority);
            }
        }

        public void SendBatch(IEnumerable<IMessage> messages, string exchangeName, string exchangeType, Priority priority)
        {
            foreach (var message in messages)
            {
                Send(message, exchangeName, exchangeType, priority);
            }
        }

        public async Task SendBatchAsync(IEnumerable<IMessage> messages, string exchangeName, string exchangeType)
        {
            foreach (var message in messages)
            {
                await SendAsync(message, exchangeName, exchangeType);
            }
        }

        public void SendBatch(IEnumerable<IMessage> messages, string exchangeName, string exchangeType)
        {
            foreach (var message in messages)
            {
                Send(message, exchangeName, exchangeType);
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