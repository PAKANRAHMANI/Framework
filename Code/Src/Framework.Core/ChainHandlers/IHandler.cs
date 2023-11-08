namespace Framework.Core.ChainHandlers
{
    public interface IHandler<T>
    {
        object Handle(T request);
        void SetNext(IHandler<T> handler);
    }
}
