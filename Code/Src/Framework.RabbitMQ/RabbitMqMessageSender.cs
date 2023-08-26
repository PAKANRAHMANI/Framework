using Framework.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Options;

namespace Framework.RabbitMQ;

public sealed class RabbitMqMessageSender : IRabbitMqMessageSender
{
	private readonly IModel _channel;
	private readonly IAcknowledgeManagement _acknowledgeManagement;
    private readonly JsonSerializerSettings _jsonSerializerSettings;
	private EventHandler<BasicAckEventArgs> _acknowledgeEventHandler;
    
    public RabbitMqMessageSender(IAcknowledgeManagement acknowledgeManagement, RabbitConfiguration rabbitConfig)
	{
		var factory = new ConnectionFactory { Uri = new Uri(rabbitConfig.Host) };
		var connection = factory.CreateConnection();
		_channel = connection.CreateModel();
		_channel.ConfirmSelect();
		_channel.WaitForConfirmsOrDie(TimeSpan.FromMilliseconds(1000));
		_acknowledgeManagement = acknowledgeManagement;
      

        _jsonSerializerSettings = new JsonSerializerSettings
		{
			Formatting = Formatting.Indented,
			ContractResolver = new CamelCasePropertyNamesContractResolver()
		};
	}

	public void Send<T>(T message, string exchange, byte priority = 0) where T : IMessage
	{
		var messageContext = new MessageContext
		{
			CorrelationId = Guid.NewGuid().ToString(),
			MessageType = BaseEventTypes.TypeNames,
			Message = message
		};

		var basicProperties = _channel.CreateBasicProperties();

		basicProperties.Priority = priority;

		var batchPublish = _channel.CreateBasicPublishBatch();

		var jsonBody = JsonConvert.SerializeObject(messageContext, _jsonSerializerSettings);

		var body = Encoding.UTF8.GetBytes(jsonBody);

		batchPublish.Add(exchange: exchange, routingKey: string.Empty, false, basicProperties, new ReadOnlyMemory<byte>(body));

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
				Messages = new List<MessageContext> { messageContext }
			});
		};

		_channel.BasicAcks += _acknowledgeEventHandler;
	}

	public void SendBatch(IEnumerable<IMessage> messages, string exchange, byte priority = 0)
	{
		var messageContexts = messages.Select(message => new MessageContext
		{
			CorrelationId = Guid.NewGuid().ToString(),
			MessageType = BaseEventTypes.TypeNames,
			Message = message
		}).ToList();

		var basicProperties = _channel.CreateBasicProperties();

		basicProperties.Priority = (byte)priority;

		var batchPublish = _channel.CreateBasicPublishBatch();

		messageContexts.ForEach(message =>
		{
			var jsonBody = JsonConvert.SerializeObject(message, _jsonSerializerSettings);

			var body = Encoding.UTF8.GetBytes(jsonBody);

			batchPublish.Add(exchange: exchange, routingKey: string.Empty, false, basicProperties, new ReadOnlyMemory<byte>(body));
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
