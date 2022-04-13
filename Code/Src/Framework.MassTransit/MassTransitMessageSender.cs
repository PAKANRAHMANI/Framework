using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.Messages;
using MassTransit;

namespace Framework.MassTransit
{
    public class MassTransitMessageSender : IMessageSender
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public MassTransitMessageSender(ISendEndpointProvider sendEndpointProvider, MassTransitConfiguration senderConfiguration)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }
        public async Task SendAsync(IMessage message, string exchangeName, string exchangeType, Priority priority)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(GenerateUriAddress(exchangeName, exchangeType));

            await endpoint.Send(message, context => context.SetPriority((byte)priority));
        }

        public void Send(IMessage message, string exchangeName, string exchangeType, Priority priority)
        {
            var endpoint = _sendEndpointProvider.GetSendEndpoint(GenerateUriAddress(exchangeName, exchangeType)).Result;

            endpoint.Send(message, context => context.SetPriority((byte)priority)).GetAwaiter().GetResult();
        }
        public async Task SendAsync(IMessage message, string exchangeName, string exchangeType)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(GenerateUriAddress(exchangeName, exchangeType));

            await endpoint.Send(message);
        }

        private Uri GenerateUriAddress(string exchangeName, string exchangeType)
        {
            return new Uri($"exchange:{exchangeName}?type={exchangeType}");
        }

        public void Send(IMessage message, string exchangeName, string exchangeType)
        {
            var endpoint = _sendEndpointProvider.GetSendEndpoint(GenerateUriAddress(exchangeName, exchangeType)).Result;

            endpoint.Send(message).GetAwaiter().GetResult();
        }

        public async Task SendBatchAsync(IEnumerable<IMessage> messages, string exchangeName, string exchangeType, Priority priority)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(GenerateUriAddress(exchangeName, exchangeType));

            foreach (var message in messages)
                await endpoint.Send(message, context => context.SetPriority((byte)priority));
        }

        public void SendBatch(IEnumerable<IMessage> messages, string exchangeName, string exchangeType, Priority priority)
        {
            var endpoint = _sendEndpointProvider.GetSendEndpoint(GenerateUriAddress(exchangeName, exchangeType)).Result;

            foreach (var message in messages)
                endpoint.Send(message, context => context.SetPriority((byte)priority)).GetAwaiter().GetResult();
        }

        public async Task SendBatchAsync(IEnumerable<IMessage> messages, string exchangeName, string exchangeType)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(GenerateUriAddress(exchangeName, exchangeType));

            await endpoint.SendBatch(messages);
        }

        public void SendBatch(IEnumerable<IMessage> messages, string exchangeName, string exchangeType)
        {
            var endpoint = _sendEndpointProvider.GetSendEndpoint(GenerateUriAddress(exchangeName, exchangeType)).Result;

            endpoint.SendBatch(messages).GetAwaiter().GetResult();
        }
    }
}
