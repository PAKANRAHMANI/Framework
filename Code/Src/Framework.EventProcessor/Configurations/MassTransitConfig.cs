namespace Framework.EventProcessor.Configurations;

public sealed record MassTransitConfig
{
    public string RabbitMqConnectionString { get; set; }
}