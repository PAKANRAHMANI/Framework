using System.Threading;
using System.Threading.Tasks;

namespace Framework.Query
{
    public class QueryBus(IQueryHandlerResolver handlerResolver) : IQueryBus
    {
        public async Task<TResponse> Execute<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IQuery
        {
            var handler = handlerResolver.ResolveHandlers<TRequest, TResponse>(request);
            return await handler.Handle(request, cancellationToken);
        }
    }
}
