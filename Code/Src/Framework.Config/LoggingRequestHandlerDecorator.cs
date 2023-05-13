using System.Threading;
using System.Threading.Tasks;
using Framework.Application.Contracts;
using Framework.Core.Logging;

namespace Framework.Config;

public class LoggingRequestHandlerDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest
{
    private readonly IRequestHandler<TRequest, TResponse> _requestHandler;
    private readonly ILogger _logger;
    public LoggingRequestHandlerDecorator(IRequestHandler<TRequest, TResponse> requestHandler, ILogger logger)
    {
        _requestHandler = requestHandler;
        _logger = logger;
    }
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        _logger.Write("Request is : {@Request}", LogLevel.Information, request);
        return await _requestHandler.Handle(request, cancellationToken);
    }
}