using System.Collections.Generic;
using Framework.Messages;

namespace Framework.RabbitMQ
{
    public interface IRabbitMqMessageSender
    {
        void SendBatch(IEnumerable<IMessage> messages, string queueName, Priority priority);
    }
}
