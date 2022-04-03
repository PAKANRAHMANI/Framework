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
        public async Task SendAsync(IMessage message, string queueName, Priority priority)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{queueName}"));

            await endpoint.Send(message, context => context.SetPriority((byte)priority));
        }

        public void Send(IMessage message, string queueName, Priority priority)
        {
            var endpoint =  _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{queueName}")).Result;

             endpoint.Send(message, context => context.SetPriority((byte)priority)).Wait();
        }

        public async Task SendAsync(IMessage message, string queueName)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{queueName}"));

            await endpoint.Send(message);
        }

        public void Send(IMessage message, string queueName)
        {
            var endpoint =  _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{queueName}")).Result;

             endpoint.Send(message).Wait();
        }

        public async Task SendBatchAsync(IEnumerable<IMessage> messages, string queueName, Priority priority)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{queueName}"));

            foreach (var message in messages)
                await endpoint.Send(message, context => context.SetPriority((byte)priority));
        }

        public void SendBatch(IEnumerable<IMessage> messages, string queueName, Priority priority)
        {
            var endpoint =  _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{queueName}")).Result;

            foreach (var message in messages)
                 endpoint.Send(message, context => context.SetPriority((byte)priority)).Wait();
        }

        public async Task SendBatchAsync(IEnumerable<IMessage> messages, string queueName)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{queueName}"));

            await endpoint.SendBatch(messages);
        }

        public void SendBatch(IEnumerable<IMessage> messages, string queueName)
        {
            var endpoint =  _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{queueName}")).Result;

             endpoint.SendBatch(messages).Wait();
        }
    }
}
