namespace Framework.EventProcessor.Configurations;

public class MassTransitConfig
{
    public string RabbitMqConnectionString { get; set; }
    public string RabbitMqUserName { get; set; }
    public string RabbitMqPassword { get; set; }
}