namespace Framework.RabbitMQ;

public interface IAcknowledgeHandler
{
	void Handle(AcknowledgeReceived acknowledge);
}
