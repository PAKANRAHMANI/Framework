using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Framework.Config
{
    public interface IDependencyRegister
    {
        void RegisterCommandHandlers(Assembly assembly);
        void RegisterRepositories(Assembly assembly);
        void RegisterFacades(Assembly assembly);
        void RegisterDomainServices(Assembly assembly);
        void RegisterQueryHandlers(Assembly assembly);
        void RegisterScoped<TService>(Func<TService> factory, Action<TService> release = null);
        void RegisterScoped<TService>(Type implementationType);
        void RegisterScoped(Type implementationType);
        void RegisterScoped<TService, TImplementation>() where TImplementation : TService;
        void RegisterSingleton<TService, TImplementation>() where TImplementation : TService;
        void RegisterSingleton<TService, TInstance>(TInstance instance) where TService : class where TInstance : TService;
        void RegisterTransient<TService, TImplementation>() where TImplementation : TService;
        void RegisterDecorator<TService, TDecorator>() where TDecorator : TService;
        void RegisterDecorator(Type service, Type decorator);
    }
}
