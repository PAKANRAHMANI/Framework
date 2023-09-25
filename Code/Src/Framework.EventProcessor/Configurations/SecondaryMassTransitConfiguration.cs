namespace Framework.EventProcessor.Configurations;

public class SecondaryMassTransitConfiguration
{
    public string RabbitMqConnectionString { get; set; }
    public string RabbitMqUserName { get; set; }
    public string RabbitMqPassword { get; set; }
}