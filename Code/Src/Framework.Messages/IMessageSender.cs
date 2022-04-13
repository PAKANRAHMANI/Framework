using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.Messages
{
    public interface IMessageSender
    {
        public Task SendAsync(IMessage message, string exchangeName, string exchangeType, Priority priority);
        public void Send(IMessage message, string exchangeName, string exchangeType, Priority priority);
        public Task SendAsync(IMessage message, string exchangeName, string exchangeType);
        public void Send(IMessage message, string exchangeName, string exchangeType);
        public Task SendBatchAsync(IEnumerable<IMessage> messages, string exchangeName, string exchangeType, Priority priority);
        public void SendBatch(IEnumerable<IMessage> messages, string exchangeName, string exchangeType, Priority priority);
        public Task SendBatchAsync(IEnumerable<IMessage> messages, string exchangeName, string exchangeType);
        public void SendBatch(IEnumerable<IMessage> messages, string exchangeName, string exchangeType);
    }
}
