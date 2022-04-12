using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.Messages
{
    public interface IMessageSender
    {
        public Task SendAsync(IMessage message, string exchangeName, Priority priority);
        public void Send(IMessage message, string exchangeName, Priority priority);
        public Task SendAsync(IMessage message, string exchangeName);
        public void Send(IMessage message, string exchangeName);
        public Task SendBatchAsync(IEnumerable<IMessage> messages, string exchangeName, Priority priority);
        public void SendBatch(IEnumerable<IMessage> messages, string exchangeName, Priority priority);
        public Task SendBatchAsync(IEnumerable<IMessage> messages, string exchangeName);
        public void SendBatch(IEnumerable<IMessage> messages, string exchangeName);
    }
}
