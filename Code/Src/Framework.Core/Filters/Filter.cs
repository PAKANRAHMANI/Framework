namespace Framework.Core.Filters
{
    public class Filter<T> : IFilter<T>
    {
        private readonly IOperation<T> _operation;
        private IFilter<T> _nextFilter;

        public Filter(IOperation<T> operation)
        {
            _operation = operation;
            _nextFilter = EndFilter<T>.Instance;
        }
        public void SetNext(IFilter<T> next)
        {
            this._nextFilter = next;
        }

        public T Apply(T obj)
        {
            obj = _operation.Apply(obj);

            return _nextFilter.Apply(obj);
        }

    }
}