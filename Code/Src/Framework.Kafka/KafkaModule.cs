using System;
using Framework.Config;

namespace Framework.Kafka
{
    public class KafkaModule : IFrameworkModule
    {
        private readonly KafkaConfigurations _configurations;

        public KafkaModule(Action<KafkaConfigurations> config)
        {
            _configurations = new KafkaConfigurations();

            config?.Invoke(_configurations);
        }
        public void Register(IDependencyRegister dependencyRegister)
        {
            dependencyRegister.RegisterScoped(typeof(KafkaConfigurations), _configurations);

            dependencyRegister.RegisterKafka();

            dependencyRegister.RegisterSingleton<IOffsetManager,OffsetManager>();
        }
    }
}
