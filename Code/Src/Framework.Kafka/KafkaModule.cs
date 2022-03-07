using System;
using Framework.Config;

namespace Framework.Kafka
{
    public class KafkaModule : IFrameworkModule
    {
        private readonly KafkaConfiguration _configuration;

        public KafkaModule(Action<KafkaConfiguration> config)
        {
            _configuration = new KafkaConfiguration();

            config?.Invoke(_configuration);
        }
        public void Register(IDependencyRegister dependencyRegister)
        {
            dependencyRegister.RegisterScoped(typeof(KafkaConfiguration), _configuration);

            dependencyRegister.RegisterKafka();
        }
    }
}
