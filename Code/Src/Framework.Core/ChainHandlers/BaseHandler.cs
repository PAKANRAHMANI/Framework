namespace Framework.Core.ChainHandlers
{
    public abstract class BaseHandler<TData, TResult> : IHandler<TData, TResult>
    {
        private IHandler<TData, TResult> _nextHandler;

        public abstract TResult Handle(TData data);

        protected TResult CallNext(TData data)
        {
            return  _nextHandler.Handle(data);
        }

        public void SetNext(IHandler<TData, TResult> handler)
        {
            this._nextHandler = handler;
        }
    }

    public abstract class BaseHandler<T> : IHandler<T>
    {
        private IHandler<T> _nextHandler;

        public abstract object Handle(T request);

        public void SetNext(IHandler<T> handler)
        {
            this._nextHandler = handler;
        }

        protected object CallNext(T request)
        {
            return _nextHandler?.Handle(request);
        }
    }
}