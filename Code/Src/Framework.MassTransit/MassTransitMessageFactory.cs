using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Messages;
using MassTransit;
using Newtonsoft.Json;

namespace Framework.MassTransit
{
	public class MassTransitMessageFactory
	{
		public static MassTransitMessage CreateFromMassTransitContext(ConsumeContext<IMessage> context)
		{
			var message = GetMessage(context);
			message.MessageType = RemoveBaseTypes(message.MessageType);
			message.MessageType = ConvertTypesToNamespaceNames(message.MessageType);
			return message;
		}

		private static MassTransitMessage GetMessage(ConsumeContext<IMessage> context)
		{
			var body = context.ReceiveContext.GetBody();
			return JsonConvert.DeserializeObject<MassTransitMessage>(Encoding.UTF8.GetString(body));
		}
		private static IList<string> RemoveBaseTypes(IList<string> messageMessageType)
		{
			return messageMessageType.Where(a => !BaseEventTypes.TypeNames.Any(b => b.Equals(a))).ToList();
		}
		private static IList<string> ConvertTypesToNamespaceNames(IList<string> messageTypes)
		{
			return messageTypes
				.Select(a =>
					a.Replace("urn:message:", "")
						.Replace(":", "."))
				.ToList();
		}
	}
}
