using System.Threading;
using System.Threading.Tasks;

namespace Framework.Query
{
    public interface IQueryBus
    {
        Task<TResponse> Execute<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IQuery;
    }
}
