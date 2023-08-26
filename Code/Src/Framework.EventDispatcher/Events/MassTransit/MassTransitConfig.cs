namespace Framework.EventProcessor.Events.MassTransit;

public class MassTransitConfig
{
    public string RabbitMqConnectionString { get; set; }
    public string RabbitMqUserName { get; set; }
    public string RabbitMqPassword { get; set; }
}