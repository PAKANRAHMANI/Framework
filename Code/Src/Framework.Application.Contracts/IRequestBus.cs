using System.Threading;
using System.Threading.Tasks;

namespace Framework.Application.Contracts;

public interface IRequestBus
{
    Task<TResponse> Dispatch<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken) where TRequest : class, IRequest;
}