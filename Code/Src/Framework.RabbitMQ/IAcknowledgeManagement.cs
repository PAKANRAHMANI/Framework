namespace Framework.RabbitMQ;

public interface IAcknowledgeManagement
{
	void Subscribe(IAcknowledgeHandler handler);
	void Publish(AcknowledgeReceived acknowledge);
}
