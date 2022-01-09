using System.Threading.Tasks;

namespace Framework.Query
{
    public interface IQueryHandler<in TRequest, TResponse> where TRequest : IQuery
    {
        Task<TResponse> Handle(TRequest request);
    }
}