using Castle.Core.Configuration;
using Framework.Config;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Kafka.Configurations;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Framework.Domain;

namespace Framework.Kafka
{
    public static class KafkaExtensions
    {
        public static IServiceCollection AddKafkaProducer(this IServiceCollection services, Action<ProducerConfiguration> options)
        {
            var redisCacheConfiguration = new ProducerConfiguration();

            options.Invoke(redisCacheConfiguration);

            services.AddSingleton(redisCacheConfiguration);

            services.TryAddSingleton(typeof(IKafkaProducer<,>), typeof(KafkaProducer<,>));

            return services;
        }
        public static IServiceCollection AddKafkaConsumer(this IServiceCollection services, Action<ConsumerConfiguration> options)
        {
            var redisCacheConfiguration = new ConsumerConfiguration();

            options.Invoke(redisCacheConfiguration);

            services.AddSingleton(redisCacheConfiguration);

            services.TryAddSingleton(typeof(IKafkaConsumer<,>), typeof(KafkaConsumer<,>));

            services.TryAddSingleton<IOffsetManager, OffsetManager>();

            return services;
        }
    }
}
