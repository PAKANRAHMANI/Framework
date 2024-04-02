using System;
using Framework.Config;
using Framework.Kafka.Configurations;

namespace Framework.Kafka
{
    public class KafkaConsumerModule : IFrameworkModule
    {
        private readonly ConsumerConfiguration _configurations;

        public KafkaConsumerModule(Action<ConsumerConfiguration> config)
        {
            _configurations = new ConsumerConfiguration();

            config?.Invoke(_configurations);
        }
        public void Register(IDependencyRegister dependencyRegister)
        {
            dependencyRegister.RegisterScoped(typeof(ConsumerConfiguration), _configurations);

            dependencyRegister.RegisterKafkaConsumer();

            dependencyRegister.RegisterSingleton<IOffsetManager,OffsetManager>();
        }
    }
}
