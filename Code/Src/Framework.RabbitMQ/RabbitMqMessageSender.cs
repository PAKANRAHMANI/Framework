using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Framework.RabbitMQ
{
	public sealed class RabbitMqMessageSender : IRabbitMqMessageSender
	{
		private readonly IModel _channel;
		private readonly IAcknowledgeManagement _acknowledgeManagement;
		private EventHandler<BasicAckEventArgs> _acknowledgeEventHandler;
		public RabbitMqMessageSender(IAcknowledgeManagement acknowledgeManagement)
		{
			var factory = new ConnectionFactory { Uri = new Uri("amqp://admin:admin@86.104.46.59:25672") };
			var connection = factory.CreateConnection();
			_channel = connection.CreateModel();
			_channel.ConfirmSelect();
			_channel.WaitForConfirmsOrDie(new TimeSpan(0, 0, 5));
			_acknowledgeManagement = acknowledgeManagement;
		}

		public void SendBatch(IEnumerable<IMessage> messages, string queueName, Priority priority)
		{
			var messageContexts = messages.Select(message => new MessageContext
			{
				CorrelationId = Guid.NewGuid().ToString(),
				MessageType = BaseEventTypes.TypeNames,
				Message = message
			}).ToList();

			var jsonSerializerSettings = new JsonSerializerSettings
			{
				Formatting = Formatting.Indented,
				ContractResolver = new CamelCasePropertyNamesContractResolver()
			};

			var basicProperties = _channel.CreateBasicProperties();

			basicProperties.Priority = (byte)priority;

			var batchPublish = _channel.CreateBasicPublishBatch();

			messageContexts.ForEach(message =>
			{
				var jsonBody = JsonConvert.SerializeObject(message, jsonSerializerSettings);

				var body = Encoding.UTF8.GetBytes(jsonBody);

				batchPublish.Add(exchange: queueName, routingKey: string.Empty, false, basicProperties, new ReadOnlyMemory<byte>(body));
			});

			batchPublish.Publish();

			if (_acknowledgeEventHandler != null)
			{
				_channel.BasicAcks -= _acknowledgeEventHandler;
			}

			_acknowledgeEventHandler = (sender, args) =>
			{
				_acknowledgeManagement.Publish(new AcknowledgeReceived
				{
					Acknowledge = args.DeliveryTag,
					Messages = messageContexts
				});
			};

			_channel.BasicAcks += _acknowledgeEventHandler;
		}
	}
}