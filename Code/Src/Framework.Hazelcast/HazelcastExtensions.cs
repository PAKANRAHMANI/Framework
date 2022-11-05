using System.Reflection;
using Hazelcast;
using Hazelcast.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Hazelcast
{
    public static class HazelcastExtensions
    {
        public static IServiceCollection AddHazelcast(this IServiceCollection services, Action<HazelcastConfiguration> options)
        {
            var hazelcastConfiguration = new HazelcastConfiguration();

            options.Invoke(hazelcastConfiguration);

            var serializedTypes =
                hazelcastConfiguration.Assembly.GetTypes()
                    .Where(type => type.IsAssignableTo(typeof(IHazelcastSerializer)))
                    .Where(type => !type.IsAbstract)
                    .Where(type => type.IsClass)
                    .Where(type => type.IsInterface == false)
                    .ToList();

            var serializedObjects = (from type in serializedTypes
                                                         let instance = (dynamic)Activator.CreateInstance(type)
                                                         select new HazelcastSerialize()
                                                         {
                                                             Object = SerializerDeserializerExtensions.CreateCustomSerializer(instance),
                                                             Type = type
                                                         }).ToList();

            services.AddSingleton(serializedObjects);

            services.Configure<HazelcastConfiguration>(options);

            services.AddSingleton<IHazelcast, Hazelcast>();

            return services;
        }
    }
}
