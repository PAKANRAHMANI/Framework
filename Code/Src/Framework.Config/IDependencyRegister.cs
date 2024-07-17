using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Framework.Config
{
    public interface IDependencyRegister
    {
        void RegisterCommandHandlers(Assembly assembly);
        void RegisterRequestHandlers(Assembly assembly);
        void RegisterRepositories(Assembly assembly);
        void RegisterFacades(Assembly assembly);
        void RegisterDomainServices(Assembly assembly);
        void RegisterQueryHandlers(Assembly assembly);
        void RegisterScoped<TService>(Func<TService> factory, Action<TService> release = null);
        void RegisterScoped<TService>(Type implementationType);
        void RegisterScoped(Type implementationType);
        void RegisterScoped<TService, TImplementation>() where TImplementation : TService;
        void RegisterSingleton<TService>(Func<TService> factory, Action<TService> release = null);
        void RegisterSingleton<TService, TImplementation>() where TImplementation : TService;
        void RegisterSingleton<TService, TInstance>(TInstance instance) where TService : class where TInstance : TService;
        void RegisterTransient<TService, TImplementation>() where TImplementation : TService;
        void RegisterTransient<TService>(Func<TService> factory, Action<TService> release = null);
        void RegisterDecorator<TService, TDecorator>() where TDecorator : TService;
        void RegisterSingletonServiceWithInterceptor<TService, TImplementation>(Type implementationType) where TImplementation : TService;
        void RegisterDecorator(Type service, Type decorator);
        void RegisterScoped(Type implementationType, object config);
        void RegisterSingleton(Type implementationType, object config);
        void RegisterKafkaProducer();
        void RegisterKafkaConsumer();
    }
}
