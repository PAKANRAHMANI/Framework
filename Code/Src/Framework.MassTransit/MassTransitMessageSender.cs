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

        public MassTransitMessageSender(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }
        public async Task Send(IMessage message, string queueName, Priority priority)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{queueName}"));

            await endpoint.Send(message, context => context.SetPriority((byte)priority));
        }

        public async Task Send(IMessage message, string queueName)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{queueName}"));

            await endpoint.Send(message);
        }

        public async Task SendBatch(IEnumerable<IMessage> messages, string queueName, Priority priority)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{queueName}"));

            foreach (var message in messages)
                await endpoint.Send(message, context => context.SetPriority((byte)priority));
        }

        public async Task SendBatch(IEnumerable<IMessage> messages, string queueName)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{queueName}"));

            await endpoint.SendBatch(messages);
        }
    }
}
