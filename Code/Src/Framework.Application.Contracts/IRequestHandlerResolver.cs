using System.Collections.Generic;

namespace Framework.Application.Contracts;

public interface IRequestHandlerResolver
{
    IRequestHandler<TRequest, TResponse> ResolveHandler<TRequest, TResponse>(TRequest request) where TRequest : IRequest;
}