using System.Collections.Generic;
using Framework.Messages;

namespace Framework.RabbitMQ;

public interface IRabbitMqMessageSender
{
	void Send<T>(T message, string exchange, byte priority = 0) where T : IMessage;
	void SendBatch(IEnumerable<IMessage> messages, string exchange, byte priority = 0);
}
