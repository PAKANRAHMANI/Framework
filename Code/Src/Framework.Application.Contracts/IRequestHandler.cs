using System.Threading;
using System.Threading.Tasks;

namespace Framework.Application.Contracts;

public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}