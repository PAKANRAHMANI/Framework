using System;
using Framework.Config;
using Framework.Kafka.Configurations;

namespace Framework.Kafka;

public class KafkaProducerModule : IFrameworkModule
{
    private readonly ProducerConfiguration _configurations;

    public KafkaProducerModule(Action<ProducerConfiguration> config)
    {
        _configurations = new ProducerConfiguration();

        config?.Invoke(_configurations);
    }
    public void Register(IDependencyRegister dependencyRegister)
    {
        dependencyRegister.RegisterScoped(typeof(ProducerConfiguration), _configurations);

        dependencyRegister.RegisterKafkaProducer();

    }
}