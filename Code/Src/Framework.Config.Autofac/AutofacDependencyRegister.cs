using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Confluent.Kafka;
using Framework.Application.Contracts;
using Framework.Core;
using Framework.Domain;
using Framework.Kafka;
using Framework.Query;
using MassTransit;


namespace Framework.Config.Autofac
{
    public class AutofacDependencyRegister : IDependencyRegister
    {
        private readonly ContainerBuilder _container;

        public AutofacDependencyRegister(ContainerBuilder container)
        {
            _container = container;
        }
        public void RegisterCommandHandlers(Assembly assembly)
        {
            _container.RegisterAssemblyTypes(assembly)
                .As(type => type.GetInterfaces()
                .Where(interfaceType => interfaceType.IsClosedTypeOf(typeof(ICommandHandler<>))))
                .InstancePerLifetimeScope();
        }

        public void RegisterRequestHandlers(Assembly assembly)
        {
            _container
                .RegisterAssemblyTypes(assembly)
                .As(type => type.GetInterfaces()
                    .Where(interfaceType => interfaceType
                        .IsClosedTypeOf(typeof(IRequestHandler<,>))))
                .InstancePerLifetimeScope();
        }

        public void RegisterQueryHandlers(Assembly assembly)
        {
            _container
                .RegisterAssemblyTypes(assembly)
                .AsClosedTypesOf(typeof(IQueryHandler<,>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }

        public void RegisterScoped<TService>(Func<TService> factory, Action<TService> release = null)
        {
            var registration = _container.Register(a => factory.Invoke()).InstancePerLifetimeScope();
            if (release != null)
                registration.OnRelease(release);
        }

        public void RegisterScoped<TService>(Type implementationType)
        {
            _container.RegisterType(implementationType).As<TService>().InstancePerLifetimeScope();
        }

        public void RegisterScoped(Type implementationType)
        {
            _container.RegisterType(implementationType).InstancePerLifetimeScope();
        }

        public void RegisterScoped<TService, TImplementation>() where TImplementation : TService
        {
            _container.RegisterType<TImplementation>().As<TService>().InstancePerLifetimeScope();
        }

        public void RegisterSingleton<TService>(Func<TService> factory, Action<TService> release = null)
        {
            var registration = _container.Register(a => factory.Invoke()).SingleInstance();

            if (release != null)
                registration.OnRelease(release);
        }

        public void RegisterSingleton<TService, TImplementation>() where TImplementation : TService
        {
            _container.RegisterType<TImplementation>().As<TService>().SingleInstance();
        }

        public void RegisterSingleton<TService, TInstance>(TInstance instance) where TService : class where TInstance : TService
        {
            _container.RegisterInstance<TService>(instance).SingleInstance();
        }

        public void RegisterTransient<TService, TImplementation>() where TImplementation : TService
        {
            _container.RegisterType<TImplementation>().As<TService>().InstancePerDependency();
        }

        public void RegisterDecorator<TService, TDecorator>() where TDecorator : TService
        {
            _container.RegisterDecorator<TDecorator, TService>();
        }

        public void RegisterSingletonServiceWithInterceptor<TService, TImplementation>(Type interceptorType, object interceptor) where TImplementation : TService
        {
            _container.Register(p => interceptor).As(interceptorType).SingleInstance();

            _container.RegisterType<TImplementation>().As<TService>().SingleInstance().EnableInterfaceInterceptors().InterceptedBy(interceptorType);
        }

        public void RegisterDecorator(Type service, Type decorator)
        {
            _container.RegisterGenericDecorator(decorator, service);
        }

        public void RegisterScoped(Type implementationType, object config)
        {
            _container.Register(p => config).As(implementationType).InstancePerLifetimeScope();
        }

        public void RegisterSingleton(Type implementationType, object config)
        {
            _container.Register(p => config).As(implementationType).SingleInstance();
        }

        public void RegisterRepositories(Assembly assembly)
        {
            _container.RegisterAssemblyTypes(assembly)
                .Where(type => typeof(IRepository).IsAssignableFrom(type))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }

        public void RegisterFacades(Assembly assembly)
        {
            _container.RegisterAssemblyTypes(assembly)
                .Where(a => typeof(IFacadeService).IsAssignableFrom(a))
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InstancePerLifetimeScope();
        }

        public void RegisterDomainServices(Assembly assembly)
        {
            _container.RegisterAssemblyTypes(assembly)
                .Where(a => typeof(IDomainService).IsAssignableFrom(a))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }

        public void RegisterKafkaConsumer()
        {

            _container
                .RegisterGeneric(typeof(KafkaConsumer<,>))
                .As(typeof(IKafkaConsumer<,>))
                .SingleInstance();
        }
        public void RegisterKafkaProducer()
        {
            _container
                .RegisterGeneric(typeof(KafkaProducer<,>))
                .As(typeof(IKafkaProducer<,>))
                .SingleInstance();

        }

        public void RegisterTransient<TService>(Func<TService> factory, Action<TService> release = null)
        {
            var registration = _container.Register(a => factory.Invoke()).InstancePerDependency();

            if (release != null)
                registration.OnRelease(release);
        }
    }
}
