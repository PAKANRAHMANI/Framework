using System.Collections.Generic;
using Framework.Messages;

namespace Framework.RabbitMQ;

public class AcknowledgeReceived
{
	public ulong Acknowledge { get; set; }
	public List<MessageContext> Messages { get; set; }
}
