using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Query
{
    public class QueryBus : IQueryBus
    {
        private readonly IQueryHandlerResolver _handlerResolver;

        public QueryBus(IQueryHandlerResolver handlerResolver)
        {
            _handlerResolver = handlerResolver;
        }
        public async Task<TResponse> Execute<TRequest, TResponse>(TRequest request) where TRequest : IQuery
        {
            var handler = _handlerResolver.ResolveHandlers<TRequest, TResponse>(request);
            return await handler.Handle(request);
        }
    }
}
