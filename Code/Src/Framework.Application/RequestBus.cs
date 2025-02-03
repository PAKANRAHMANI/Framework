using Framework.Application.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Application
{
    public class RequestBus(IRequestHandlerResolver resolver) : IRequestBus
    {
        public async Task<TResponse> Dispatch<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken) where TRequest : class, IRequest
        {
            var handler = resolver.ResolveHandler<TRequest, TResponse>(request);

            return await handler.Handle(request, cancellationToken);
        }
    }
}
