using System;

namespace Framework.Messages
{
	public class ProviderMessage : IMessage
	{
		public Guid MessageId { get; set; }
		public DateTime PublishDateTime { get; set; }
		public long NotificationId { get; set; }
		public long ChannelId { get; set; }
		public long ReceiverId { get; set; }
		public string ReceiverAddress { get; set; }
		public string Subject { get; set; }
		public string Content { get; set; }
	}
}
