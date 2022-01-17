using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.Messages
{
	public interface IMessageSender
	{
		public Task Send(IMessage message, string queueName, Priority priority);
		public Task Send(IMessage message, string queueName);
        public Task SendBatch(IEnumerable<IMessage> messages, string queueName, Priority priority);
	}
}
