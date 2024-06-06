namespace Framework.EventProcessor.Configurations;

public sealed record SecondaryMassTransitConfiguration
{
    public string RabbitMqConnectionString { get; set; }
}