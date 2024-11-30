using System.Threading.Tasks;

namespace Framework.Core.ChainHandlers
{
    public interface IHandler<TData, TResult>
    {
        TResult Handle(TData data);
        void SetNext(IHandler<TData, TResult> handler);
    }

    public interface IHandler<T>
    {
        object Handle(T request);
        void SetNext(IHandler<T> handler);
    }
}