using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.Topology;
using Framework.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using IMessage = Framework.Messages.IMessage;

namespace Framework.EasyNetQ
{
	public class EasyNetQMessageSender : IMessageSender
	{
		private readonly IBus _bus;
		public EasyNetQMessageSender(IBus bus)
		{
			this._bus = bus;
		}

		public async Task Send(IMessage message, string queueName, Priority priority)
		{
			var messageContext = new MessageContext
			{
				CorrelationId = Guid.NewGuid().ToString(),
				MessageType = BaseEventTypes.TypeNames,
				Message = message
			};

			var jsonSerializerSettings = new JsonSerializerSettings
			{
				Formatting = Formatting.Indented,
				ContractResolver = new CamelCasePropertyNamesContractResolver()
			};

			var jsonBody = JsonConvert.SerializeObject(messageContext, jsonSerializerSettings);

			await _bus.Advanced.PublishAsync(
				new Exchange(queueName),
				"",
				false,
				new MessageProperties
				{
					Priority = (byte)priority,
				},
				Encoding.UTF8.GetBytes(jsonBody)
			);
		}

		public async Task SendBatch(IEnumerable<IMessage> messages, string queueName, Priority priority)
		{
			foreach (var message in messages)
			{
				await this.Send(message, queueName, priority);
			}
		}
	}
}