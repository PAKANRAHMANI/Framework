using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.Messages;
using MassTransit;
using Microsoft.Extensions.Options;

namespace Framework.MassTransit
{
    public class MassTransitMessageSender : IMessageSender
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly MassTransitConfiguration _senderConfiguration;

        public MassTransitMessageSender(ISendEndpointProvider sendEndpointProvider, MassTransitConfiguration senderConfiguration)
        {
            _sendEndpointProvider = sendEndpointProvider;
            _senderConfiguration = senderConfiguration;
        }
        public async Task SendAsync(IMessage message, string exchangeName, Priority priority)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(GenerateUriAddress(exchangeName));

            await endpoint.Send(message, context => context.SetPriority((byte)priority));
        }

        public void Send(IMessage message, string exchangeName, Priority priority)
        {
            var endpoint = _sendEndpointProvider.GetSendEndpoint(GenerateUriAddress(exchangeName)).Result;

            endpoint.Send(message, context => context.SetPriority((byte)priority)).GetAwaiter().GetResult();
        }
        public async Task SendAsync(IMessage message, string exchangeName)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(GenerateUriAddress(exchangeName));

            await endpoint.Send(message);
        }

        private Uri GenerateUriAddress(string exchangeName)
        {
            return new Uri($"exchange:{exchangeName}?type={_senderConfiguration.ProducerExchangeType}");
        }

        public void Send(IMessage message, string exchangeName)
        {
            var endpoint = _sendEndpointProvider.GetSendEndpoint(GenerateUriAddress(exchangeName)).Result;

            endpoint.Send(message).GetAwaiter().GetResult();
        }

        public async Task SendBatchAsync(IEnumerable<IMessage> messages, string exchangeName, Priority priority)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(GenerateUriAddress(exchangeName));

            foreach (var message in messages)
                await endpoint.Send(message, context => context.SetPriority((byte)priority));
        }

        public void SendBatch(IEnumerable<IMessage> messages, string exchangeName, Priority priority)
        {
            var endpoint = _sendEndpointProvider.GetSendEndpoint(GenerateUriAddress(exchangeName)).Result;

            foreach (var message in messages)
                endpoint.Send(message, context => context.SetPriority((byte)priority)).GetAwaiter().GetResult();
        }

        public async Task SendBatchAsync(IEnumerable<IMessage> messages, string exchangeName)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(GenerateUriAddress(exchangeName));

            await endpoint.SendBatch(messages);
        }

        public void SendBatch(IEnumerable<IMessage> messages, string exchangeName)
        {
            var endpoint = _sendEndpointProvider.GetSendEndpoint(GenerateUriAddress(exchangeName)).Result;

            endpoint.SendBatch(messages).GetAwaiter().GetResult();
        }
    }
}
