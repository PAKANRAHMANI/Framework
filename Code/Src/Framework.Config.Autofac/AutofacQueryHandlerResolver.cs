using Autofac;
using Framework.Query;

namespace Framework.Config.Autofac
{
    public class AutofacQueryHandlerResolver(ILifetimeScope lifetimeScope) : IQueryHandlerResolver
    {
        public IQueryHandler<TRequest, TResponse> ResolveHandlers<TRequest, TResponse>(TRequest request) where TRequest : IQuery
        {
            return lifetimeScope.Resolve<IQueryHandler<TRequest, TResponse>>();
        }
    }
}
