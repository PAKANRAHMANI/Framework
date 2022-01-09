using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Query
{
    public interface IQueryHandlerResolver
    {
        IQueryHandler<TRequest,TResponse> ResolveHandlers<TRequest, TResponse>(TRequest request) where TRequest : IQuery;
    }
}
