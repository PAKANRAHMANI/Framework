using Framework.Application;
using Framework.Application.Contracts;
using Framework.Core.Clock;
using Framework.Core.Events;
using Framework.Domain;
using Framework.Query;

namespace Framework.Config
{
    public class CoreModule : IFrameworkModule
    {
        private readonly FrameworkConfiguration _frameworkConfiguration;

        public CoreModule(FrameworkConfiguration frameworkConfiguration)
        {
            _frameworkConfiguration = frameworkConfiguration;
        }

        public void Register(IDependencyRegister dependencyRegister)
        {
            dependencyRegister.RegisterScoped<IQueryBus, QueryBus>();
            dependencyRegister.RegisterSingleton<IDateTime, SystemDateTime>();
            dependencyRegister.RegisterScoped<IEventAggregator, EventAggregator>();
            dependencyRegister.RegisterScoped<IEventListener, EventListener>();
            dependencyRegister.RegisterScoped<IEventPublisher, EventPublisher>();
            dependencyRegister.RegisterScoped<ICommandBus, CommandBus>();
            dependencyRegister.RegisterScoped<IRequestBus, RequestBus>();
            dependencyRegister.RegisterScoped<IAggregateRootConfigurator, AggregateRootConfigurator>();


            if (_frameworkConfiguration.UseUnitOfWork)
            {
                dependencyRegister.RegisterDecorator(typeof(IRequestHandler<,>), typeof(TransactionalRequestHandlerDecorator<,>));

                dependencyRegister.RegisterDecorator(typeof(ICommandHandler<>), typeof(TransactionalCommandHandlerDecorator<>));
            }

            if (_frameworkConfiguration.EnableLogInRequest)
                dependencyRegister.RegisterDecorator(typeof(IRequestHandler<,>), typeof(LoggingRequestHandlerDecorator<,>));

            if (_frameworkConfiguration.EnableLogInCommand)
                dependencyRegister.RegisterDecorator(typeof(ICommandHandler<>), typeof(LoggingCommandHandlerDecorator<>));
        }
    }
}
