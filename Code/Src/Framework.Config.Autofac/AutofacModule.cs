using System;
using Autofac;
using Framework.Application.Contracts;
using Framework.Query;

namespace Framework.Config.Autofac
{
    public class AutofacModule : IFrameworkIocModule
    {
        private readonly ContainerBuilder _builder;
        public AutofacModule(ContainerBuilder builder)
        {
            _builder = builder;
        }
        public void Register(IDependencyRegister dependencyRegister)
        {
            dependencyRegister.RegisterScoped<ICommandHandlerResolver, AutofacCommandHandlerResolver>();
            dependencyRegister.RegisterScoped<IRequestHandlerResolver, AutofacRequestHandlerResolver>();
            dependencyRegister.RegisterScoped<IQueryHandlerResolver, AutofacQueryHandlerResolver>();
        }

        public IDependencyRegister CreateServiceRegistry()
        {
            return new AutofacDependencyRegister(_builder);
        }
    }
}
