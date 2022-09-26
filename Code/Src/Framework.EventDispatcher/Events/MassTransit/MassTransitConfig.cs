namespace Framework.EventProcessor.Events.MassTransit;

public class MassTransitConfig
{
    public string RabbitMqConnectionString { get; set; }
    public string BusEndpointQueueName { get; set; }
}