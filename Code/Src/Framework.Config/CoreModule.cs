using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Application;
using Framework.Application.Contracts;
using Framework.Core.Clock;
using Framework.Core.Events;
using Framework.Query;

namespace Framework.Config
{
    public class CoreModule : IFrameworkModule
    {
        public void Register(IDependencyRegister dependencyRegister)
        {
            dependencyRegister.RegisterScoped<IQueryBus, QueryBus>();
            dependencyRegister.RegisterSingleton<IDateTime, SystemDateTime>();
            dependencyRegister.RegisterScoped<IEventAggregator, EventAggregator>();
            dependencyRegister.RegisterScoped<IEventListener, EventListener>();
            dependencyRegister.RegisterScoped<IEventPublisher, EventPublisher>();
            dependencyRegister.RegisterScoped<ICommandBus, CommandBus>();
            dependencyRegister.RegisterDecorator(typeof(ICommandHandler<>), typeof(LoggingCommandHandlerDecorator<>));
        }
    }
}
